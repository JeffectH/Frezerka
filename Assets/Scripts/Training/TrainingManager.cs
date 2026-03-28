using UnityEngine;
using Frezerka.Scenario;
using Frezerka.Scenario.Interfaces;
using Frezerka.Core;
using Frezerka.Interaction.Interfaces;

namespace Frezerka.Training
{
    public class TrainingManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ScenarioStateMachine scenarioStateMachine;
        [SerializeField] private TrainingHintUI hintUI;
        [SerializeField] private TrainingHighlighter highlighter;
        [SerializeField] private TrainingArrow arrow;

        [Header("Settings")]
        [SerializeField] private bool pauseOnHint = false;

        private bool _isActive;

        private void Start()
        {
            var gm = GameManager.Instance;
            _isActive = gm != null && gm.IsTrainingMode;

            if (!_isActive)
            {
                enabled = false;
                return;
            }

            if (scenarioStateMachine != null)
            {
                scenarioStateMachine.OnStepEntered += OnStepEntered;
                scenarioStateMachine.OnStepCompleted += OnStepCompleted;
            }
        }

        private void OnDestroy()
        {
            if (scenarioStateMachine != null)
            {
                scenarioStateMachine.OnStepEntered -= OnStepEntered;
                scenarioStateMachine.OnStepCompleted -= OnStepCompleted;
            }
        }

        private void OnStepEntered(IScenarioStep step)
        {
            if (!_isActive) return;

            // Show hint image
            string lang = GameManager.Instance?.CurrentLanguage == GameLanguage.RU ? "RU" : "Eng";
            string imageName = $"{step.HintImageName}-{lang}";
            hintUI?.ShowHint(imageName, step.StepNameRU, step.DescriptionRU);

            // Highlight target objects
            if (step.RequiredInteractionIds != null && step.RequiredInteractionIds.Length > 0)
            {
                highlighter?.HighlightObjects(step.RequiredInteractionIds);

                // Show arrow to first target
                string firstTarget = step.RequiredInteractionIds[0];
                var targetObj = FindInteractableById(firstTarget);
                if (targetObj != null)
                    arrow?.PointAt(targetObj.transform);
            }

            if (pauseOnHint)
                Time.timeScale = 0f;
        }

        private void OnStepCompleted(IScenarioStep step, float duration)
        {
            if (!_isActive) return;

            hintUI?.HideHint();
            highlighter?.ClearHighlights();
            arrow?.Hide();

            if (pauseOnHint)
                Time.timeScale = 1f;
        }

        private GameObject FindInteractableById(string interactionId)
        {
            var interactables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var mb in interactables)
            {
                if (mb is IInteractable interactable && interactable.InteractionId == interactionId)
                    return mb.gameObject;
            }
            return null;
        }
    }
}
