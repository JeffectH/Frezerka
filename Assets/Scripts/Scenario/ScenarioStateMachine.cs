using System;
using System.Collections.Generic;
using UnityEngine;
using Frezerka.Scenario.Interfaces;
using Frezerka.Utility;

namespace Frezerka.Scenario
{
    public class ScenarioStateMachine : MonoBehaviour
    {
        public enum StateMachineState { Idle, Running, Completed, Failed }

        private List<IScenarioStep> _steps = new List<IScenarioStep>();
        private IScenarioContext _context;
        private int _currentStepIndex = -1;
        private float _stepStartTime;
        private StateMachineState _state = StateMachineState.Idle;

        public StateMachineState State => _state;
        public int CurrentStepIndex => _currentStepIndex;
        public int TotalSteps => _steps.Count;
        public IScenarioStep CurrentStep => _currentStepIndex >= 0 && _currentStepIndex < _steps.Count
            ? _steps[_currentStepIndex]
            : null;

        public event Action<IScenarioStep> OnStepEntered;
        public event Action<IScenarioStep, float> OnStepCompleted;
        public event Action<IScenarioStep> OnStepFailed;
        public event Action OnScenarioCompleted;

        public void Initialize(List<IScenarioStep> steps, IScenarioContext context)
        {
            _steps = steps;
            _context = context;

            // Set step indices
            for (int i = 0; i < _steps.Count; i++)
                _steps[i].StepIndex = i;

            _state = StateMachineState.Idle;
            _currentStepIndex = -1;
        }

        public void StartScenario()
        {
            if (_steps.Count == 0)
            {
                Debug.LogWarning("[ScenarioStateMachine] No steps defined");
                return;
            }

            _state = StateMachineState.Running;
            EnterStep(0);
        }

        private void Update()
        {
            if (_state != StateMachineState.Running) return;
            if (CurrentStep == null) return;

            var result = CurrentStep.Evaluate(_context);
            switch (result)
            {
                case StepResult.Completed:
                    CompleteCurrentStep();
                    break;
                case StepResult.Failed:
                    OnStepFailed?.Invoke(CurrentStep);
                    // Allow retry — re-enter the same step
                    EnterStep(_currentStepIndex);
                    break;
            }
        }

        public void HandleInteraction(string interactionId, string value = "")
        {
            if (_state != StateMachineState.Running) return;
            CurrentStep?.OnInteraction(interactionId, value, _context);
        }

        private void EnterStep(int index)
        {
            if (index < 0 || index >= _steps.Count) return;

            _currentStepIndex = index;
            _stepStartTime = Time.time;

            var step = _steps[index];
            step.Enter(_context);

            EventBus.Publish(new StepChangedEvent
            {
                PreviousStepId = index > 0 ? _steps[index - 1].StepId : "",
                NewStepId = step.StepId,
                StepIndex = index,
                TotalSteps = _steps.Count
            });

            OnStepEntered?.Invoke(step);

            Debug.Log($"[Scenario] Step {index + 1}/{_steps.Count}: {step.StepNameRU}");
        }

        private void CompleteCurrentStep()
        {
            float duration = Time.time - _stepStartTime;
            var completedStep = CurrentStep;

            completedStep.Exit(_context);

            EventBus.Publish(new StepCompletedEvent
            {
                StepId = completedStep.StepId,
                StepIndex = _currentStepIndex,
                DurationSeconds = duration
            });

            OnStepCompleted?.Invoke(completedStep, duration);

            // Advance to next step
            int nextIndex = _currentStepIndex + 1;
            if (nextIndex >= _steps.Count)
            {
                _state = StateMachineState.Completed;
                OnScenarioCompleted?.Invoke();
                Debug.Log("[Scenario] Completed!");
                return;
            }

            EnterStep(nextIndex);
        }

        public void ForceAdvance()
        {
            if (_state != StateMachineState.Running) return;
            CompleteCurrentStep();
        }

        public void Reset()
        {
            _state = StateMachineState.Idle;
            _currentStepIndex = -1;
        }
    }
}
