using System.Linq;
using LD57.Interactables;
using LD57.Vessels.Anchors;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LD57.Aliens {
   public class AlienInteractionController : MonoBehaviour, IInteractor, IAnchorAttachable {
      [SerializeField] private AlienInteractionConfig config;
      [SerializeField] private Transform interactionOrigin;
      [SerializeField] private InteractableDetector interactableDetector;
      [SerializeField] private Rigidbody2D selfBody;
      [SerializeField] private float maxInteractionDistance = 10;

      private IInteractable CurrentInteractable { get; set; }
      private bool IsInteracting { get; set; }
      public Transform InteractionOrigin => interactionOrigin;
      public IPickable PickedObject { get; private set; }
      public Transform AnchorAttachPointReference => interactionOrigin;
      public IAnchorAttachable.WinchEffect EffectOnWinchActivation => IAnchorAttachable.WinchEffect.MoveAnchor;

      public void Move(Vector2 force) {
         selfBody.AddForce(force, ForceMode2D.Force);
      }

      public void Tick() {
         CurrentInteractable = EvaluatePreferredInteractable();

         IsInteracting = config.InteractionAction.action.phase == InputActionPhase.Performed;

         if (IsInteracting) {
            CurrentInteractable?.Interact(this);
         }
      }

      private IInteractable EvaluatePreferredInteractable() {
         if (IsInteracting) {
            if (CurrentInteractable == default) return default;
            if (interactableDetector.OverlappingInteractables.Contains(CurrentInteractable)) return CurrentInteractable;
            return default;
         }

         if (!interactableDetector.OverlappingInteractables.Any()) return default;

         var preferredInteractable = interactableDetector.OverlappingInteractables.Select(t => (interactable: t, score: t.GetInteractionPriority(this))).OrderByDescending(t => t.score).First();

         if (preferredInteractable.score < 0) return default;

         return preferredInteractable.interactable;
      }

      public bool TryGetCurrentInteractableInteractionPoint(out Vector3 position) {
         position = default;
         if (CurrentInteractable == null) return false;
         position = CurrentInteractable.GetInteractionPoint(interactionOrigin.position);
         return true;
      }

      public void SetPickedObject(IPickable pickable) => PickedObject = pickable;

      public void UnsetPickedObject(IPickable pickable = default) {
         if (pickable != null && pickable != PickedObject) return;
         PickedObject = default;
      }
   }
}