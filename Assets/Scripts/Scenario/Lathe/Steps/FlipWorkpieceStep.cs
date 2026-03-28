using Frezerka.Scenario.Interfaces;

namespace Frezerka.Scenario.Lathe.Steps
{
    public class FlipWorkpieceStep : ScenarioStep
    {
        public FlipWorkpieceStep()
        {
            StepId = "lathe_flip_workpiece";
            StepNameRU = "Перевернуть заготовку";
            StepNameEN = "Flip the workpiece";
            DescriptionRU = "Перевернуть заготовку и закрепить с вылетом 18 мм";
            DescriptionEN = "Flip the workpiece and secure with 18 mm overhang";
        }

        public override string[] RequiredInteractionIds => new[] { "chuck" };
    }
}
