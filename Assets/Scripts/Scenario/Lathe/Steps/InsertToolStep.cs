using Frezerka.Scenario.Interfaces;

namespace Frezerka.Scenario.Lathe.Steps
{
    public class InsertToolStep : ScenarioStep
    {
        private readonly int _slotIndex;

        public InsertToolStep(int slotIndex)
        {
            _slotIndex = slotIndex;
            StepId = $"lathe_insert_tool_slot_{slotIndex + 1}";
            StepNameRU = $"Вставить резец в позицию {slotIndex + 1}";
            StepNameEN = $"Insert tool into position {slotIndex + 1}";
            DescriptionRU = $"Поместите резец в резцедержатель, позиция {slotIndex + 1}";
            DescriptionEN = $"Place the cutting tool into tool holder, position {slotIndex + 1}";
        }

        public override string[] RequiredInteractionIds => new[] { $"tool_holder.slot_{_slotIndex}" };
    }
}
