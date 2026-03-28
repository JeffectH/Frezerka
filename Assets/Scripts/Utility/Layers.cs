using UnityEngine;

namespace Frezerka.Utility
{
    public static class Layers
    {
        public const string Interactable = "Interactable";
        public const string UI = "UI";
        public const string DangerZone = "DangerZone";
        public const string HeatmapZone = "HeatmapZone";

        public static int InteractableMask => LayerMask.GetMask(Interactable);
        public static int UIMask => LayerMask.GetMask(UI);
    }
}
