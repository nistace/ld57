using LD57.Aliens;
using UnityEngine;

namespace LD57.Interactables {
   public interface IInteractable {
      public string ActionDisplayInfo { get; }

      public Vector3 GetInteractionPoint(Vector3 interactionOrigin);
      public void Interact(IInteractor interactor);
      public float GetInteractionPriority(IInteractor interactor);
   }
}