using System;
using UnityEngine;

namespace Frezerka.Machines.Interfaces
{
    public interface ICarriageController
    {
        Vector3 CurrentPosition { get; }
        void MoveManual(Vector2 delta);
        void MoveJoystick(Vector2 input);
        void SetFeedRate(float rate);
        void ResetPosition();
        event Action<Vector3> OnPositionChanged;
    }
}
