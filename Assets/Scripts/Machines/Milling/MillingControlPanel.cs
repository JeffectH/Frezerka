using UnityEngine;

namespace Frezerka.Machines.Milling
{
    public class MillingControlPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MillingController millingController;
        [SerializeField] private MillingSpindle spindle;
        [SerializeField] private MillingDoor door;

        [Header("Current Settings")]
        [SerializeField] private float currentRPM = 1500f;
        [SerializeField] private float currentFeedSpeed = 1f;

        public float CurrentRPM => currentRPM;
        public float CurrentFeedSpeed => currentFeedSpeed;

        public void SetRPM(float rpm)
        {
            currentRPM = rpm;
            spindle?.SetSpeed(rpm);
        }

        public void SetFeedSpeed(float speed)
        {
            currentFeedSpeed = speed;
        }

        public void OnPowerOnClicked()
        {
            millingController?.PowerOn();
        }

        public void OnPowerOffClicked()
        {
            millingController?.PowerOff();
        }

        public void OnLockDoorClicked()
        {
            door?.Lock();
        }

        public void OnUnlockDoorClicked()
        {
            door?.Unlock();
        }
    }
}
