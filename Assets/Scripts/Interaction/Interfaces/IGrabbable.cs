using UnityEngine;

namespace Frezerka.Interaction.Interfaces
{
    public interface IGrabbable : IInteractable
    {
        bool IsGrabbed { get; }
        Transform GrabPoint { get; }
        void OnGrab();
        void OnRelease();
        bool CanPlaceAt(Transform target);
        void OnPlaceAt(Transform target);
    }
}
