using System;
using UnityEngine;

namespace Frezerka.Machines.Milling
{
    public enum MillingAxis { X, Y, Z, RotationA, RotationB }

    public class MillingTable : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform tableTransform;

        [Header("Limits")]
        [SerializeField] private Vector3 minPosition = new Vector3(-0.3f, -0.2f, -0.3f);
        [SerializeField] private Vector3 maxPosition = new Vector3(0.3f, 0.2f, 0.3f);
        [SerializeField] private float maxRotation = 90f;

        private Vector3 _startPosition;
        private Quaternion _startRotation;
        private float _moveSpeed = 0.01f;

        public Vector3 CurrentPosition => tableTransform != null
            ? tableTransform.localPosition - _startPosition
            : Vector3.zero;

        public event Action<Vector3> OnPositionChanged;

        private void Start()
        {
            if (tableTransform == null)
                tableTransform = transform;
            _startPosition = tableTransform.localPosition;
            _startRotation = tableTransform.localRotation;
        }

        public void Move(MillingAxis axis, float delta)
        {
            if (tableTransform == null) return;

            var pos = tableTransform.localPosition;

            switch (axis)
            {
                case MillingAxis.X:
                    pos.x += delta * _moveSpeed;
                    pos.x = Mathf.Clamp(pos.x, _startPosition.x + minPosition.x, _startPosition.x + maxPosition.x);
                    break;
                case MillingAxis.Y:
                    pos.y += delta * _moveSpeed;
                    pos.y = Mathf.Clamp(pos.y, _startPosition.y + minPosition.y, _startPosition.y + maxPosition.y);
                    break;
                case MillingAxis.Z:
                    pos.z += delta * _moveSpeed;
                    pos.z = Mathf.Clamp(pos.z, _startPosition.z + minPosition.z, _startPosition.z + maxPosition.z);
                    break;
                case MillingAxis.RotationA:
                    tableTransform.Rotate(Vector3.up, delta, Space.Self);
                    break;
                case MillingAxis.RotationB:
                    tableTransform.Rotate(Vector3.right, delta, Space.Self);
                    break;
            }

            if (axis != MillingAxis.RotationA && axis != MillingAxis.RotationB)
            {
                tableTransform.localPosition = pos;
            }

            OnPositionChanged?.Invoke(CurrentPosition);
        }

        public void SetMoveSpeed(float speed)
        {
            _moveSpeed = Mathf.Max(0.001f, speed);
        }

        public void ResetPosition()
        {
            if (tableTransform == null) return;
            tableTransform.localPosition = _startPosition;
            tableTransform.localRotation = _startRotation;
            OnPositionChanged?.Invoke(Vector3.zero);
        }
    }
}
