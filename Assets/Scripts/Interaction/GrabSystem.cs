using UnityEngine;
using Frezerka.Interaction.Interfaces;

namespace Frezerka.Interaction
{
    public class GrabSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform grabHolder;
        [SerializeField] private float grabDistance = 2f;
        [SerializeField] private Vector3 holdOffset = new Vector3(0f, -0.3f, 0.8f);

        private IGrabbable _grabbedObject;
        private Transform _grabbedTransform;
        private Transform _originalParent;
        private Vector3 _originalLocalPosition;
        private Quaternion _originalLocalRotation;

        public bool HasGrabbedObject => _grabbedObject != null;
        public IGrabbable GrabbedObject => _grabbedObject;

        private void Start()
        {
            if (grabHolder == null)
            {
                var cam = Camera.main;
                if (cam != null)
                {
                    var holder = new GameObject("GrabHolder");
                    holder.transform.SetParent(cam.transform);
                    holder.transform.localPosition = holdOffset;
                    holder.transform.localRotation = Quaternion.identity;
                    grabHolder = holder.transform;
                }
            }
        }

        public bool TryGrab(IGrabbable grabbable)
        {
            if (_grabbedObject != null) return false;
            if (grabbable == null || grabbable.IsGrabbed) return false;

            _grabbedObject = grabbable;
            _grabbedTransform = ((MonoBehaviour)grabbable).transform;

            _originalParent = _grabbedTransform.parent;
            _originalLocalPosition = _grabbedTransform.localPosition;
            _originalLocalRotation = _grabbedTransform.localRotation;

            grabbable.OnGrab();

            _grabbedTransform.SetParent(grabHolder);
            _grabbedTransform.localPosition = Vector3.zero;
            _grabbedTransform.localRotation = Quaternion.identity;

            var rb = _grabbedTransform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            var col = _grabbedTransform.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }

            return true;
        }

        public bool TryPlace(Transform target)
        {
            if (_grabbedObject == null) return false;

            if (!_grabbedObject.CanPlaceAt(target)) return false;

            var col = _grabbedTransform.GetComponent<Collider>();
            if (col != null)
                col.enabled = true;

            var rb = _grabbedTransform.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = false;

            _grabbedObject.OnPlaceAt(target);
            _grabbedObject = null;
            _grabbedTransform = null;

            return true;
        }

        public void Drop()
        {
            if (_grabbedObject == null) return;

            _grabbedObject.OnRelease();

            _grabbedTransform.SetParent(_originalParent);
            _grabbedTransform.localPosition = _originalLocalPosition;
            _grabbedTransform.localRotation = _originalLocalRotation;

            var col = _grabbedTransform.GetComponent<Collider>();
            if (col != null)
                col.enabled = true;

            var rb = _grabbedTransform.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = false;

            _grabbedObject = null;
            _grabbedTransform = null;
        }
    }
}
