using System;
using UnityEngine;
using Frezerka.Machines.Interfaces;

namespace Frezerka.Machines.Lathe
{
    public class LatheSpindle : MonoBehaviour, ISpindle
    {
        [Header("Visual")]
        [SerializeField] private Transform spindleVisual;

        [Header("Settings")]
        [SerializeField] private float maxRPM = 2000f;
        [SerializeField] private float defaultRPM = 800f;

        private float _currentRPM;
        private bool _engaged;
        private int _currentGear = 1;

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
            Debug.Log($"[LatheSpindle] Speed set to {_currentRPM} RPM");
        }

        public void SetTransmission(int gear)
        {
            _currentGear = Mathf.Clamp(gear, 1, 6);
            Debug.Log($"[LatheSpindle] Transmission gear: {_currentGear}");
        }

        public void Engage(RotationDirection direction)
        {
            if (direction == RotationDirection.Stopped)
            {
                Disengage();
                return;
            }

            _engaged = true;
            Direction = direction;
            Debug.Log($"[LatheSpindle] Engaged: {direction}, {_currentRPM} RPM");
        }

        public void Disengage()
        {
            _engaged = false;
            Direction = RotationDirection.Stopped;
            Debug.Log("[LatheSpindle] Disengaged");
        }

        private void Update()
        {
            if (!IsRotating || spindleVisual == null) return;

            float degreesPerSecond = (_currentRPM / 60f) * 360f;
            float sign = Direction == RotationDirection.CounterClockwise ? -1f : 1f;
            spindleVisual.Rotate(Vector3.forward, sign * degreesPerSecond * Time.deltaTime);
        }
    }
}
