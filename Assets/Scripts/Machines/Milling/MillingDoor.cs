using UnityEngine;
using Frezerka.Interaction;
using Frezerka.Interaction.Interfaces;

namespace Frezerka.Machines.Milling
{
    public class MillingDoor : InteractableBase, IClickable
    {
        [Header("Door")]
        [SerializeField] private Animator doorAnimator;
        [SerializeField] private string openAnimParam = "IsOpen";

        private bool _isOpen = true;
        private bool _isLocked;

        public bool IsOpen => _isOpen;
        public bool IsLocked => _isLocked;

        public override InteractionType GetInteractionType() => InteractionType.Toggle;

        public override void OnInteract()
        {
            OnClick();
        }

        public void OnClick()
        {
            if (_isLocked)
            {
                Debug.Log("[MillingDoor] Door is locked, cannot toggle");
                return;
            }

            _isOpen = !_isOpen;
            if (doorAnimator != null)
                doorAnimator.SetBool(openAnimParam, _isOpen);

            Debug.Log($"[MillingDoor] Door {(_isOpen ? "opened" : "closed")}");
        }

        public void Close()
        {
            _isOpen = false;
            if (doorAnimator != null)
                doorAnimator.SetBool(openAnimParam, false);
        }

        public void Open()
        {
            if (_isLocked) return;
            _isOpen = true;
            if (doorAnimator != null)
                doorAnimator.SetBool(openAnimParam, true);
        }

        public void Lock()
        {
            if (_isOpen) return; // Can only lock when closed
            _isLocked = true;
            Debug.Log("[MillingDoor] Door locked");
        }

        public void Unlock()
        {
            _isLocked = false;
            Debug.Log("[MillingDoor] Door unlocked");
        }
    }
}
