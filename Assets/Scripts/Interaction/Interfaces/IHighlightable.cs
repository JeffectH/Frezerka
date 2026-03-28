using UnityEngine;

namespace Frezerka.Interaction.Interfaces
{
    public interface IHighlightable
    {
        void SetHighlight(bool active, Color? color = null);
        void Pulse(float duration, Color color);
    }
}
