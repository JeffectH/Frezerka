using UnityEngine;
using Frezerka.Utility;

namespace Frezerka.Safety
{
    public class SafetyManager : SingletonMonoBehaviour<SafetyManager>
    {
        [Header("State")]
        [SerializeField] private bool glassesEquipped;
        [SerializeField] private bool headphonesEquipped;

        public bool GlassesEquipped => glassesEquipped;
        public bool HeadphonesEquipped => headphonesEquipped;
        public bool AllSafetyGearEquipped => glassesEquipped && headphonesEquipped;

        public void EquipGlasses()
        {
            glassesEquipped = true;
            Debug.Log("[SafetyManager] Glasses equipped");
        }

        public void EquipHeadphones()
        {
            headphonesEquipped = true;
            Debug.Log("[SafetyManager] Headphones equipped");
        }

        public void ResetGear()
        {
            glassesEquipped = false;
            headphonesEquipped = false;
        }

        public bool CanStartMachine()
        {
            if (!AllSafetyGearEquipped)
            {
                Debug.LogWarning("[SafetyManager] Cannot start machine - safety gear not equipped!");
                EventBus.Publish(new SafetyViolationEvent
                {
                    ViolationType = "missing_safety_gear",
                    Description = GetMissingGearDescription()
                });
                return false;
            }
            return true;
        }

        private string GetMissingGearDescription()
        {
            if (!glassesEquipped && !headphonesEquipped)
                return "Не надеты очки и наушники";
            if (!glassesEquipped)
                return "Не надеты защитные очки";
            return "Не надеты защитные наушники";
        }
    }
}
