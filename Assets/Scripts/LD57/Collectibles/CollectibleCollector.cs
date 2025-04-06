using System.Collections.Generic;
using LD57.Interactables;
using UnityEngine;
using UnityEngine.Events;

namespace LD57.Collectibles {
   public class CollectibleCollector : MonoBehaviour, IInteractable {
      private static readonly int suckAnimParam = Animator.StringToHash("Suck");
      [SerializeField] private Transform interactionPoint;
      [SerializeField] private Animator animator;
      [SerializeField] private float collectibleMoveSpeed = 2;
      [SerializeField] private float collectibleScaleSpeed = 2;

      public string ActionDisplayInfo => "Collect";
      private readonly List<Collectible> collectiblesBeingCollected = new List<Collectible>();

      public UnityEvent<Collectible> OnCollected { get; } = new UnityEvent<Collectible>();

      public Vector3 GetInteractionPoint(Vector3 interactionOrigin) => interactionPoint.position;

      public void Interact(IInteractor interactor) {
         if (interactor.PickedObject is not Collectible collectible) return;

         interactor.UnsetPickedObject(collectible);
         collectible.transform.SetParent(transform);
         collectible.DisableOnCollected();

         collectiblesBeingCollected.Add(collectible);
         animator.SetTrigger(suckAnimParam);
      }

      public float GetInteractionPriority(IInteractor interactor) {
         if (interactor.PickedObject is not Collectible) return -1;

         return 1;
      }

      private void Update() {
         for (var i = 0; i < collectiblesBeingCollected.Count; ++i) {
            var collectible = collectiblesBeingCollected[i];
            collectible.transform.localPosition = Vector3.MoveTowards(collectible.transform.localPosition, Vector3.zero, collectibleMoveSpeed * Time.deltaTime);
            collectible.transform.localScale = Vector3.MoveTowards(collectible.transform.localScale, Vector3.zero, collectibleScaleSpeed * Time.deltaTime);

            if (collectible.transform.localPosition == Vector3.zero) {
               collectiblesBeingCollected.RemoveAt(i);
               collectible.gameObject.SetActive(false);
               OnCollected.Invoke(collectible);
               i--;
            }
         }
      }
   }
}