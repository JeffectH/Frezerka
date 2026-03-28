using UnityEngine;

namespace Frezerka.Interaction
{
    public static class ObjectHighlighter
    {
        public static void SetEmission(Renderer renderer, bool active, Color color)
        {
            if (renderer == null || renderer.material == null) return;

            if (active)
            {
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", color * 0.5f);
            }
            else
            {
                renderer.material.DisableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", Color.black);
            }
        }

        public static void SetEmission(Renderer[] renderers, bool active, Color color)
        {
            if (renderers == null) return;
            foreach (var r in renderers)
                SetEmission(r, active, color);
        }
    }
}
