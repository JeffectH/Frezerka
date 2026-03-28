using System.Collections.Generic;
using UnityEngine;
using Frezerka.Interaction.Interfaces;

namespace Frezerka.Training
{
    public class TrainingHighlighter : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Color highlightColor = Color.green;
        [SerializeField] private float pulseDuration = 999f;

        private List<IHighlightable> _activeHighlights = new List<IHighlightable>();

        public void HighlightObjects(string[] interactionIds)
        {
            ClearHighlights();

            var allInteractables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var mb in allInteractables)
            {
                if (mb is IInteractable interactable && mb is IHighlightable highlightable)
                {
                    foreach (var id in interactionIds)
                    {
                        if (interactable.InteractionId == id)
                        {
                            highlightable.Pulse(pulseDuration, highlightColor);
                            _activeHighlights.Add(highlightable);
                            break;
                        }
                    }
                }
            }
        }

        public void ClearHighlights()
        {
            foreach (var h in _activeHighlights)
            {
                h?.SetHighlight(false);
            }
            _activeHighlights.Clear();
        }
    }
}
