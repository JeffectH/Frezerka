using System;

namespace Frezerka.Machines.Interfaces
{
    public enum MachineType { Lathe, Milling }

    public enum MachineState
    {
        Off,
        Idle,
        SpindleRunning,
        Cutting,
        Emergency
    }

    public interface IMachine
    {
        string MachineId { get; }
        MachineType MachineType { get; }
        MachineState CurrentState { get; }
        bool IsPoweredOn { get; }
        event Action<MachineState, MachineState> OnStateChanged;

        void PowerOn();
        void PowerOff();
        void EmergencyStop();
    }
}
