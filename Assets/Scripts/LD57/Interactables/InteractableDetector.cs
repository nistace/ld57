using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LD57.Interactables {
   public class InteractableDetector : MonoBehaviour {
      private readonly HashSet<Collider2D> overlappingColliders = new HashSet<Collider2D>();
      private readonly Dictionary<IInteractable, HashSet<Collider2D>> overlappingInteractables = new Dictionary<IInteractable, HashSet<Collider2D>>();

      public Dictionary<IInteractable, HashSet<Collider2D>>.KeyCollection OverlappingInteractables => overlappingInteractables.Keys;

      public bool IsDetectingAnyInteractable => overlappingInteractables.Count > 0;
      private UnityEvent<IInteractable> OnInteractableDetected { get; } = new UnityEvent<IInteractable>();
      private UnityEvent<IInteractable> OnInteractableLost { get; } = new UnityEvent<IInteractable>();

      private void OnTriggerEnter2D(Collider2D other) {
         if (!overlappingColliders.Add(other)) return;

         var interactable = other.gameObject.GetComponentInParent<IInteractable>();
         if (interactable == null) return;

         var newDetection = false;
         if (!overlappingInteractables.ContainsKey(interactable)) {
            overlappingInteractables.Add(interactable, new HashSet<Collider2D>());
            newDetection = true;
         }

         overlappingInteractables[interactable].Add(other);
         if (newDetection) {
            OnInteractableDetected.Invoke(interactable);
         }
      }

      private void OnTriggerExit2D(Collider2D other) {
         if (!overlappingColliders.Remove(other)) return;

         var interactable = other.gameObject.GetComponentInParent<IInteractable>();
         if (interactable == null) return;

         if (!overlappingInteractables.TryGetValue(interactable, out var collidersForInteractable)) return;
         if (!collidersForInteractable.Remove(other)) return;

         if (collidersForInteractable.Count == 0) {
            overlappingInteractables.Remove(interactable);
            OnInteractableLost.Invoke(interactable);
         }
      }
   }
}