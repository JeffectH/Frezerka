using System.Collections.Generic;
using Frezerka.Scenario.Interfaces;
using Frezerka.Scenario.Lathe.Steps;
using Frezerka.Scenario.Milling.Steps;

namespace Frezerka.Scenario.Milling
{
    public static class MillingScenarioDefinition
    {
        public static List<IScenarioStep> CreateSteps()
        {
            return new List<IScenarioStep>
            {
                // 1-2: Safety gear (reuse from lathe)
                new PutOnSafetyGlassesStep(),
                new PutOnSafetyHeadphonesStep(),

                // 3-5: Workpiece parameters
                new MillSetWorkpieceLengthStep(),
                new MillSetWorkpieceWidthStep(),
                new MillSetWorkpieceHeightStep(),

                // 6: Generate workpiece
                new MillGenerateWorkpieceStep(),

                // 7: Select cutter
                new MillSelectCutterStep(),

                // 8: Configure speed
                new MillConfigureSpeedStep(),

                // 9-10: Close and lock door
                new MillCloseDoorStep(),
                new MillLockDoorStep(),

                // 11: Power on
                new MillPowerOnStep(),

                // 12-13: Remote control operation
                new MillRemoteControlStep(),

                // 14: Power off
                new MillPowerOffStep(),

                // 15-16: Unlock and open door
                new MillUnlockDoorStep(),
                new MillOpenDoorStep(),

                // 17-18: Remove and save
                new MillRemoveWorkpieceStep(),
                new MillSavePartStep()
            };
        }
    }
}
