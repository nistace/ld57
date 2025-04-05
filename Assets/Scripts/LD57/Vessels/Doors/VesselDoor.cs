using UnityEngine;

namespace LD57.Vessels.Doors {
   public class VesselDoor : MonoBehaviour {
      [SerializeField] private TriggerChecker detectionChecker;
      [SerializeField] private Collider2D doorCollider;
      [SerializeField] private Animator animator;

      private static readonly int openAnimParam = Animator.StringToHash("Open");

      private void Start() {
         detectionChecker.OnValidStarted.AddListener(HandleValidStarted);
         detectionChecker.OnValidEnded.AddListener(HandleValidEnded);
      }

      private void OnDestroy() {
         detectionChecker.OnValidStarted.RemoveListener(HandleValidStarted);
         detectionChecker.OnValidEnded.RemoveListener(HandleValidEnded);
      }

      private void HandleValidEnded() {
         doorCollider.enabled = true;
         animator.SetBool(openAnimParam, false);
      }

      private void HandleValidStarted() {
         doorCollider.enabled = false;
         animator.SetBool(openAnimParam, true);
      }
   }
}