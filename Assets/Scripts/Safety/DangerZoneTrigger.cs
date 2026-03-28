using UnityEngine;
using Frezerka.Machines.Interfaces;
using Frezerka.Utility;

namespace Frezerka.Safety
{
    [RequireComponent(typeof(Collider))]
    public class DangerZoneTrigger : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string violationType = "touched_rotating_spindle";
        [SerializeField] private string warningMessageRU = "Опасно! Контакт с вращающимся шпинделем!";
        [SerializeField] private string warningImageName = "warn1-ru";

        [Header("References")]
        [SerializeField] private MonoBehaviour spindleRef; // Should implement ISpindle
        [SerializeField] private SafetyWarningUI warningUI;

        private ISpindle _spindle;
        private float _lastViolationTime;
        private const float ViolationCooldown = 3f;

        private void Start()
        {
            if (spindleRef != null)
                _spindle = spindleRef as ISpindle;

            var col = GetComponent<Collider>();
            if (col != null)
                col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player)) return;
            if (_spindle == null || !_spindle.IsRotating) return;

            // Cooldown to avoid spam
            if (Time.time - _lastViolationTime < ViolationCooldown) return;
            _lastViolationTime = Time.time;

            // Publish safety violation
            EventBus.Publish(new SafetyViolationEvent
            {
                ViolationType = violationType,
                ActiveStepId = "",
                PlayerPosition = other.transform.position,
                Description = warningMessageRU
            });

            // Show warning
            warningUI?.ShowWarning(warningMessageRU, warningImageName);

            Debug.LogWarning($"[DangerZone] Safety violation: {violationType}");
        }
    }
}
