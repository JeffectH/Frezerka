using UnityEngine;
using Frezerka.Interaction;
using Frezerka.Interaction.Interfaces;

namespace Frezerka.Machines.Milling
{
    public class MillingRemoteControl : InteractableBase, IClickable
    {
        [Header("Remote Control")]
        [SerializeField] private MillingTable table;

        [Header("Settings")]
        [SerializeField] private float[] speedLevels = { 0.005f, 0.01f, 0.02f };

        private MillingAxis _currentAxis = MillingAxis.X;
        private int _currentSpeedLevel = 1; // F1, F2, F3

        public MillingAxis CurrentAxis => _currentAxis;
        public int CurrentSpeedLevel => _currentSpeedLevel;

        public override InteractionType GetInteractionType() => InteractionType.Click;

        public override void OnInteract()
        {
            OnClick();
        }

        public void OnClick()
        {
            // Default click cycles through axes
            CycleAxis();
        }

        public void CycleAxis()
        {
            _currentAxis = (MillingAxis)(((int)_currentAxis + 1) % 5);
            Debug.Log($"[MillingRemote] Axis: {_currentAxis}");
        }

        public void SetAxis(MillingAxis axis)
        {
            _currentAxis = axis;
        }

        public void SetSpeedLevel(int level)
        {
            _currentSpeedLevel = Mathf.Clamp(level, 0, speedLevels.Length - 1);
            if (table != null)
                table.SetMoveSpeed(speedLevels[_currentSpeedLevel]);
            Debug.Log($"[MillingRemote] Speed: F{_currentSpeedLevel + 1}");
        }

        public void MovePlus()
        {
            table?.Move(_currentAxis, speedLevels[_currentSpeedLevel]);
        }

        public void MoveMinus()
        {
            table?.Move(_currentAxis, -speedLevels[_currentSpeedLevel]);
        }
    }
}
