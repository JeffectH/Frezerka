using UnityEngine;
using Frezerka.Machines.Interfaces;
using Frezerka.Scenario;
using Frezerka.Scenario.Lathe;
using Frezerka.Scenario.Milling;
using Frezerka.Utility;

namespace Frezerka.Core
{
    public class SessionManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ScenarioStateMachine scenarioStateMachine;

        [Header("State")]
        [SerializeField] private bool sessionActive;

        private float _sessionStartTime;
        private ScenarioContext _context;

        public bool IsSessionActive => sessionActive;
        public float SessionElapsedTime => sessionActive ? Time.time - _sessionStartTime : 0f;

        private void OnEnable()
        {
            EventBus.Subscribe<InteractionEvent>(OnInteraction);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<InteractionEvent>(OnInteraction);
        }

        public void StartSession(IMachine machine)
        {
            if (sessionActive) return;

            var gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                Debug.LogError("[SessionManager] GameManager not found");
                return;
            }

            sessionActive = true;
            _sessionStartTime = Time.time;

            _context = new ScenarioContext
            {
                Machine = machine,
                Mode = gameManager.CurrentMode
            };

            // Create steps based on machine type
            var steps = machine.MachineType switch
            {
                MachineType.Lathe => LatheScenarioDefinition.CreateSteps(),
                MachineType.Milling => MillingScenarioDefinition.CreateSteps(),
                _ => LatheScenarioDefinition.CreateSteps()
            };

            scenarioStateMachine.Initialize(steps, _context);
            scenarioStateMachine.OnScenarioCompleted += OnScenarioCompleted;
            scenarioStateMachine.StartScenario();

            EventBus.Publish(new SessionEvent
            {
                EventType = SessionEvent.SessionEventType.Started,
                ParticipantId = gameManager.ParticipantId,
                MachineType = machine.MachineType.ToString(),
                SessionMode = gameManager.CurrentMode.ToString()
            });

            Debug.Log($"[SessionManager] Session started: {gameManager.ParticipantId}, {machine.MachineType}, {gameManager.CurrentMode}");
        }

        public void EndSession()
        {
            if (!sessionActive) return;

            sessionActive = false;

            scenarioStateMachine.OnScenarioCompleted -= OnScenarioCompleted;

            EventBus.Publish(new SessionEvent
            {
                EventType = SessionEvent.SessionEventType.Ended,
                ParticipantId = GameManager.Instance?.ParticipantId ?? "",
                MachineType = _context?.Machine?.MachineType.ToString() ?? "",
                SessionMode = GameManager.Instance?.CurrentMode.ToString() ?? ""
            });

            Debug.Log($"[SessionManager] Session ended. Duration: {SessionElapsedTime:F1}s");
        }

        private void OnScenarioCompleted()
        {
            Debug.Log("[SessionManager] Scenario completed!");
            EndSession();
        }

        private void OnInteraction(InteractionEvent evt)
        {
            if (!sessionActive) return;
            scenarioStateMachine.HandleInteraction(evt.InteractionId, evt.Value);
        }
    }
}
