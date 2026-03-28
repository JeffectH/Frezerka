using Frezerka.Scenario.Interfaces;

namespace Frezerka.Scenario.Lathe.Steps
{
    public class PutOnSafetyGlassesStep : ScenarioStep
    {
        public PutOnSafetyGlassesStep()
        {
            StepId = "lathe_safety_glasses";
            StepNameRU = "Надеть защитные очки";
            StepNameEN = "Put on safety glasses";
            DescriptionRU = "Возьмите и наденьте защитные очки";
            DescriptionEN = "Pick up and put on safety glasses";
        }

        public override string[] RequiredInteractionIds => new[] { "safety_glasses" };
    }

    public class PutOnSafetyHeadphonesStep : ScenarioStep
    {
        public PutOnSafetyHeadphonesStep()
        {
            StepId = "lathe_safety_headphones";
            StepNameRU = "Надеть защитные наушники";
            StepNameEN = "Put on safety headphones";
            DescriptionRU = "Возьмите и наденьте защитные наушники";
            DescriptionEN = "Pick up and put on safety headphones";
        }

        public override string[] RequiredInteractionIds => new[] { "safety_headphones" };
    }
}
