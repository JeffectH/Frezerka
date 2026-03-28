using UnityEngine;
using Frezerka.Interaction;
using Frezerka.Interaction.Interfaces;

namespace Frezerka.Machines.Lathe
{
    public class LatheChuck : InteractableBase, IClickable
    {
        [Header("Chuck Settings")]
        [SerializeField] private Transform workpieceSlot;

        private IWorkpiece _currentWorkpiece;

        public Transform WorkpieceSlot => workpieceSlot;
        public bool HasWorkpiece => _currentWorkpiece != null;
        public IWorkpiece CurrentWorkpiece => _currentWorkpiece;

        public override InteractionType GetInteractionType() => InteractionType.Click;

        public override void OnInteract()
        {
            OnClick();
        }

        public void OnClick()
        {
            Debug.Log("[LatheChuck] Clicked");
        }

        public void InsertWorkpiece(IWorkpiece workpiece)
        {
            _currentWorkpiece = workpiece;
            if (workpiece != null && workpieceSlot != null)
            {
                workpiece.Transform.SetParent(workpieceSlot);
                workpiece.Transform.localPosition = Vector3.zero;
                workpiece.Transform.localRotation = Quaternion.identity;
            }
            Debug.Log($"[LatheChuck] Workpiece inserted: {workpiece?.WorkpieceId}");
        }

        public IWorkpiece RemoveWorkpiece()
        {
            var wp = _currentWorkpiece;
            _currentWorkpiece = null;
            if (wp != null)
                wp.Transform.SetParent(null);
            Debug.Log("[LatheChuck] Workpiece removed");
            return wp;
        }
    }
}
