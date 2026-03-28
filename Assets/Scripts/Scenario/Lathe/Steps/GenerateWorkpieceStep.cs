using Frezerka.Scenario.Interfaces;

namespace Frezerka.Scenario.Lathe.Steps
{
    public class GenerateWorkpieceStep : ScenarioStep
    {
        public GenerateWorkpieceStep()
        {
            StepId = "lathe_generate_workpiece";
            StepNameRU = "Сгенерировать заготовку";
            StepNameEN = "Generate workpiece";
            DescriptionRU = "Нажмите кнопку генерации заготовки";
            DescriptionEN = "Press the generate workpiece button";
        }

        public override string[] RequiredInteractionIds => new[] { "workpiece_param_panel.generate_btn" };
    }
}
