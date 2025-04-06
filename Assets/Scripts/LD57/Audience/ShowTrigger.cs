using LD57.Aliens;
using UnityEngine;

namespace LD57.Audience {
   public class ShowTrigger : MonoBehaviour {
      [SerializeField] private AudienceController audienceController;

      private void OnTriggerEnter2D(Collider2D other) {
         if (other.GetComponentInParent<AlienController>()) {
            audienceController.StartShow();
            Destroy(gameObject);
         }
      }
   }
}