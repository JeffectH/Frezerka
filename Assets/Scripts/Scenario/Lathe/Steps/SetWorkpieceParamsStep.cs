using Frezerka.Machines.Interfaces;
using Frezerka.Scenario.Interfaces;

namespace Frezerka.Scenario.Lathe.Steps
{
    public class SetWorkpieceDiameterStep : ScenarioStep
    {
        private readonly float _expectedDiameter;

        public SetWorkpieceDiameterStep(float expectedDiameter = 26f)
        {
            _expectedDiameter = expectedDiameter;
            StepId = "lathe_set_diameter";
            StepNameRU = "Задать диаметр заготовки";
            StepNameEN = "Set workpiece diameter";
            DescriptionRU = $"Введите диаметр заготовки: {_expectedDiameter} мм";
            DescriptionEN = $"Enter workpiece diameter: {_expectedDiameter} mm";
        }

        public override string[] RequiredInteractionIds => new[] { "workpiece_param_panel.diameter" };

        protected override void OnRequiredInteraction(string interactionId, string value, IScenarioContext context)
        {
            if (float.TryParse(value, out float diameter))
            {
                var p = context.WorkpieceParams;
                p.Diameter = diameter;
                context.WorkpieceParams = p;

                if (System.Math.Abs(diameter - _expectedDiameter) < 0.1f)
                    Complete(context);
                else
                    context.NotifyError(StepId, "wrong_value", $"Введён диаметр {diameter} вместо {_expectedDiameter}");
            }
        }
    }

    public class SetWorkpieceLengthStep : ScenarioStep
    {
        private readonly float _expectedLength;

        public SetWorkpieceLengthStep(float expectedLength = 64f)
        {
            _expectedLength = expectedLength;
            StepId = "lathe_set_length";
            StepNameRU = "Задать длину заготовки";
            StepNameEN = "Set workpiece length";
            DescriptionRU = $"Введите длину заготовки: {_expectedLength} мм";
            DescriptionEN = $"Enter workpiece length: {_expectedLength} mm";
        }

        public override string[] RequiredInteractionIds => new[] { "workpiece_param_panel.length" };

        protected override void OnRequiredInteraction(string interactionId, string value, IScenarioContext context)
        {
            if (float.TryParse(value, out float length))
            {
                var p = context.WorkpieceParams;
                p.Length = length;
                context.WorkpieceParams = p;

                if (System.Math.Abs(length - _expectedLength) < 0.1f)
                    Complete(context);
                else
                    context.NotifyError(StepId, "wrong_value", $"Введена длина {length} вместо {_expectedLength}");
            }
        }
    }

    public class SetWorkpieceSpindleDepthStep : ScenarioStep
    {
        public SetWorkpieceSpindleDepthStep()
        {
            StepId = "lathe_set_spindle_depth";
            StepNameRU = "Задать глубину посадки в шпиндель";
            StepNameEN = "Set spindle seating depth";
            DescriptionRU = "Введите глубину погружения заготовки в шпиндель";
            DescriptionEN = "Enter the depth of workpiece insertion into spindle";
        }

        public override string[] RequiredInteractionIds => new[] { "workpiece_param_panel.depth" };

        protected override void OnRequiredInteraction(string interactionId, string value, IScenarioContext context)
        {
            if (float.TryParse(value, out float depth))
            {
                var p = context.WorkpieceParams;
                p.SpindleDepth = depth;
                context.WorkpieceParams = p;
                Complete(context);
            }
        }
    }
}
