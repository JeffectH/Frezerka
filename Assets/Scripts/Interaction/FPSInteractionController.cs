using UnityEngine;
using Frezerka.Interaction.Interfaces;
using Frezerka.Utility;
using StarterAssets;

namespace Frezerka.Interaction
{
    [RequireComponent(typeof(InteractionRaycaster))]
    [RequireComponent(typeof(GrabSystem))]
    [RequireComponent(typeof(FPSInteractionInputs))]
    public class FPSInteractionController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CrosshairUI crosshairUI;

        [Header("UI Interaction")]
        [SerializeField] private float uiInteractionDistance = 3f;

        private InteractionRaycaster _raycaster;
        private GrabSystem _grabSystem;
        private FPSInteractionInputs _inputs;
        private StarterAssetsInputs _movementInputs;
        private bool _isCursorMode;

        private void Start()
        {
            _raycaster = GetComponent<InteractionRaycaster>();
            _grabSystem = GetComponent<GrabSystem>();
            _inputs = GetComponent<FPSInteractionInputs>();
            _movementInputs = GetComponent<StarterAssetsInputs>();
        }

        private void Update()
        {
            if (_isCursorMode)
            {
                HandleCursorMode();
                return;
            }

            _raycaster.CastRay();
            UpdateCrosshair();
            HandleInput();
        }

        private void UpdateCrosshair()
        {
            if (crosshairUI == null) return;

            if (_grabSystem.HasGrabbedObject)
            {
                crosshairUI.ShowGrabbed(_grabSystem.GrabbedObject.DisplayName);
                return;
            }

            if (_raycaster.HasTarget)
            {
                var target = _raycaster.CurrentTarget;
                if (target.IsInteractable)
                    crosshairUI.ShowHover(target.DisplayName);
                else
                    crosshairUI.ShowInvalid();
            }
            else
            {
                crosshairUI.ShowDefault();
            }
        }

        private void HandleInput()
        {
            // Cancel / drop grabbed object
            if (_inputs.CancelPressed)
            {
                if (_grabSystem.HasGrabbedObject)
                {
                    _grabSystem.Drop();
                    crosshairUI?.ShowDefault();
                    return;
                }
            }

            // Interact
            if (_inputs.InteractPressed)
            {
                // If holding an object, try to place it
                if (_grabSystem.HasGrabbedObject)
                {
                    if (_raycaster.HasTarget)
                    {
                        var targetTransform = ((MonoBehaviour)_raycaster.CurrentTarget).transform;
                        if (_grabSystem.TryPlace(targetTransform))
                        {
                            crosshairUI?.HideGrabbed();
                            PublishInteraction(_raycaster.CurrentTarget, "Place");
                        }
                    }
                    return;
                }

                // Nothing grabbed — interact with target
                if (_raycaster.HasTarget)
                {
                    var target = _raycaster.CurrentTarget;
                    if (!target.IsInteractable) return;

                    // Check if target is on a UI panel — switch to cursor mode
                    var targetMono = (MonoBehaviour)target;
                    if (targetMono.CompareTag(Tags.UIPanel))
                    {
                        EnterCursorMode();
                        return;
                    }

                    // Try grab
                    if (target is IGrabbable grabbable)
                    {
                        if (_grabSystem.TryGrab(grabbable))
                        {
                            crosshairUI?.ShowGrabbed(target.DisplayName);
                            PublishInteraction(target, "Grab");
                            return;
                        }
                    }

                    // Click / interact
                    target.OnInteract();

                    if (target is IClickable clickable)
                        clickable.OnClick();

                    PublishInteraction(target, target.GetInteractionType().ToString());
                }
            }
        }

        private void EnterCursorMode()
        {
            _isCursorMode = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (_movementInputs != null)
                _movementInputs.cursorInputForLook = false;

            crosshairUI?.ShowDefault();
        }

        public void ExitCursorMode()
        {
            _isCursorMode = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (_movementInputs != null)
                _movementInputs.cursorInputForLook = true;
        }

        private void HandleCursorMode()
        {
            // Escape exits cursor mode
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitCursorMode();
            }
        }

        private void PublishInteraction(IInteractable target, string type)
        {
            var playerTransform = transform;
            EventBus.Publish(new InteractionEvent
            {
                InteractionId = target.InteractionId,
                InteractionType = type,
                PlayerPosition = playerTransform.position,
                PlayerRotation = playerTransform.rotation,
                Timestamp = Time.time
            });
        }
    }
}
