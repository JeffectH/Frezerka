namespace Frezerka.Machines.Interfaces
{
    public enum CuttingToolType
    {
        // Lathe tools
        LathePassingBent,       // Проходной отогнутый
        LathePassingPersistent, // Проходной упорный
        LatheGrooving,          // Канавочный
        LatheCutoff,            // Отрезной

        // Milling tools
        MillingFace,            // Торцевая фреза
        MillingEnd              // Концевая фреза
    }

    public interface ICuttingTool
    {
        string ToolId { get; }
        string ToolName { get; }
        CuttingToolType ToolType { get; }
        int ToolHolderPosition { get; }
    }
}
