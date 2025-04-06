using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace LD57.Common {
   public class TriggerChecker : MonoBehaviour {
      private readonly Dictionary<GameObject, HashSet<Collider2D>> overlappingObjects = new Dictionary<GameObject, HashSet<Collider2D>>();

      public bool IsValid => overlappingObjects.Count > 0;

      public GameObject FirstOverlappingObject => overlappingObjects.Keys.FirstOrDefault();

      public UnityEvent OnValidStarted { get; } = new UnityEvent();
      public UnityEvent OnValidEnded { get; } = new UnityEvent();

      private void OnTriggerEnter2D(Collider2D other) {
         var wasValid = IsValid;

         if (!overlappingObjects.ContainsKey(other.gameObject)) {
            overlappingObjects.Add(other.gameObject, new HashSet<Collider2D>());
         }

         overlappingObjects[other.gameObject].Add(other);

         if (!wasValid && IsValid) {
            OnValidStarted.Invoke();
         }
      }

      private void OnTriggerExit2D(Collider2D other) {
         if (!overlappingObjects.TryGetValue(other.gameObject, out var gameObjectColliders)) return;
         if (!gameObjectColliders.Remove(other)) return;
         if (gameObjectColliders.Count > 0) return;

         if (overlappingObjects.Remove(other.gameObject) && !IsValid) {
            OnValidEnded.Invoke();
         }
      }

      private void OnValidate() {
         if (TryGetComponent(out Collider2D attachedCollider)) {
            attachedCollider.isTrigger = true;
         }
      }
   }
}