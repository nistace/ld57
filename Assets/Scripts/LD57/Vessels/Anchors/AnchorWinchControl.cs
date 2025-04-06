using System;
using LD57.Common;
using LD57.Interactables;
using UnityEngine;

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

      public string ActionDisplayInfo => "Activate Anchor Winch";
      public InteractionType InteractionType => InteractionType.Hold;
      public Vector3 GetInteractionPoint(Vector3 interactionOrigin) => interactionPoint.position;

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
      }

      public float GetInteractionPriority(IInteractor interactor) {
         if (interactor.PickedObject is not null) return -1;
         return 2;
      }

      private void FixedUpdate() {
         if (anchorWinch.IsAnchorInPlace) return;
         if (anchorWinch.Anchor.Attached) return;

         anchorWinch.Anchor.transform.position = Vector3.MoveTowards(anchorWinch.Anchor.transform.position, anchorWinch.AnchorRope.GetRopeOneBeforeEnd(), Time.deltaTime * autoRetractSpeed);
         if (Vector3.SqrMagnitude(anchorWinch.Anchor.transform.position - anchorWinch.transform.position) < .05f) {
            anchorWinch.AttachAnchor();
         }
      }

      private void LateUpdate() {
         if (attachedToRope.TryGetPointOnRopeFromEnd(offsetWithEndOfRope, out var point, out var up)) {
            transform.position = point;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, up);

            objectRoot.gameObject.SetActive(true);
         }
         else {
            objectRoot.gameObject.SetActive(false);
         }
      }
   }
}