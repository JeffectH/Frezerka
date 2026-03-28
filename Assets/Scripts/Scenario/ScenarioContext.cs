using System.Collections.Generic;
using Frezerka.Core;
using Frezerka.Machines.Interfaces;
using Frezerka.Scenario.Interfaces;
using Frezerka.Utility;

namespace Frezerka.Scenario
{
    public class ScenarioContext : IScenarioContext
    {
        public IMachine Machine { get; set; }
        public SessionMode Mode { get; set; }
        public WorkpieceParams WorkpieceParams { get; set; }
        public string SelectedToolId { get; set; }
        public Dictionary<string, object> StepData { get; } = new Dictionary<string, object>();

        public void NotifyStepCompleted(string stepId)
        {
            EventBus.Publish(new StepCompletedEvent
            {
                StepId = stepId,
                StepIndex = -1,
                DurationSeconds = 0f
            });
        }

        public void NotifyError(string stepId, string errorType, string details)
        {
            EventBus.Publish(new ErrorEvent
            {
                StepId = stepId,
                ErrorType = errorType,
                Details = details,
                StepTimeAtError = UnityEngine.Time.time
            });
        }

        public void NotifySafetyViolation(string violationType)
        {
            EventBus.Publish(new SafetyViolationEvent
            {
                ViolationType = violationType,
                ActiveStepId = "",
                PlayerPosition = UnityEngine.Vector3.zero,
                Description = violationType
            });
        }
    }
}
