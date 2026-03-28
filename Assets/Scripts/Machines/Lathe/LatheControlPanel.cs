using UnityEngine;
using Frezerka.Interaction;
using Frezerka.Interaction.Interfaces;

namespace Frezerka.Machines.Lathe
{
    public class LatheControlPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LatheController latheController;
        [SerializeField] private LatheSpindle spindle;

        [Header("Current Settings")]
        [SerializeField] private float currentRPM = 800f;
        [SerializeField] private int currentGear = 1;

        public float CurrentRPM => currentRPM;
        public int CurrentGear => currentGear;

        public void SetRPM(float rpm)
        {
            currentRPM = rpm;
            spindle?.SetSpeed(rpm);
        }

        public void SetGear(int gear)
        {
            currentGear = gear;
            spindle?.SetTransmission(gear);
        }

        public void OnPowerOnClicked()
        {
            latheController?.PowerOn();
        }

        public void OnPowerOffClicked()
        {
            latheController?.PowerOff();
        }

        public void OnEmergencyStopClicked()
        {
            latheController?.EmergencyStop();
        }
    }
}
