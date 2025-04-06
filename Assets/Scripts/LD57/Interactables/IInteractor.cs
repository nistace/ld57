using UnityEngine;

namespace LD57.Interactables {
   public interface IInteractor {
      Transform InteractionOrigin { get; }
      IPickable PickedObject { get; }

      public void SetPickedObject(IPickable pickable);
      public void UnsetPickedObject(IPickable pickable = default);
   }
}