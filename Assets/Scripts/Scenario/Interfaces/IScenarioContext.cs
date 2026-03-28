using System.Collections.Generic;
using Frezerka.Core;
using Frezerka.Machines.Interfaces;

namespace Frezerka.Scenario.Interfaces
{
    public interface IScenarioContext
    {
        IMachine Machine { get; }
        SessionMode Mode { get; }
        WorkpieceParams WorkpieceParams { get; set; }
        string SelectedToolId { get; set; }
        Dictionary<string, object> StepData { get; }

        void NotifyStepCompleted(string stepId);
        void NotifyError(string stepId, string errorType, string details);
        void NotifySafetyViolation(string violationType);
    }
}
