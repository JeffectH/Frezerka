using Frezerka.Scenario.Interfaces;

namespace Frezerka.Scenario.Lathe.Steps
{
    public class SelectToolStep : ScenarioStep
    {
        private readonly string _expectedToolType;

        public SelectToolStep(int stepNumber, string expectedToolType, string toolNameRU, string toolNameEN)
        {
            _expectedToolType = expectedToolType;
            StepId = $"lathe_select_tool_{stepNumber}";
            StepNameRU = $"Выбрать {toolNameRU}";
            StepNameEN = $"Select {toolNameEN}";
            DescriptionRU = $"Возьмите {toolNameRU} со стойки";
            DescriptionEN = $"Take {toolNameEN} from the rack";
        }

        public override string[] RequiredInteractionIds => new[]
        {
            "cutter_rack.tool_1", "cutter_rack.tool_2",
            "cutter_rack.tool_3", "cutter_rack.tool_4"
        };

        protected override void OnRequiredInteraction(string interactionId, string value, IScenarioContext context)
        {
            context.SelectedToolId = interactionId;
            Complete(context);
        }
    }
}
