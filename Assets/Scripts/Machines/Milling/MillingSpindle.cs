using System;
using UnityEngine;
using Frezerka.Machines.Interfaces;

namespace Frezerka.Machines.Milling
{
    public class MillingSpindle : MonoBehaviour, ISpindle
    {
        [Header("Visual")]
        [SerializeField] private Transform cutterVisual;

        [Header("Settings")]
        [SerializeField] private float maxRPM = 3000f;
        [SerializeField] private float defaultRPM = 1500f;

        private float _currentRPM;
        private bool _engaged;

        public float CurrentRPM => _engaged ? _currentRPM : 0f;
        public bool IsRotating => _engaged && _currentRPM > 0f;
        public RotationDirection Direction { get; private set; } = RotationDirection.Stopped;
        public event Action<float> OnSpeedChanged;

        private void Start()
        {
            _currentRPM = defaultRPM;
        }

        public void SetSpeed(float rpm)
        {
            _currentRPM = Mathf.Clamp(rpm, 0f, maxRPM);
            OnSpeedChanged?.Invoke(_currentRPM);
            Debug.Log($"[MillingSpindle] Speed set to {_currentRPM} RPM");
        }

        public void SetTransmission(int gear)
        {
            Debug.Log($"[MillingSpindle] Transmission: {gear}");
        }

        public void Engage(RotationDirection direction = RotationDirection.Clockwise)
        {
            _engaged = true;
            Direction = direction;
            Debug.Log($"[MillingSpindle] Engaged: {_currentRPM} RPM");
        }

        public void Disengage()
        {
            _engaged = false;
            Direction = RotationDirection.Stopped;
            Debug.Log("[MillingSpindle] Disengaged");
        }

        private void Update()
        {
            if (!IsRotating || cutterVisual == null) return;
            float degreesPerSecond = (_currentRPM / 60f) * 360f;
            cutterVisual.Rotate(Vector3.up, degreesPerSecond * Time.deltaTime);
        }
    }
}
