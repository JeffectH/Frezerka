using Frezerka.Scenario.Interfaces;

namespace Frezerka.Scenario.Lathe.Steps
{
    public class CutPassStep : ScenarioStep
    {
        private readonly int _passNumber;
        private readonly float _targetDiameter;
        private readonly float _targetLength;
        private readonly string _cutDescriptionRU;
        private readonly string _cutDescriptionEN;

        public CutPassStep(int passNumber, string cutDescRU, string cutDescEN,
            float targetDiameter = 0f, float targetLength = 0f)
        {
            _passNumber = passNumber;
            _targetDiameter = targetDiameter;
            _targetLength = targetLength;
            _cutDescriptionRU = cutDescRU;
            _cutDescriptionEN = cutDescEN;

            StepId = $"lathe_cut_pass_{passNumber}";
            StepNameRU = $"Проход {passNumber}: {cutDescRU}";
            StepNameEN = $"Pass {passNumber}: {cutDescEN}";
            DescriptionRU = $"Выполните обработку: {cutDescRU}";
            DescriptionEN = $"Perform machining: {cutDescEN}";
        }

        public override string[] RequiredInteractionIds => new[]
        {
            "carriage_joystick", "manual_handles"
        };

        protected override void OnRequiredInteraction(string interactionId, string value, IScenarioContext context)
        {
            // Stub: any carriage movement counts as completing the cut pass
            context.StepData[$"pass_{_passNumber}_completed"] = true;
            Complete(context);
        }
    }
}
