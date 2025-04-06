using LD57.Interactables;
using UnityEngine;

namespace LD57.Vessels.Anchors {
   public class StaticAnchorAttach : MonoBehaviour, IInteractable, IAnchorAttachable {
      [SerializeField] private Collider2D anchorableCollider;

      public Transform AnchorAttachPointReference => transform;
      public IAnchorAttachable.WinchEffect EffectOnWinchActivation => IAnchorAttachable.WinchEffect.MoveAnchorOwner;

      public void Move(Vector2 force) { }

      public string ActionDisplayInfo => "Attach anchor";
      public InteractionType InteractionType => InteractionType.Tap;

      public Vector3 GetInteractionPoint(Vector3 interactingActor) => anchorableCollider.ClosestPoint(interactingActor);
      public bool CanInteractWhenInteractingWith(IPickable heldPickable) => heldPickable is Anchor;

      public void Interact(IInteractor interactor) {
         if (interactor.PickedObject is not Anchor anchor) return;

         var targetPosition = GetInteractionPoint(anchor.transform.position);
         anchor.AttachTo(this, transform.worldToLocalMatrix.MultiplyPoint(targetPosition), anchor.transform.position - targetPosition);
         interactor.UnsetPickedObject(anchor);
      }

      public float GetInteractionPriority(IInteractor interactor) {
         if (interactor.PickedObject is not Anchor) return -1;

         return 1;
      }
   }
}