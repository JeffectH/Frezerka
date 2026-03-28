using System;

namespace Frezerka.Machines.Interfaces
{
    public enum RotationDirection { Stopped, Clockwise, CounterClockwise }

    public interface ISpindle
    {
        float CurrentRPM { get; }
        bool IsRotating { get; }
        RotationDirection Direction { get; }
        void SetSpeed(float rpm);
        void SetTransmission(int gear);
        void Engage(RotationDirection direction);
        void Disengage();
        event Action<float> OnSpeedChanged;
    }
}
