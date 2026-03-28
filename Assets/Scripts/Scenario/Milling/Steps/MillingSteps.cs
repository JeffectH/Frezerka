using Frezerka.Scenario.Interfaces;

namespace Frezerka.Scenario.Milling.Steps
{
    // Reuse safety gear steps from Lathe namespace — they are generic

    public class MillSetWorkpieceLengthStep : ScenarioStep
    {
        public MillSetWorkpieceLengthStep()
        {
            StepId = "mill_set_length";
            StepNameRU = "Задать длину заготовки";
            StepNameEN = "Set workpiece length";
            DescriptionRU = "Введите длину заготовки на панели параметров";
            DescriptionEN = "Enter workpiece length on the parameters panel";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_param_panel.length" };
        protected override void OnRequiredInteraction(string id, string value, IScenarioContext ctx)
        {
            if (float.TryParse(value, out float v)) { var p = ctx.WorkpieceParams; p.Length = v; ctx.WorkpieceParams = p; }
            Complete(ctx);
        }
    }

    public class MillSetWorkpieceWidthStep : ScenarioStep
    {
        public MillSetWorkpieceWidthStep()
        {
            StepId = "mill_set_width";
            StepNameRU = "Задать ширину заготовки";
            StepNameEN = "Set workpiece width";
            DescriptionRU = "Введите ширину заготовки";
            DescriptionEN = "Enter workpiece width";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_param_panel.width" };
        protected override void OnRequiredInteraction(string id, string value, IScenarioContext ctx)
        {
            if (float.TryParse(value, out float v)) { var p = ctx.WorkpieceParams; p.Width = v; ctx.WorkpieceParams = p; }
            Complete(ctx);
        }
    }

    public class MillSetWorkpieceHeightStep : ScenarioStep
    {
        public MillSetWorkpieceHeightStep()
        {
            StepId = "mill_set_height";
            StepNameRU = "Задать высоту заготовки";
            StepNameEN = "Set workpiece height";
            DescriptionRU = "Введите высоту заготовки";
            DescriptionEN = "Enter workpiece height";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_param_panel.height" };
        protected override void OnRequiredInteraction(string id, string value, IScenarioContext ctx)
        {
            if (float.TryParse(value, out float v)) { var p = ctx.WorkpieceParams; p.Height = v; ctx.WorkpieceParams = p; }
            Complete(ctx);
        }
    }

    public class MillGenerateWorkpieceStep : ScenarioStep
    {
        public MillGenerateWorkpieceStep()
        {
            StepId = "mill_generate_workpiece";
            StepNameRU = "Сгенерировать заготовку";
            StepNameEN = "Generate workpiece";
            DescriptionRU = "Нажмите кнопку генерации";
            DescriptionEN = "Press the generate button";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_param_panel.generate_btn" };
    }

    public class MillSelectCutterStep : ScenarioStep
    {
        public MillSelectCutterStep()
        {
            StepId = "mill_select_cutter";
            StepNameRU = "Выбрать фрезу";
            StepNameEN = "Select cutter";
            DescriptionRU = "Выберите тип фрезы (торцевая или концевая) на панели настройки";
            DescriptionEN = "Select cutter type (face or end mill) on the setup panel";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_cutter_panel.select" };
    }

    public class MillConfigureSpeedStep : ScenarioStep
    {
        public MillConfigureSpeedStep()
        {
            StepId = "mill_configure_speed";
            StepNameRU = "Настроить скорость";
            StepNameEN = "Configure speed";
            DescriptionRU = "Задайте скорость вращения и перемещения фрезы на панели управления";
            DescriptionEN = "Set rotation and movement speed on the control panel";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_control_panel.speed" };
    }

    public class MillCloseDoorStep : ScenarioStep
    {
        public MillCloseDoorStep()
        {
            StepId = "mill_close_door";
            StepNameRU = "Закрыть дверь";
            StepNameEN = "Close the door";
            DescriptionRU = "Закройте защитную дверь фрезерного станка";
            DescriptionEN = "Close the milling machine safety door";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_door" };
    }

    public class MillLockDoorStep : ScenarioStep
    {
        public MillLockDoorStep()
        {
            StepId = "mill_lock_door";
            StepNameRU = "Заблокировать дверь";
            StepNameEN = "Lock the door";
            DescriptionRU = "Нажмите кнопку с иконкой замка на панели управления";
            DescriptionEN = "Press the lock icon button on the control panel";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_control_panel.lock" };
    }

    public class MillPowerOnStep : ScenarioStep
    {
        public MillPowerOnStep()
        {
            StepId = "mill_power_on";
            StepNameRU = "Включить фрезерный станок";
            StepNameEN = "Turn on the milling machine";
            DescriptionRU = "Нажмите кнопку включения";
            DescriptionEN = "Press the power-on button";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_btn_on" };
    }

    public class MillRemoteControlStep : ScenarioStep
    {
        public MillRemoteControlStep()
        {
            StepId = "mill_remote_control";
            StepNameRU = "Управление пультом";
            StepNameEN = "Remote control operation";
            DescriptionRU = "Используйте пульт для перемещения фрезы и обработки детали";
            DescriptionEN = "Use the remote to move the cutter and machine the workpiece";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_remote.move_plus", "mill_remote.move_minus" };
    }

    public class MillPowerOffStep : ScenarioStep
    {
        public MillPowerOffStep()
        {
            StepId = "mill_power_off";
            StepNameRU = "Выключить фрезерный станок";
            StepNameEN = "Turn off the milling machine";
            DescriptionRU = "Нажмите кнопку выключения";
            DescriptionEN = "Press the power-off button";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_btn_off" };
    }

    public class MillUnlockDoorStep : ScenarioStep
    {
        public MillUnlockDoorStep()
        {
            StepId = "mill_unlock_door";
            StepNameRU = "Разблокировать дверь";
            StepNameEN = "Unlock the door";
            DescriptionRU = "Нажмите кнопку разблокировки двери";
            DescriptionEN = "Press the unlock button";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_control_panel.unlock" };
    }

    public class MillOpenDoorStep : ScenarioStep
    {
        public MillOpenDoorStep()
        {
            StepId = "mill_open_door";
            StepNameRU = "Открыть дверь";
            StepNameEN = "Open the door";
            DescriptionRU = "Откройте дверь фрезерного станка";
            DescriptionEN = "Open the milling machine door";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_door" };
    }

    public class MillRemoveWorkpieceStep : ScenarioStep
    {
        public MillRemoveWorkpieceStep()
        {
            StepId = "mill_remove_workpiece";
            StepNameRU = "Снять деталь";
            StepNameEN = "Remove the workpiece";
            DescriptionRU = "Извлеките обработанную деталь из тисков";
            DescriptionEN = "Remove the finished part from the vise";
        }
        public override string[] RequiredInteractionIds => new[] { "mill_vise" };
    }

    public class MillSavePartStep : ScenarioStep
    {
        public MillSavePartStep()
        {
            StepId = "mill_save_part";
            StepNameRU = "Сохранить деталь";
            StepNameEN = "Save the part";
            DescriptionRU = "Поместите деталь в зону сканирования и нажмите сохранить";
            DescriptionEN = "Place the part in the scanning area and press save";
        }
        public override string[] RequiredInteractionIds => new[] { "save_button" };
    }
}
