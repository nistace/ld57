using System.Collections.Generic;
using UnityEngine;

namespace LD57 {
   public class GroundChecker : MonoBehaviour {
      public bool IsOnGround => OverlappingObjects.Count > 0;
      private HashSet<GameObject> OverlappingObjects { get; } = new HashSet<GameObject>();

      private void OnTriggerEnter2D(Collider2D other) {
         OverlappingObjects.Add(other.gameObject);
      }

      private void OnTriggerExit2D(Collider2D other) {
         OverlappingObjects.Remove(other.gameObject);
      }

      private void OnValidate() {
         if (TryGetComponent(out Collider2D attachedCollider)) {
            attachedCollider.isTrigger = true;
         }
      }
   }
}