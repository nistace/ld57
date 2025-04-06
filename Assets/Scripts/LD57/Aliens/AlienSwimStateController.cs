using System;
using LD57.Cameras;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LD57.Aliens {
   public class AlienSwimStateController : MonoBehaviour, IStateController {
      [SerializeField] private Rigidbody2D selfBody;
      [SerializeField] private AlienSwimConfig config;
      [SerializeField] private Transform rotationPoint;

      public AlienSwimConfig Config => config;
      public bool IsPreparingPropel { get; private set; }
      public float PropelPreparationTime { get; private set; }
      public float GravityScale => config.GravityScale;

      private void Awake() {
         DisableState();
      }

      public void EnableState() {
         selfBody.linearDamping = config.DecelerationRate;
         config.PropelAction.action.Enable();
         config.OrientAction.action.Enable();
      }

      public void DisableState() {
         config.PropelAction.action.Disable();
         config.OrientAction.action.Disable();
      }

      public void Tick() {
         TickRotation();
         TickVelocity();
      }

      private void TickVelocity() {
         var wasPreparingPropel = IsPreparingPropel;
         IsPreparingPropel = config.PropelAction.action.phase == InputActionPhase.Performed;

         if (IsPreparingPropel) {
            selfBody.linearDamping = config.BrakingDecelerationRate;
            PropelPreparationTime += Time.deltaTime;
         }
         else {
            selfBody.linearDamping = config.DecelerationRate;
            if (wasPreparingPropel) {
               var speed = config.PropelSpeedPerPreparationTime.Evaluate(PropelPreparationTime);

               PropelPreparationTime = 0;

               selfBody.AddForce((Vector2)transform.up * speed, ForceMode2D.Impulse);
            }
         }
      }

      private void TickRotation() {
         var targetAngle = Vector2.SignedAngle(Vector2.up, CameraController.CursorToWorldPoint(config.OrientAction.action.ReadValue<Vector2>()) - (Vector2)rotationPoint.position);
         var currentAngle = Vector2.SignedAngle(Vector2.up, rotationPoint.up);

         var rotationAngle = targetAngle - currentAngle;
         if (rotationAngle > 180) rotationAngle -= 360;
         if (rotationAngle < -180) rotationAngle += 360;

         var maxRotation = config.MaxRotationPerSecond * Time.deltaTime;
         rotationAngle = Mathf.Clamp(rotationAngle, -maxRotation, maxRotation);

         transform.RotateAround(rotationPoint.position, Vector3.forward, rotationAngle);
      }
   }
}