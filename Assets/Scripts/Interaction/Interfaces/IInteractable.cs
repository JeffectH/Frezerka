namespace Frezerka.Interaction.Interfaces
{
    public enum InteractionType
    {
        Click,
        Grab,
        Toggle,
        Drag,
        NumpadInput
    }

    public interface IInteractable
    {
        string InteractionId { get; }
        string DisplayName { get; }
        bool IsInteractable { get; set; }
        void OnHoverEnter();
        void OnHoverExit();
        void OnInteract();
        InteractionType GetInteractionType();
    }
}
