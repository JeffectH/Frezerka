using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Frezerka.Experiment.Metrics;
using Frezerka.Utility;

namespace Frezerka.Experiment
{
    public class ExperimentDataCollector : SingletonMonoBehaviour<ExperimentDataCollector>
    {
        [Header("Settings")]
        [SerializeField] private float positionSampleInterval = 1f;
        [SerializeField] private float gazeSampleInterval = 0.5f;
        [SerializeField] private float autoSaveInterval = 60f;

        private ExperimentSessionData _sessionData;
        private ActionTimingMetric _timingMetric;
        private ErrorTrackingMetric _errorMetric;
        private GazeTrackingMetric _gazeMetric;
        private NavigationMetric _navigationMetric;
        private HesitationMetric _hesitationMetric;
        private SafetyViolationMetric _safetyMetric;

        private bool _isCollecting;
        private Transform _playerTransform;
        private float _nextPositionSample;
        private float _nextGazeSample;
        private float _nextAutoSave;

        public ExperimentSessionData SessionData => _sessionData;

        protected override void Awake()
        {
            base.Awake();
            _timingMetric = new ActionTimingMetric();
            _errorMetric = new ErrorTrackingMetric();
            _gazeMetric = new GazeTrackingMetric();
            _navigationMetric = new NavigationMetric();
            _hesitationMetric = new HesitationMetric();
            _safetyMetric = new SafetyViolationMetric();
        }

        private void OnEnable()
        {
            EventBus.Subscribe<SessionEvent>(OnSessionEvent);
            EventBus.Subscribe<InteractionEvent>(OnInteraction);
            EventBus.Subscribe<StepChangedEvent>(OnStepChanged);
            EventBus.Subscribe<StepCompletedEvent>(OnStepCompleted);
            EventBus.Subscribe<ErrorEvent>(OnError);
            EventBus.Subscribe<SafetyViolationEvent>(OnSafetyViolation);
            EventBus.Subscribe<GazeEvent>(OnGaze);
            EventBus.Subscribe<EmergencyEvent>(OnEmergency);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<SessionEvent>(OnSessionEvent);
            EventBus.Unsubscribe<InteractionEvent>(OnInteraction);
            EventBus.Unsubscribe<StepChangedEvent>(OnStepChanged);
            EventBus.Unsubscribe<StepCompletedEvent>(OnStepCompleted);
            EventBus.Unsubscribe<ErrorEvent>(OnError);
            EventBus.Unsubscribe<SafetyViolationEvent>(OnSafetyViolation);
            EventBus.Unsubscribe<GazeEvent>(OnGaze);
            EventBus.Unsubscribe<EmergencyEvent>(OnEmergency);
        }

        private void Update()
        {
            if (!_isCollecting) return;

            if (_playerTransform == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                    _playerTransform = player.transform;
                return;
            }

            // Position sampling
            if (Time.time >= _nextPositionSample)
            {
                _nextPositionSample = Time.time + positionSampleInterval;
                _navigationMetric.SamplePosition(_playerTransform.position, Time.time);
            }

            // Auto-save
            if (Time.time >= _nextAutoSave)
            {
                _nextAutoSave = Time.time + autoSaveInterval;
                AutoSave();
            }
        }

        private void OnSessionEvent(SessionEvent evt)
        {
            if (evt.EventType == SessionEvent.SessionEventType.Started)
            {
                StartCollection(evt.ParticipantId, evt.MachineType, evt.SessionMode);
            }
            else if (evt.EventType == SessionEvent.SessionEventType.Ended)
            {
                StopCollection();
            }
        }

        private void StartCollection(string participantId, string machineType, string sessionMode)
        {
            _sessionData = new ExperimentSessionData
            {
                sessionId = Guid.NewGuid().ToString(),
                participantId = participantId,
                machineType = machineType,
                sessionMode = sessionMode,
                startTimestampUTC = DateTime.UtcNow.ToString("o"),
                applicationVersion = Application.version
            };

            _sessionData.navigationData.sampleIntervalSeconds = positionSampleInterval;
            _sessionData.gazeData.sampleIntervalSeconds = gazeSampleInterval;

            _timingMetric.Reset();
            _errorMetric.Reset();
            _gazeMetric.Reset();
            _navigationMetric.Reset();
            _hesitationMetric.Reset();
            _safetyMetric.Reset();

            _isCollecting = true;
            _nextPositionSample = Time.time + positionSampleInterval;
            _nextAutoSave = Time.time + autoSaveInterval;

            Debug.Log($"[ExperimentDataCollector] Collection started: {_sessionData.sessionId}");
        }

        private void StopCollection()
        {
            if (!_isCollecting) return;
            _isCollecting = false;

            _sessionData.endTimestampUTC = DateTime.UtcNow.ToString("o");
            _sessionData.totalDurationSeconds =
                (float)(DateTime.UtcNow - DateTime.Parse(_sessionData.startTimestampUTC)).TotalSeconds;

            FinalizeData();
            SaveToFile();

            Debug.Log($"[ExperimentDataCollector] Collection stopped. Duration: {_sessionData.totalDurationSeconds:F1}s");
        }

        private void OnInteraction(InteractionEvent evt)
        {
            if (!_isCollecting) return;

            _hesitationMetric.RecordInteraction(evt.InteractionId, Time.time);

            var record = new InteractionRecord
            {
                timestamp = DateTime.UtcNow.ToString("o"),
                interactionId = evt.InteractionId,
                interactionType = evt.InteractionType,
                value = evt.Value,
                playerPosition = Vec3Data.From(evt.PlayerPosition),
                playerRotation = RotationData.From(evt.PlayerRotation)
            };

            _timingMetric.RecordInteraction(record);
        }

        private void OnStepChanged(StepChangedEvent evt)
        {
            if (!_isCollecting) return;

            _timingMetric.OnStepEntered(evt.NewStepId, evt.StepIndex, Time.time);
            _hesitationMetric.OnStepEntered(evt.NewStepId, Time.time);
        }

        private void OnStepCompleted(StepCompletedEvent evt)
        {
            if (!_isCollecting) return;

            var stepData = _timingMetric.OnStepCompleted(evt.StepId, evt.DurationSeconds);
            if (stepData != null)
            {
                stepData.hesitation = _hesitationMetric.GetHesitationData(evt.StepId);
                stepData.errors = _errorMetric.GetStepErrors(evt.StepId);
                _sessionData.steps.Add(stepData);
            }

            _sessionData.scenarioCompleted = true;
        }

        private void OnError(ErrorEvent evt)
        {
            if (!_isCollecting) return;
            _errorMetric.RecordError(evt.StepId, evt.ErrorType, evt.Details, evt.StepTimeAtError);
        }

        private void OnSafetyViolation(SafetyViolationEvent evt)
        {
            if (!_isCollecting) return;
            _safetyMetric.RecordViolation(evt.ViolationType, evt.ActiveStepId,
                evt.PlayerPosition, evt.Description);
        }

        private void OnGaze(GazeEvent evt)
        {
            if (!_isCollecting) return;
            _gazeMetric.RecordGaze(evt.TargetId, evt.Timestamp);
            _hesitationMetric.RecordGaze(evt.TargetId, evt.Timestamp);
        }

        private void OnEmergency(EmergencyEvent evt)
        {
            if (!_isCollecting) return;
            _sessionData.emergencyEvents.Add(new EmergencyEventData
            {
                timestamp = DateTime.UtcNow.ToString("o"),
                eventType = evt.EventType,
                reactionTimeSeconds = evt.ReactionTimeSeconds,
                handledCorrectly = evt.HandledCorrectly
            });
        }

        private void FinalizeData()
        {
            // Summary
            _sessionData.summary.totalSteps = _sessionData.steps.Count;
            _sessionData.summary.completedSteps =
                _sessionData.steps.Count(s => s.result == "Completed");
            _sessionData.summary.failedSteps =
                _sessionData.steps.Count(s => s.result == "Failed");
            _sessionData.summary.totalErrors =
                _sessionData.steps.Sum(s => s.errors.Count);
            _sessionData.summary.totalSafetyViolations = _sessionData.safetyViolations.Count;

            // Navigation
            _sessionData.navigationData = _navigationMetric.GetNavigationData();
            _sessionData.summary.totalDistanceWalkedMeters =
                _sessionData.navigationData.totalDistanceMeters;

            // Gaze
            _sessionData.gazeData = _gazeMetric.GetGazeData(gazeSampleInterval);

            // Safety violations
            _sessionData.safetyViolations = _safetyMetric.GetViolations();

            // Step timing stats
            if (_sessionData.steps.Count > 0)
            {
                var durations = _sessionData.steps
                    .Select(s => s.durationSeconds)
                    .OrderBy(d => d)
                    .ToList();

                _sessionData.summary.averageStepTimeSeconds = durations.Average();
                _sessionData.summary.medianStepTimeSeconds =
                    durations[durations.Count / 2];
            }
        }

        private void SaveToFile()
        {
            ExperimentFileWriter.Save(_sessionData);
        }

        private void AutoSave()
        {
            if (_sessionData == null) return;
            FinalizeData();
            ExperimentFileWriter.Save(_sessionData, "_autosave");
        }
    }
}
