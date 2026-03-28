using System.Collections.Generic;
using Frezerka.Scenario.Interfaces;
using Frezerka.Scenario.Lathe.Steps;

namespace Frezerka.Scenario.Lathe
{
    public static class LatheScenarioDefinition
    {
        /// <summary>
        /// Creates the full ordered list of steps for the lathe scenario.
        /// Based on the "Палец" part: D=26mm, L=64mm blank, 6 passes + flip + 2 passes.
        /// </summary>
        public static List<IScenarioStep> CreateSteps()
        {
            return new List<IScenarioStep>
            {
                // 1-2: Safety gear
                new PutOnSafetyGlassesStep(),
                new PutOnSafetyHeadphonesStep(),

                // 3-5: Workpiece parameters
                new SetWorkpieceDiameterStep(26f),
                new SetWorkpieceLengthStep(64f),
                new SetWorkpieceSpindleDepthStep(),

                // 6: Generate workpiece
                new GenerateWorkpieceStep(),

                // 7-8: Select and insert tools (4 tools for 4 positions)
                new SelectToolStep(1, "LathePassingBent", "проходной отогнутый резец", "passing bent tool"),
                new InsertToolStep(0),

                // 9: Power on
                new PowerOnStep(),

                // 10-11: Speed and transmission
                new SetSpeedStep(),
                new SetTransmissionStep(),

                // 12: Engage spindle
                new EngageSpindleStep(true),

                // 13: Set carriage speed
                new SetCarriageSpeedStep(),

                // 14-19: Six cutting passes (first side)
                new CutPassStep(1,
                    "Подрезать торец в размер 64 мм",
                    "Face cut to 64 mm size",
                    targetLength: 64f),

                new CutPassStep(2,
                    "Точить D22 мм на длину 59 мм",
                    "Turn D22 mm for 59 mm length",
                    targetDiameter: 22f, targetLength: 59f),

                new CutPassStep(3,
                    "Точить D20 мм на длину 50 мм",
                    "Turn D20 mm for 50 mm length",
                    targetDiameter: 20f, targetLength: 50f),

                new CutPassStep(4,
                    "Канавка глубиной 1 мм на длине 50 мм",
                    "Groove depth 1 mm at 50 mm length"),

                new CutPassStep(5,
                    "Фаска 2×45°",
                    "Chamfer 2×45°"),

                new CutPassStep(6,
                    "Отрезать заготовку на 59 мм от торца",
                    "Cut off workpiece at 59 mm from end",
                    targetLength: 59f),

                // 20: Disengage spindle for flip
                new EngageSpindleStep(false),

                // 21: Flip workpiece
                new FlipWorkpieceStep(),

                // 22: Re-engage spindle
                new EngageSpindleStep(true),

                // 23-24: Two more passes (second side)
                new CutPassStep(7,
                    "Подрезать торец в размер 15 мм",
                    "Face cut to 15 mm",
                    targetLength: 15f),

                new CutPassStep(8,
                    "Фаска 2×45°",
                    "Chamfer 2×45°"),

                // 25: Disengage and power off
                new EngageSpindleStep(false),
                new PowerOffStep(),

                // 26-27: Remove and save
                new RemoveWorkpieceStep(),
                new SavePartStep()
            };
        }
    }
}
