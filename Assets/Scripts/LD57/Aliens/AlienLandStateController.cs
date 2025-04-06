using LD57.Common;
using UnityEngine;

namespace LD57.Aliens {
   public class AlienLandStateController : MonoBehaviour, IStateController {
      [SerializeField] private Rigidbody2D selfBody;
      [SerializeField] private AlienLandConfig config;
      [SerializeField] private TriggerChecker groundChecker;

      public float GravityScale => 1;
      public bool OnGround => groundChecker.IsValid;

      private void Awake() {
         DisableState();
      }

      public void EnableState() {
         selfBody.linearDamping = config.LinearDamping;
         config.WalkAction.action.Enable();
      }

      public void DisableState() {
         config.WalkAction.action.Disable();
      }

      public void Tick() {
         TickRotation();

         if (groundChecker.IsValid) {
            selfBody.linearVelocity = new Vector2(config.WalkAction.action.ReadValue<float>() * config.WalkSpeed, selfBody.linearVelocity.y);
         }
      }

      private void TickRotation() {
         if (groundChecker.IsValid) {
            transform.up = Vector3.up;
            return;
         }

         var rotationAngle = -Vector2.SignedAngle(Vector2.up, transform.up);

         var maxRotation = Mathf.Max(0, (1 - selfBody.linearVelocity.y) * config.MaxRotationPerSecond * Time.deltaTime);
         rotationAngle = Mathf.Clamp(rotationAngle, -maxRotation, maxRotation);

         transform.Rotate(Vector3.forward, rotationAngle);
      }
   }
}