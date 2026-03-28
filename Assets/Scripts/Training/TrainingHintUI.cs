using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Frezerka.Training
{
    public class TrainingHintUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject hintPanel;
        [SerializeField] private Image hintImage;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button dismissButton;

        [Header("Settings")]
        [SerializeField] private string hintSpritesPath = "Training/";

        private void Start()
        {
            if (dismissButton != null)
                dismissButton.onClick.AddListener(HideHint);

            HideHint();
        }

        public void ShowHint(string imageName, string title, string description)
        {
            if (hintPanel != null)
                hintPanel.SetActive(true);

            if (hintImage != null)
            {
                var sprite = Resources.Load<Sprite>(hintSpritesPath + imageName);
                if (sprite != null)
                    hintImage.sprite = sprite;
                else
                    Debug.LogWarning($"[TrainingHintUI] Sprite not found: {hintSpritesPath}{imageName}");
            }

            if (titleText != null)
                titleText.text = title;

            if (descriptionText != null)
                descriptionText.text = description;
        }

        public void HideHint()
        {
            if (hintPanel != null)
                hintPanel.SetActive(false);
        }
    }
}
