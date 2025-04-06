using LD57.Common;
using LD57.Interactables;
using UnityEngine;

namespace LD57.Vessels.Anchors {
   public class AnchorWinch : MonoBehaviour, IInteractable, IAnchorAttachable {
      [SerializeField] private Transform interactionPoint;
      [SerializeField] private Transform anchorAnchorAttach;
      [SerializeField] private Anchor anchor;
      [SerializeField] private Rope anchorRope;

      public string ActionDisplayInfo => "Attach Anchor";
      public Rope AnchorRope => anchorRope;
      public Transform AnchorAttachPointReference => anchorAnchorAttach;
      public IAnchorAttachable.WinchEffect EffectOnWinchActivation => IAnchorAttachable.WinchEffect.None;
      public void Move(Vector2 force) { }

      public Anchor Anchor => anchor;
      public bool IsAnchorInPlace => ReferenceEquals(anchor.AttachedTo, this);
      public Vector3 GetInteractionPoint(Vector3 interactionOrigin) => interactionPoint.position;

      private void Start() {
         AttachAnchor();
      }

      public void Interact(IInteractor interactor) {
         if (!ReferenceEquals(interactor.PickedObject, anchor)) return;
         AttachAnchor();
         interactor.UnsetPickedObject(anchor);
      }

      private void AttachAnchor() {
         anchor.AttachTo(this, Vector3.zero, Vector3.up);
         anchor.transform.rotation = anchorAnchorAttach.rotation;
         anchorRope.ResetRope();
      }

      public float GetInteractionPriority(IInteractor interactor) => ReferenceEquals(interactor.PickedObject, anchor) ? 3 : -1;
   }
}