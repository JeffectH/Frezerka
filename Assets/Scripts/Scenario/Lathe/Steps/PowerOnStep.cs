using Frezerka.Scenario.Interfaces;

namespace Frezerka.Scenario.Lathe.Steps
{
    public class PowerOnStep : ScenarioStep
    {
        public PowerOnStep()
        {
            StepId = "lathe_power_on";
            StepNameRU = "Включить токарный станок";
            StepNameEN = "Turn on the lathe";
            DescriptionRU = "Нажмите синюю кнопку включения станка";
            DescriptionEN = "Press the blue power-on button";
        }

        public override string[] RequiredInteractionIds => new[] { "lathe_btn_on" };
    }

    public class PowerOffStep : ScenarioStep
    {
        public PowerOffStep()
        {
            StepId = "lathe_power_off";
            StepNameRU = "Выключить токарный станок";
            StepNameEN = "Turn off the lathe";
            DescriptionRU = "Нажмите кнопку выключения станка";
            DescriptionEN = "Press the power-off button";
        }

        public override string[] RequiredInteractionIds => new[] { "lathe_btn_off" };
    }
}
