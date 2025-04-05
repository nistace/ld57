using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LD57 {
   public class TriggerChecker : MonoBehaviour {
      public bool IsValid => OverlappingObjects.Count > 0;
      private HashSet<GameObject> OverlappingObjects { get; } = new HashSet<GameObject>();

      public UnityEvent OnValidStarted { get; } = new UnityEvent();
      public UnityEvent OnValidEnded { get; } = new UnityEvent();

      private void OnTriggerEnter2D(Collider2D other) {
         if (OverlappingObjects.Add(other.gameObject) && OverlappingObjects.Count == 1) {
            OnValidStarted.Invoke();
         }
      }

      private void OnTriggerExit2D(Collider2D other) {
         if (OverlappingObjects.Remove(other.gameObject) && !IsValid) {
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