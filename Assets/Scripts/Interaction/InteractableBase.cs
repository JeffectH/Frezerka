using UnityEngine;
using System.Collections;
using Frezerka.Interaction.Interfaces;

namespace Frezerka.Interaction
{
    [RequireComponent(typeof(Collider))]
    public abstract class InteractableBase : MonoBehaviour, IInteractable, IHighlightable
    {
        [Header("Interaction Settings")]
        [SerializeField] protected string interactionId;
        [SerializeField] protected string displayName;
        [SerializeField] protected bool isInteractable = true;

        [Header("Highlight")]
        [SerializeField] protected Color hoverColor = new Color(1f, 1f, 0f, 0.3f);

        protected Renderer[] renderers;
        protected Color[] originalColors;
        private Coroutine _pulseCoroutine;

        public string InteractionId => interactionId;
        public string DisplayName => displayName;

        public bool IsInteractable
        {
            get => isInteractable;
            set => isInteractable = value;
        }

        protected virtual void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>();
            CacheOriginalColors();
        }

        private void CacheOriginalColors()
        {
            if (renderers == null || renderers.Length == 0) return;

            originalColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].material != null)
                    originalColors[i] = renderers[i].material.color;
            }
        }

        public virtual void OnHoverEnter()
        {
            if (!isInteractable) return;
            SetHighlight(true, hoverColor);
        }

        public virtual void OnHoverExit()
        {
            SetHighlight(false);
        }

        public abstract void OnInteract();
        public abstract InteractionType GetInteractionType();

        public virtual void SetHighlight(bool active, Color? color = null)
        {
            if (renderers == null) return;

            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] == null || renderers[i].material == null) continue;

                if (active && color.HasValue)
                {
                    renderers[i].material.color = Color.Lerp(
                        originalColors != null && i < originalColors.Length ? originalColors[i] : Color.white,
                        color.Value,
                        0.4f
                    );
                }
                else if (originalColors != null && i < originalColors.Length)
                {
                    renderers[i].material.color = originalColors[i];
                }
            }
        }

        public virtual void Pulse(float duration, Color color)
        {
            if (_pulseCoroutine != null)
                StopCoroutine(_pulseCoroutine);
            _pulseCoroutine = StartCoroutine(PulseCoroutine(duration, color));
        }

        private IEnumerator PulseCoroutine(float duration, Color color)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = Mathf.PingPong(elapsed * 2f, 1f);
                SetHighlight(true, Color.Lerp(Color.clear, color, t));
                elapsed += Time.deltaTime;
                yield return null;
            }
            SetHighlight(false);
            _pulseCoroutine = null;
        }
    }
}
