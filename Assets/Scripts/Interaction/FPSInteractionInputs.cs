using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace Frezerka.Interaction
{
    public class FPSInteractionInputs : MonoBehaviour
    {
        [Header("Input Values")]
        public bool interact;
        public bool cancel;

        private bool _interactPrevious;
        private bool _cancelPrevious;

        /// <summary>Returns true only on the frame when interact was first pressed.</summary>
        public bool InteractPressed => interact && !_interactPrevious;

        /// <summary>Returns true only on the frame when cancel was first pressed.</summary>
        public bool CancelPressed => cancel && !_cancelPrevious;

        private void LateUpdate()
        {
            _interactPrevious = interact;
            _cancelPrevious = cancel;
        }

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnInteract(InputValue value)
        {
            interact = value.isPressed;
        }

        public void OnCancel(InputValue value)
        {
            cancel = value.isPressed;
        }
#else
        private void Update()
        {
            interact = Input.GetMouseButton(0);
            cancel = Input.GetMouseButton(1);
        }
#endif
    }
}
