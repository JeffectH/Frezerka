using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Frezerka.Core;

namespace Frezerka.UI
{
    public class SettingsUI : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private TextMeshProUGUI volumeValueText;

        [Header("Language")]
        [SerializeField] private Button ruButton;
        [SerializeField] private Button enButton;

        [Header("Participant")]
        [SerializeField] private TMP_InputField participantIdInput;

        private void Start()
        {
            if (volumeSlider != null)
            {
                volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
                volumeSlider.value = AudioListener.volume;
            }

            if (ruButton != null)
                ruButton.onClick.AddListener(() => SetLanguage(GameLanguage.RU));
            if (enButton != null)
                enButton.onClick.AddListener(() => SetLanguage(GameLanguage.EN));

            if (participantIdInput != null)
            {
                var gm = GameManager.Instance;
                if (gm != null)
                    participantIdInput.text = gm.ParticipantId;

                participantIdInput.onEndEdit.AddListener(OnParticipantIdChanged);
            }
        }

        private void OnVolumeChanged(float value)
        {
            AudioListener.volume = value;
            if (volumeValueText != null)
                volumeValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
        }

        private void SetLanguage(GameLanguage lang)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.CurrentLanguage = lang;
            Debug.Log($"[SettingsUI] Language: {lang}");
        }

        private void OnParticipantIdChanged(string id)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.ParticipantId = id;
            Debug.Log($"[SettingsUI] Participant ID: {id}");
        }
    }
}
