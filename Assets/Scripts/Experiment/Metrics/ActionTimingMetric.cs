using System;
using System.Collections.Generic;

namespace Frezerka.Experiment.Metrics
{
    public class ActionTimingMetric
    {
        private Dictionary<string, StepData> _activeSteps = new Dictionary<string, StepData>();
        private Dictionary<string, float> _stepStartTimes = new Dictionary<string, float>();
        private Dictionary<string, int> _stepAttemptCounts = new Dictionary<string, int>();
        private string _currentStepId;

        public void Reset()
        {
            _activeSteps.Clear();
            _stepStartTimes.Clear();
            _stepAttemptCounts.Clear();
            _currentStepId = null;
        }

        public void OnStepEntered(string stepId, int stepIndex, float time)
        {
            _currentStepId = stepId;
            _stepStartTimes[stepId] = time;

            if (!_stepAttemptCounts.ContainsKey(stepId))
                _stepAttemptCounts[stepId] = 0;
            _stepAttemptCounts[stepId]++;

            if (!_activeSteps.ContainsKey(stepId))
            {
                _activeSteps[stepId] = new StepData
                {
                    stepId = stepId,
                    stepIndex = stepIndex,
                    startTimestamp = DateTime.UtcNow.ToString("o"),
                    interactions = new List<InteractionRecord>()
                };
            }
        }

        public StepData OnStepCompleted(string stepId, float duration)
        {
            if (!_activeSteps.ContainsKey(stepId)) return null;

            var data = _activeSteps[stepId];
            data.endTimestamp = DateTime.UtcNow.ToString("o");
            data.durationSeconds = duration;
            data.result = "Completed";
            data.attemptCount = _stepAttemptCounts.GetValueOrDefault(stepId, 1);

            _activeSteps.Remove(stepId);
            return data;
        }

        public void RecordInteraction(InteractionRecord record)
        {
            if (_currentStepId != null && _activeSteps.ContainsKey(_currentStepId))
            {
                _activeSteps[_currentStepId].interactions.Add(record);
            }
        }
    }
}
