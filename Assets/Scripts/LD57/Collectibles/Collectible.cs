using System;
using LD57.Interactables;
using UnityEngine;

namespace LD57.Collectibles {
   public class Collectible : MonoBehaviour, IPickable {
      [SerializeField] private Rigidbody2D selfBody;
      [SerializeField] private Collider2D selfCollider;
      [SerializeField] private Transform interactionPoint;
      [SerializeField] private CollectibleType type;
      [SerializeField] private MonoBehaviour[] behavioursToStopOnCollected;

      public string ActionDisplayInfo => $"Pick up {type.DisplayName}";
      public CollectibleType Type => type;

      public Vector3 GetInteractionPoint(Vector3 interactionOrigin) => interactionPoint.position;

      public void Interact(IInteractor interactor) {
         interactor.SetPickedObject(this);
         DisableOnCollected();
         transform.SetParent(interactor.InteractionOrigin);
         transform.localPosition = Vector3.zero;
         transform.localRotation = Quaternion.identity;
      }

      public void DisableOnCollected() {
         selfBody.bodyType = RigidbodyType2D.Kinematic;
         selfCollider.enabled = false;
         foreach (var behaviourToStopOnCollected in behavioursToStopOnCollected) {
            behaviourToStopOnCollected.enabled = false;
         }
      }

      public float GetInteractionPriority(IInteractor interactor) =>
         interactor.PickedObject == null ? 100 - Vector3.SqrMagnitude(interactor.InteractionOrigin.position - interactionPoint.position) : -1;
   }
}