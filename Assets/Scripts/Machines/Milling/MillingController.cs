using UnityEngine;
using Frezerka.Machines.Base;
using Frezerka.Machines.Interfaces;

namespace Frezerka.Machines.Milling
{
    public class MillingController : MachineBase
    {
        [Header("Milling Components")]
        [SerializeField] private MillingSpindle spindle;
        [SerializeField] private MillingTable table;
        [SerializeField] private MillingDoor door;
        [SerializeField] private MillingControlPanel controlPanel;
        [SerializeField] private MillingRemoteControl remoteControl;
        [SerializeField] private MillingCoolantSystem coolantSystem;

        public override MachineType MachineType => MachineType.Milling;

        public MillingSpindle Spindle => spindle;
        public MillingTable Table => table;
        public MillingDoor Door => door;
        public MillingControlPanel ControlPanel => controlPanel;
        public MillingRemoteControl RemoteControl => remoteControl;
        public MillingCoolantSystem CoolantSystem => coolantSystem;

        public override void PowerOn()
        {
            if (door != null && !door.IsLocked)
            {
                Debug.LogWarning("[MillingController] Cannot start: door not locked!");
                return;
            }

            base.PowerOn();
            if (machineAudio != null)
                machineAudio.Play();
        }

        public override void PowerOff()
        {
            spindle?.Disengage();
            coolantSystem?.StopCoolant();
            base.PowerOff();
            if (machineAudio != null)
                machineAudio.Stop();
        }

        public override void EmergencyStop()
        {
            spindle?.Disengage();
            coolantSystem?.StopCoolant();
            base.EmergencyStop();
            if (machineAudio != null)
                machineAudio.Stop();
        }
    }
}
