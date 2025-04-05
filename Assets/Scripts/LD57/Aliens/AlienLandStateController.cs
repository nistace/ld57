using UnityEngine;

namespace LD57.Aliens {
   public class AlienLandStateController : MonoBehaviour, IStateController {
      [SerializeField] private AlienLandConfig config;
      [SerializeField] private GroundChecker groundChecker;

      public float GravityScale => 1;

      public void EnableState() {
         config.WalkAction.action.Enable();
      }

      public void DisableState() {
         config.WalkAction.action.Disable();
      }

      public void Tick(ref Vector2 currentVelocity) {
         if (Mathf.Abs(currentVelocity.y) < .1f) {
            transform.up = Vector3.up;
         }

         var walkInput = config.WalkAction.action.ReadValue<float>();

         currentVelocity = groundChecker.IsOnGround
            ? EvaluateNewVelocity(currentVelocity, walkInput * config.WalkMaxSpeed, config.WalkAcceleration)
            : EvaluateNewVelocity(currentVelocity, walkInput * config.FlightMaxSpeed, config.FlightAcceleration);
      }

      private static Vector2 EvaluateNewVelocity(Vector2 currentVelocity, float targetSpeed, float acceleration) {
         if (currentVelocity.x > 0 && currentVelocity.x > targetSpeed) targetSpeed = currentVelocity.x;
         else if (currentVelocity.x < 0 && currentVelocity.x < targetSpeed) targetSpeed = currentVelocity.x;
         var velocityX = Mathf.MoveTowards(currentVelocity.x, targetSpeed, acceleration * Time.deltaTime);

         return new Vector2(velocityX, currentVelocity.y);
      }
   }
}