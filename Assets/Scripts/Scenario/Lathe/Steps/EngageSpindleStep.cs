using Frezerka.Scenario.Interfaces;

namespace Frezerka.Scenario.Lathe.Steps
{
    public class EngageSpindleStep : ScenarioStep
    {
        private readonly bool _engage;

        public EngageSpindleStep(bool engage = true)
        {
            _engage = engage;

            if (engage)
            {
                StepId = "lathe_engage_spindle";
                StepNameRU = "Включить вращение шпинделя";
                StepNameEN = "Engage spindle rotation";
                DescriptionRU = "Потяните рычаг включения шпинделя вверх (по часовой) или вниз (против часовой)";
                DescriptionEN = "Pull the spindle engagement lever up (CW) or down (CCW)";
            }
            else
            {
                StepId = "lathe_disengage_spindle";
                StepNameRU = "Остановить вращение шпинделя";
                StepNameEN = "Disengage spindle rotation";
                DescriptionRU = "Переведите рычаг шпинделя в нейтральное положение";
                DescriptionEN = "Move the spindle lever to neutral position";
            }
        }

        public override string[] RequiredInteractionIds => new[] { "spindle_lever" };
    }

    public class SetCarriageSpeedStep : ScenarioStep
    {
        public SetCarriageSpeedStep()
        {
            StepId = "lathe_set_carriage_speed";
            StepNameRU = "Настроить скорость суппорта";
            StepNameEN = "Set carriage feed speed";
            DescriptionRU = "Используйте регулятор для настройки скорости перемещения суппорта";
            DescriptionEN = "Use the regulator to set the carriage movement speed";
        }

        public override string[] RequiredInteractionIds => new[] { "carriage_speed_control" };
    }
}
