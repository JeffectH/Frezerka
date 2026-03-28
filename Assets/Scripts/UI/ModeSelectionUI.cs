using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Frezerka.Core;

namespace Frezerka.UI
{
    public class ModeSelectionUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button trainingButton;
        [SerializeField] private Button normalButton;
        [SerializeField] private TextMeshProUGUI selectedModeText;

        [Header("Visual Feedback")]
        [SerializeField] private Color selectedColor = new Color(0.2f, 0.8f, 0.2f);
        [SerializeField] private Color defaultColor = Color.white;

        private void Start()
        {
            if (trainingButton != null)
                trainingButton.onClick.AddListener(SelectTraining);
            if (normalButton != null)
                normalButton.onClick.AddListener(SelectNormal);

            UpdateVisuals();
        }

        public void SelectTraining()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.CurrentMode = SessionMode.Training;
            UpdateVisuals();
        }

        public void SelectNormal()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.CurrentMode = SessionMode.Normal;
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            var gm = GameManager.Instance;
            if (gm == null) return;

            bool isTraining = gm.CurrentMode == SessionMode.Training;

            if (trainingButton != null)
            {
                var colors = trainingButton.colors;
                colors.normalColor = isTraining ? selectedColor : defaultColor;
                trainingButton.colors = colors;
            }

            if (normalButton != null)
            {
                var colors = normalButton.colors;
                colors.normalColor = !isTraining ? selectedColor : defaultColor;
                normalButton.colors = colors;
            }

            if (selectedModeText != null)
                selectedModeText.text = isTraining ? "Режим: Обучение" : "Режим: Обычный";
        }
    }
}
