using UnityEngine;

namespace LD57.Aliens {
   public class AlienInteractionRenderer : MonoBehaviour {
      [SerializeField] private AlienController alien;
      [SerializeField] private SpriteRenderer interactionRenderer;

      private Vector3 position;

      private void Update() {
         interactionRenderer.enabled = alien && alien.InteractionController.TryGetCurrentInteractableInteractionPoint(out position);
         interactionRenderer.transform.position = position;
      }
   }
}