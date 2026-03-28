using UnityEngine;
using Frezerka.Interaction;
using Frezerka.Interaction.Interfaces;
using Frezerka.Machines.Interfaces;

namespace Frezerka.Machines.Lathe
{
    public class LatheToolHolder : InteractableBase, IClickable
    {
        [Header("Tool Holder")]
        [SerializeField] private Transform[] toolSlots = new Transform[4];
        [SerializeField] private int activeSlotIndex;

        private ICuttingTool[] _tools = new ICuttingTool[4];

        public int ActiveSlotIndex => activeSlotIndex;
        public ICuttingTool ActiveTool => activeSlotIndex < _tools.Length ? _tools[activeSlotIndex] : null;

        public override InteractionType GetInteractionType() => InteractionType.Click;

        public override void OnInteract()
        {
            OnClick();
        }

        public void OnClick()
        {
            // Rotate to next slot position
            activeSlotIndex = (activeSlotIndex + 1) % toolSlots.Length;
            Debug.Log($"[LatheToolHolder] Rotated to slot {activeSlotIndex + 1}");
        }

        public bool InsertTool(ICuttingTool tool, int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _tools.Length) return false;
            if (_tools[slotIndex] != null) return false;

            _tools[slotIndex] = tool;

            if (tool is MonoBehaviour toolMono && slotIndex < toolSlots.Length && toolSlots[slotIndex] != null)
            {
                toolMono.transform.SetParent(toolSlots[slotIndex]);
                toolMono.transform.localPosition = Vector3.zero;
                toolMono.transform.localRotation = Quaternion.identity;
            }

            Debug.Log($"[LatheToolHolder] Tool '{tool.ToolName}' inserted in slot {slotIndex + 1}");
            return true;
        }

        public ICuttingTool RemoveTool(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _tools.Length) return null;
            var tool = _tools[slotIndex];
            _tools[slotIndex] = null;
            Debug.Log($"[LatheToolHolder] Tool removed from slot {slotIndex + 1}");
            return tool;
        }

        public void SetActiveSlot(int index)
        {
            activeSlotIndex = Mathf.Clamp(index, 0, toolSlots.Length - 1);
        }
    }
}
