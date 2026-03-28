using Frezerka.Scenario.Interfaces;

namespace Frezerka.Scenario.Lathe.Steps
{
    public class RemoveWorkpieceStep : ScenarioStep
    {
        public RemoveWorkpieceStep()
        {
            StepId = "lathe_remove_workpiece";
            StepNameRU = "Снять деталь";
            StepNameEN = "Remove the workpiece";
            DescriptionRU = "Снимите обработанную деталь из патрона";
            DescriptionEN = "Remove the finished part from the chuck";
        }

        public override string[] RequiredInteractionIds => new[] { "chuck" };
    }

    public class SavePartStep : ScenarioStep
    {
        public SavePartStep()
        {
            StepId = "lathe_save_part";
            StepNameRU = "Сохранить деталь";
            StepNameEN = "Save the part";
            DescriptionRU = "Поместите деталь в зону сканирования на верстаке и нажмите кнопку сохранения";
            DescriptionEN = "Place the part in the scanning area on the workbench and press save";
        }

        public override string[] RequiredInteractionIds => new[] { "save_button" };
    }
}
