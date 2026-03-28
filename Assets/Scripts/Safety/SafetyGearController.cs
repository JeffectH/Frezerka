using UnityEngine;
using Frezerka.Interaction;
using Frezerka.Interaction.Interfaces;

namespace Frezerka.Safety
{
    public enum SafetyGearType { Glasses, Headphones }

    public class SafetyGearController : InteractableBase, IGrabbable
    {
        [Header("Safety Gear")]
        [SerializeField] private SafetyGearType gearType;
        [SerializeField] private GameObject visualModel;

        private bool _isGrabbed;
        private bool _isEquipped;

        public bool IsGrabbed => _isGrabbed;
        public bool IsEquipped => _isEquipped;
        public Transform GrabPoint => transform;

        public override InteractionType GetInteractionType() => InteractionType.Grab;

        public override void OnInteract()
        {
            // Shortcut: clicking on safety gear equips it directly
            if (!_isEquipped)
                Equip();
        }

        public void OnGrab()
        {
            _isGrabbed = true;
            Debug.Log($"[SafetyGear] {gearType} grabbed");
        }

        public void OnRelease()
        {
            _isGrabbed = false;
        }

        public bool CanPlaceAt(Transform target)
        {
            // Can be placed back on rack or equipped on player
            return target.CompareTag(Utility.Tags.Player) ||
                   target.CompareTag(Utility.Tags.SafetyGear);
        }

        public void OnPlaceAt(Transform target)
        {
            if (target.CompareTag(Utility.Tags.Player))
            {
                Equip();
            }
            _isGrabbed = false;
        }

        private void Equip()
        {
            _isEquipped = true;

            switch (gearType)
            {
                case SafetyGearType.Glasses:
                    SafetyManager.Instance?.EquipGlasses();
                    break;
                case SafetyGearType.Headphones:
                    SafetyManager.Instance?.EquipHeadphones();
                    break;
            }

            // Hide the visual model (gear is "on" the player)
            if (visualModel != null)
                visualModel.SetActive(false);
            else
                gameObject.SetActive(false);

            Debug.Log($"[SafetyGear] {gearType} equipped");
        }
    }
}
