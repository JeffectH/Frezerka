using UnityEngine;
using UnityEngine.SceneManagement;
using Frezerka.Core;

namespace Frezerka.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject settingsPanel;

        [Header("Scene Names")]
        [SerializeField] private string latheSceneName = "Lathe";
        [SerializeField] private string millingSceneName = "Milling";

        public void OnLatheSelected()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.SelectedMachineType = "Lathe";

            LoadScene(latheSceneName);
        }

        public void OnMillingSelected()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.SelectedMachineType = "Milling";

            LoadScene(millingSceneName);
        }

        public void OnSettingsClicked()
        {
            if (mainPanel != null) mainPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(true);
        }

        public void OnSettingsBack()
        {
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (mainPanel != null) mainPanel.SetActive(true);
        }

        public void OnQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void LoadScene(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
