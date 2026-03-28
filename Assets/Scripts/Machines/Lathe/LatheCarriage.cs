using System;
using UnityEngine;
using Frezerka.Machines.Interfaces;

namespace Frezerka.Machines.Lathe
{
    public class LatheCarriage : MonoBehaviour, ICarriageController
    {
        [Header("Movement")]
        [SerializeField] private Transform carriageTransform;
        [SerializeField] private float moveSpeed = 0.01f;
        [SerializeField] private Vector3 minPosition = new Vector3(-0.5f, 0f, -0.5f);
        [SerializeField] private Vector3 maxPosition = new Vector3(0.5f, 0f, 0.5f);

        private Vector3 _startPosition;
        private float _feedRate = 1f;

        public Vector3 CurrentPosition => carriageTransform != null
            ? carriageTransform.localPosition - _startPosition
            : Vector3.zero;

        public event Action<Vector3> OnPositionChanged;

        private void Start()
        {
            if (carriageTransform == null)
                carriageTransform = transform;
            _startPosition = carriageTransform.localPosition;
        }

        public void MoveManual(Vector2 delta)
        {
            Move(delta * moveSpeed);
        }

        public void MoveJoystick(Vector2 input)
        {
            Move(input * moveSpeed * _feedRate * Time.deltaTime);
        }

        public void SetFeedRate(float rate)
        {
            _feedRate = Mathf.Max(0.1f, rate);
            Debug.Log($"[LatheCarriage] Feed rate: {_feedRate}");
        }

        public void ResetPosition()
        {
            if (carriageTransform != null)
                carriageTransform.localPosition = _startPosition;
            OnPositionChanged?.Invoke(CurrentPosition);
        }

        private void Move(Vector2 delta)
        {
            if (carriageTransform == null) return;

            var pos = carriageTransform.localPosition;
            pos.x += delta.x;
            pos.z += delta.y;

            pos.x = Mathf.Clamp(pos.x, _startPosition.x + minPosition.x, _startPosition.x + maxPosition.x);
            pos.z = Mathf.Clamp(pos.z, _startPosition.z + minPosition.z, _startPosition.z + maxPosition.z);

            carriageTransform.localPosition = pos;
            OnPositionChanged?.Invoke(CurrentPosition);
        }
    }
}
