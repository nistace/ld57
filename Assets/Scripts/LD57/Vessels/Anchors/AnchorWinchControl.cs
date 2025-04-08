using System;
using LD57.Common;
using LD57.Interactables;
using UnityEngine;
using UnityEngine.Events;

namespace LD57.Vessels.Anchors {
   public class AnchorWinchControl : MonoBehaviour, IInteractable {
      [SerializeField] private Rope attachedToRope;
      [SerializeField] private float offsetWithEndOfRope = 2;
      [SerializeField] private GameObject objectRoot;

      [SerializeField] private Transform interactionPoint;
      [SerializeField] private AnchorWinch anchorWinch;
      [SerializeField] private Rigidbody2D vesselBody;
      [SerializeField] private float winchForce = 200;
      [SerializeField] private float autoRetractSpeed = 1;

      private bool ActivePreviousFrame { get; set; }
      private bool ActiveThisFrame { get; set; }
      public string ActionDisplayInfo => "Activate Anchor Winch";
      public InteractionType InteractionType => InteractionType.Hold;
      public Vector3 GetInteractionPoint(Vector3 interactionOrigin) => interactionPoint.position;

      public UnityEvent OnActivated { get; } = new UnityEvent();
      public UnityEvent OnDeactivated { get; } = new UnityEvent();

      public void Interact(IInteractor interactor) {
         if (anchorWinch.IsAnchorInPlace) return;
         if (!anchorWinch.Anchor.Attached) return;

         switch (anchorWinch.Anchor.AttachedTo.EffectOnWinchActivation) {
            case IAnchorAttachable.WinchEffect.None:
               break;
            case IAnchorAttachable.WinchEffect.MoveAnchorOwner:
               vesselBody.AddForceAtPosition(winchForce * anchorWinch.AnchorRope.GetRopeStartDirection(), anchorWinch.transform.position);
               break;
            case IAnchorAttachable.WinchEffect.MoveAnchor:
               anchorWinch.Anchor.AttachedTo.Move(Time.deltaTime * winchForce * anchorWinch.AnchorRope.GetRopeEndDirection());
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }
         ActiveThisFrame = true;
      }

      public float GetInteractionPriority(IInteractor interactor) {
         if (interactor.PickedObject is not null) return -1;
         return 2;
      }

      private void FixedUpdate() {
         if (attachedToRope.TryGetPointOnRopeFromEnd(offsetWithEndOfRope, out var point, out var up)) {
            transform.position = point;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, up);

            objectRoot.gameObject.SetActive(true);
         }
         else {
            objectRoot.gameObject.SetActive(false);
         }

         if (!anchorWinch.IsAnchorInPlace && !anchorWinch.Anchor.Attached) {
            anchorWinch.Anchor.transform.position = Vector3.MoveTowards(anchorWinch.Anchor.transform.position, anchorWinch.AnchorRope.GetRopeOneBeforeEnd(), Time.deltaTime * autoRetractSpeed);
            ActiveThisFrame = true;
            if (Vector3.SqrMagnitude(anchorWinch.Anchor.transform.position - anchorWinch.transform.position) < .05f) {
               anchorWinch.AttachAnchor();
            }
         }

         if (ActivePreviousFrame && !ActiveThisFrame) {
            OnDeactivated.Invoke();
         }
         else if (!ActivePreviousFrame && ActiveThisFrame) {
            OnActivated.Invoke();
         }

         ActivePreviousFrame = ActiveThisFrame;
         ActiveThisFrame = false;
      }
   }
}