using UnityEngine;
using Frezerka.Machines.Interfaces;

namespace Frezerka.Machines.Shared
{
    [CreateAssetMenu(fileName = "CuttingTool", menuName = "Frezerka/CuttingToolData")]
    public class CuttingToolData : ScriptableObject, ICuttingTool
    {
        [Header("Tool Info")]
        [SerializeField] private string toolId;
        [SerializeField] private string toolName;
        [SerializeField] private CuttingToolType toolType;
        [SerializeField] private int toolHolderPosition;

        [Header("Visuals")]
        [SerializeField] private GameObject prefab;
        [SerializeField] private Sprite icon;

        public string ToolId => toolId;
        public string ToolName => toolName;
        public CuttingToolType ToolType => toolType;
        public int ToolHolderPosition => toolHolderPosition;
        public GameObject Prefab => prefab;
        public Sprite Icon => icon;
    }
}
