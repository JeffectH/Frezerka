namespace Frezerka.Scenario.Interfaces
{
    public enum StepResult
    {
        InProgress,
        Completed,
        Failed,
        Skipped
    }

    public interface IScenarioStep
    {
        string StepId { get; }
        string StepNameRU { get; }
        string StepNameEN { get; }
        string DescriptionRU { get; }
        string DescriptionEN { get; }
        int StepIndex { get; }
        bool IsCompleted { get; }
        bool IsFailed { get; }
        string[] RequiredInteractionIds { get; }
        string HintImageName { get; }

        void Enter(IScenarioContext context);
        void Exit(IScenarioContext context);
        StepResult Evaluate(IScenarioContext context);
        void OnInteraction(string interactionId, string value, IScenarioContext context);
    }
}
