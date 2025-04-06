using LD57.Common;
using UnityEngine;

namespace LD57.Aliens {
   public class AlienLandStateController : MonoBehaviour, IStateController {
      [SerializeField] private AlienLandConfig config;
      [SerializeField] private TriggerChecker groundChecker;

      public float GravityScale => 1;
      public bool OnGround => groundChecker.IsValid;

      public void EnableState() {
         config.WalkAction.action.Enable();
      }

      public void DisableState() {
         config.WalkAction.action.Disable();
      }

      public void Tick(ref Vector2 currentVelocity) {
         TickRotation(currentVelocity);

         var walkInput = config.WalkAction.action.ReadValue<float>();

         currentVelocity = groundChecker.IsValid
            ? EvaluateNewVelocity(currentVelocity, walkInput * config.WalkMaxSpeed, Mathf.Abs(walkInput) < .2f ? config.WalkDeceleration : config.WalkAcceleration)
            : EvaluateNewVelocity(currentVelocity, walkInput * config.FlightMaxSpeed, config.FlightAcceleration);
      }

      private static Vector2 EvaluateNewVelocity(Vector2 currentVelocity, float targetSpeed, float acceleration) {
         if (currentVelocity.x > 0 && currentVelocity.x > targetSpeed) targetSpeed = currentVelocity.x;
         else if (currentVelocity.x < 0 && currentVelocity.x < targetSpeed) targetSpeed = currentVelocity.x;
         var velocityX = Mathf.MoveTowards(currentVelocity.x, targetSpeed, acceleration * Time.deltaTime);

         return new Vector2(velocityX, currentVelocity.y);
      }

      private void TickRotation(Vector2 currentVelocity) {
         if (groundChecker.IsValid) {
            transform.up = Vector3.up;
            return;
         }

         var rotationAngle = -Vector2.SignedAngle(Vector2.up, transform.up);

         var maxRotation = Mathf.Max(0, (1 - currentVelocity.y) * config.MaxRotationPerSecond * Time.deltaTime);
         rotationAngle = Mathf.Clamp(rotationAngle, -maxRotation, maxRotation);

         transform.Rotate(Vector3.forward, rotationAngle);
      }
   }
}