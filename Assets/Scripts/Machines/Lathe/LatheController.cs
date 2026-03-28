using UnityEngine;
using Frezerka.Machines.Base;
using Frezerka.Machines.Interfaces;

namespace Frezerka.Machines.Lathe
{
    public class LatheController : MachineBase
    {
        [Header("Lathe Components")]
        [SerializeField] private LatheSpindle spindle;
        [SerializeField] private LatheChuck chuck;
        [SerializeField] private LatheCarriage carriage;
        [SerializeField] private LatheToolHolder toolHolder;
        [SerializeField] private LatheCoordinateDisplay coordinateDisplay;

        public override MachineType MachineType => MachineType.Lathe;

        public LatheSpindle Spindle => spindle;
        public LatheChuck Chuck => chuck;
        public LatheCarriage Carriage => carriage;
        public LatheToolHolder ToolHolder => toolHolder;

        public override void PowerOn()
        {
            base.PowerOn();
            if (machineAudio != null)
                machineAudio.Play();
        }

        public override void PowerOff()
        {
            spindle?.Disengage();
            base.PowerOff();
            if (machineAudio != null)
                machineAudio.Stop();
        }

        public override void EmergencyStop()
        {
            spindle?.Disengage();
            base.EmergencyStop();
            if (machineAudio != null)
                machineAudio.Stop();
        }
    }
}
