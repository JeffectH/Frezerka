using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Frezerka.Safety
{
    public class SafetyWarningUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject warningPanel;
        [SerializeField] private Image warningImage;
        [SerializeField] private TextMeshProUGUI warningText;

        [Header("Settings")]
        [SerializeField] private float displayDuration = 4f;
        [SerializeField] private string warningSpritesPath = "Warnings/";

        private Coroutine _hideCoroutine;

        private void Start()
        {
            if (warningPanel != null)
                warningPanel.SetActive(false);
        }

        public void ShowWarning(string message, string warningImageName = null)
        {
            if (warningPanel != null)
                warningPanel.SetActive(true);

            if (warningText != null)
                warningText.text = message;

            if (warningImage != null && !string.IsNullOrEmpty(warningImageName))
            {
                var sprite = Resources.Load<Sprite>(warningSpritesPath + warningImageName);
                if (sprite != null)
                {
                    warningImage.sprite = sprite;
                    warningImage.gameObject.SetActive(true);
                }
            }

            if (_hideCoroutine != null)
                StopCoroutine(_hideCoroutine);
            _hideCoroutine = StartCoroutine(HideAfterDelay());
        }

        public void HideWarning()
        {
            if (warningPanel != null)
                warningPanel.SetActive(false);
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(displayDuration);
            HideWarning();
            _hideCoroutine = null;
        }
    }
}
