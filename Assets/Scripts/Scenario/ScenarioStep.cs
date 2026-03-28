using System;
using Frezerka.Scenario.Interfaces;

namespace Frezerka.Scenario
{
    [Serializable]
    public abstract class ScenarioStep : IScenarioStep
    {
        public string StepId { get; protected set; }
        public string StepNameRU { get; protected set; }
        public string StepNameEN { get; protected set; }
        public string DescriptionRU { get; protected set; }
        public string DescriptionEN { get; protected set; }
        public int StepIndex { get; set; }
        public bool IsCompleted { get; protected set; }
        public bool IsFailed { get; protected set; }
        public virtual string[] RequiredInteractionIds => Array.Empty<string>();
        public virtual string HintImageName => $"{StepIndex + 1}";

        protected bool _interactionReceived;
        protected string _lastInteractionId;

        public virtual void Enter(IScenarioContext context)
        {
            IsCompleted = false;
            IsFailed = false;
            _interactionReceived = false;
            _lastInteractionId = null;
        }

        public virtual void Exit(IScenarioContext context)
        {
        }

        public virtual StepResult Evaluate(IScenarioContext context)
        {
            if (IsCompleted) return StepResult.Completed;
            if (IsFailed) return StepResult.Failed;
            return StepResult.InProgress;
        }

        public virtual void OnInteraction(string interactionId, string value, IScenarioContext context)
        {
            _lastInteractionId = interactionId;

            foreach (var required in RequiredInteractionIds)
            {
                if (interactionId == required)
                {
                    _interactionReceived = true;
                    OnRequiredInteraction(interactionId, value, context);
                    return;
                }
            }
        }

        protected virtual void OnRequiredInteraction(string interactionId, string value, IScenarioContext context)
        {
            Complete(context);
        }

        protected void Complete(IScenarioContext context)
        {
            IsCompleted = true;
            context.NotifyStepCompleted(StepId);
        }

        protected void Fail(IScenarioContext context, string errorType, string details)
        {
            IsFailed = true;
            context.NotifyError(StepId, errorType, details);
        }
    }
}
