using UnityEngine;
using Frezerka.Interaction.Interfaces;

namespace Frezerka.Interaction
{
    public class InteractionRaycaster : MonoBehaviour
    {
        [SerializeField] private float maxDistance = 5f;
        [SerializeField] private LayerMask interactableLayers = ~0;

        private Camera _camera;
        private IInteractable _currentTarget;
        private RaycastHit _lastHit;
        private bool _hasHit;

        public IInteractable CurrentTarget => _currentTarget;
        public RaycastHit LastHit => _lastHit;
        public bool HasTarget => _hasHit && _currentTarget != null;
        public float MaxDistance => maxDistance;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void CastRay()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
                if (_camera == null) return;
            }

            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            _hasHit = Physics.Raycast(ray, out _lastHit, maxDistance, interactableLayers);

            IInteractable newTarget = null;

            if (_hasHit)
            {
                newTarget = _lastHit.collider.GetComponent<IInteractable>();
                if (newTarget == null)
                    newTarget = _lastHit.collider.GetComponentInParent<IInteractable>();
            }

            if (newTarget != _currentTarget)
            {
                _currentTarget?.OnHoverExit();

                if (newTarget != null && newTarget.IsInteractable)
                {
                    newTarget.OnHoverEnter();
                    _currentTarget = newTarget;
                }
                else
                {
                    _currentTarget = null;
                }
            }
        }

        public void ClearTarget()
        {
            _currentTarget?.OnHoverExit();
            _currentTarget = null;
        }
    }
}
