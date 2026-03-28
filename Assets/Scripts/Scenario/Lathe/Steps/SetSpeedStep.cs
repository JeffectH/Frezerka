using Frezerka.Scenario.Interfaces;

namespace Frezerka.Scenario.Lathe.Steps
{
    public class SetSpeedStep : ScenarioStep
    {
        public SetSpeedStep()
        {
            StepId = "lathe_set_speed";
            StepNameRU = "Настроить скорость вращения";
            StepNameEN = "Set spindle speed";
            DescriptionRU = "Задайте скорость вращения шпинделя на панели управления";
            DescriptionEN = "Set the spindle rotation speed on the control panel";
        }

        public override string[] RequiredInteractionIds => new[] { "rpm_panel.speed_input" };

        protected override void OnRequiredInteraction(string interactionId, string value, IScenarioContext context)
        {
            Complete(context);
        }
    }

    public class SetTransmissionStep : ScenarioStep
    {
        public SetTransmissionStep()
        {
            StepId = "lathe_set_transmission";
            StepNameRU = "Выбрать передачу";
            StepNameEN = "Select transmission gear";
            DescriptionRU = "Выберите передачу на панели управления";
            DescriptionEN = "Select the transmission gear on the control panel";
        }

        public override string[] RequiredInteractionIds => new[] { "rpm_panel.transmission" };
    }
}
