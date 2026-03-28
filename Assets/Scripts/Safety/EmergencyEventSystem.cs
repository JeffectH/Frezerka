using UnityEngine;
using Frezerka.Utility;

namespace Frezerka.Safety
{
    public class EmergencyEventSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float minTimeBetweenEvents = 60f;
        [SerializeField] private float maxTimeBetweenEvents = 180f;
        [SerializeField] private bool enableRandomEmergencies = true;

        [Header("References")]
        [SerializeField] private SafetyWarningUI warningUI;

        private float _nextEventTime;
        private bool _emergencyActive;
        private float _emergencyStartTime;
        private string _currentEmergencyType;

        private readonly string[] _emergencyTypes = new[]
        {
            "tool_overload",
            "workpiece_jam",
            "overheating",
            "spindle_vibration",
            "coolant_failure"
        };

        private readonly string[] _emergencyMessagesRU = new[]
        {
            "Перегрузка режущего инструмента!",
            "Защемление заготовки!",
            "Перегрев инструмента!",
            "Повышенная вибрация шпинделя!",
            "Сбой системы охлаждения!"
        };

        private void Start()
        {
            ScheduleNextEvent();
        }

        private void Update()
        {
            if (!enableRandomEmergencies) return;

            if (!_emergencyActive && Time.time >= _nextEventTime)
            {
                TriggerRandomEmergency();
            }
        }

        private void ScheduleNextEvent()
        {
            _nextEventTime = Time.time + Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
        }

        private void TriggerRandomEmergency()
        {
            int index = Random.Range(0, _emergencyTypes.Length);
            _currentEmergencyType = _emergencyTypes[index];
            _emergencyActive = true;
            _emergencyStartTime = Time.time;

            warningUI?.ShowWarning(_emergencyMessagesRU[index]);

            Debug.Log($"[EmergencySystem] Emergency triggered: {_currentEmergencyType}");
        }

        /// <summary>
        /// Called when player responds to emergency (e.g., presses emergency stop).
        /// </summary>
        public void HandleEmergencyResponse()
        {
            if (!_emergencyActive) return;

            float reactionTime = Time.time - _emergencyStartTime;
            _emergencyActive = false;

            EventBus.Publish(new EmergencyEvent
            {
                EventType = _currentEmergencyType,
                ReactionTimeSeconds = reactionTime,
                HandledCorrectly = true
            });

            warningUI?.HideWarning();
            ScheduleNextEvent();

            Debug.Log($"[EmergencySystem] Handled in {reactionTime:F1}s");
        }

        /// <summary>
        /// Called if emergency times out without player response.
        /// </summary>
        public void EmergencyTimeout()
        {
            if (!_emergencyActive) return;

            float reactionTime = Time.time - _emergencyStartTime;
            _emergencyActive = false;

            EventBus.Publish(new EmergencyEvent
            {
                EventType = _currentEmergencyType,
                ReactionTimeSeconds = reactionTime,
                HandledCorrectly = false
            });

            ScheduleNextEvent();
        }
    }
}
