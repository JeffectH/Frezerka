using UnityEngine;
using TMPro;

namespace Frezerka.Interaction
{
    public class CrosshairUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private RectTransform crosshairDot;
        [SerializeField] private RectTransform crosshairExpanded;
        [SerializeField] private TextMeshProUGUI interactionText;
        [SerializeField] private RectTransform grabIndicator;

        [Header("Settings")]
        [SerializeField] private float dotSize = 8f;
        [SerializeField] private float expandedSize = 24f;

        private bool _isHovering;

        private void Start()
        {
            ShowDefault();
        }

        public void ShowDefault()
        {
            _isHovering = false;

            if (crosshairDot != null)
            {
                crosshairDot.gameObject.SetActive(true);
                crosshairDot.sizeDelta = new Vector2(dotSize, dotSize);
            }

            if (crosshairExpanded != null)
                crosshairExpanded.gameObject.SetActive(false);

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
                interactionText.text = "";
            }

            if (grabIndicator != null)
                grabIndicator.gameObject.SetActive(false);
        }

        public void ShowHover(string displayName)
        {
            _isHovering = true;

            if (crosshairDot != null)
                crosshairDot.gameObject.SetActive(false);

            if (crosshairExpanded != null)
            {
                crosshairExpanded.gameObject.SetActive(true);
                crosshairExpanded.sizeDelta = new Vector2(expandedSize, expandedSize);
            }

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true);
                interactionText.text = displayName;
            }
        }

        public void ShowGrabbed(string objectName)
        {
            if (grabIndicator != null)
                grabIndicator.gameObject.SetActive(true);

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true);
                interactionText.text = objectName + " [ПКМ - бросить]";
            }
        }

        public void HideGrabbed()
        {
            if (grabIndicator != null)
                grabIndicator.gameObject.SetActive(false);
        }

        public void ShowInvalid()
        {
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true);
                interactionText.text = "X";
                interactionText.color = Color.red;
            }
        }
    }
}
