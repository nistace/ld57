using LD57.Interactables;
using Unity.Mathematics;
using UnityEngine;

namespace LD57.Vessels.Anchors {
   public class Anchor : MonoBehaviour, IPickable {
      [SerializeField] private Collider2D selfCollider;

      public string ActionDisplayInfo => "Grab Anchor";
      public InteractionType InteractionType => InteractionType.Tap;

      public IAnchorAttachable AttachedTo { get; private set; }
      private Vector3 AttachedRelativePosition { get; set; }
      private Vector3 AttachedRelativeUp { get; set; }

      private bool Attached => AttachedTo != null;

      public Vector3 GetInteractionPoint(Vector3 interactingActor) => transform.position;
      public bool CanInteractWhenInteractingWith(IPickable heldPickable) => heldPickable == null;

      public void Interact(IInteractor interactor) {
         if (interactor is not IAnchorAttachable attachable) return;
         AttachTo(attachable, Vector3.zero, Vector3.up);
         interactor.SetPickedObject(this);
      }

      public float GetInteractionPriority(IInteractor interactor) => interactor.PickedObject == null ? 1 : -1;

      private void Update() {
         RefreshAttachedPosition();
      }

      public void AttachTo(IAnchorAttachable target, Vector3 relativePosition, Vector3 relativeUp) {
         AttachedTo = target;
         AttachedRelativePosition = relativePosition;
         AttachedRelativeUp = relativeUp;
         RefreshAttachedPosition();
      }

      private void RefreshAttachedPosition() {
         if (!Attached) return;
         var targetPosition = AttachedTo.AnchorAttachPointReference.localToWorldMatrix.MultiplyPoint(AttachedRelativePosition);
         if (transform.position == targetPosition) return;
         transform.rotation = quaternion.LookRotation(Vector3.forward, AttachedTo.AnchorAttachPointReference.localToWorldMatrix.MultiplyVector(AttachedRelativeUp));
         transform.position = targetPosition;
      }

      public void Detach() => AttachedTo = null;
   }
}