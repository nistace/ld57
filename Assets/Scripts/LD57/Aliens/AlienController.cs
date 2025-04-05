using LD57.Cameras;
using UnityEngine;

namespace LD57.Aliens {
   [RequireComponent(typeof(AlienLandStateController))]
   [RequireComponent(typeof(AlienSwimStateController))]
   public class AlienController : MonoBehaviour {
      [SerializeField] private Rigidbody2D bodyRigid;
      [SerializeField] private AlienLandStateController landStateController;
      [SerializeField] private AlienSwimStateController swimStateController;
      [SerializeField] private WaterDetector waterDetector;

      public IStateController CurrentState { get; private set; }
      public Vector2 Velocity => bodyRigid.linearVelocity;

      private void Reset() {
         landStateController = GetComponent<AlienLandStateController>();
         swimStateController = GetComponent<AlienSwimStateController>();
      }

      private void Start() {
         ChangeState(landStateController);
      }

      private void Update() {
         if (CurrentState is AlienLandStateController && waterDetector.IsInWater) {
            ChangeState(swimStateController);
         }
         else if (CurrentState is AlienSwimStateController && !waterDetector.IsInWater) {
            ChangeState(landStateController);
         }
      }

      private void FixedUpdate() {
         var velocity = bodyRigid.linearVelocity;
         CurrentState.Tick(ref velocity);
         bodyRigid.linearVelocity = velocity;
      }

      [ContextMenu("Iterate through states")]
      private void IterateStates() {
         ChangeState(CurrentState switch {
            AlienLandStateController => swimStateController,
            AlienSwimStateController => landStateController,
            _ => landStateController
         });
      }

      private void ChangeState(IStateController newState) {
         if (CurrentState == newState) return;

         CurrentState?.DisableState();
         CurrentState = newState;
         CurrentState?.EnableState();
         bodyRigid.gravityScale = CurrentState?.GravityScale ?? 0;
      }

      private void OnDrawGizmos() {
         if (!Application.isPlaying) return;
         if (CurrentState is not AlienSwimStateController alienSwimStateController) return;

         Gizmos.color = Color.red;
         Gizmos.DrawSphere(CameraController.CursorToWorldPoint(alienSwimStateController.Config.OrientAction.action.ReadValue<Vector2>()), .5f);
      }
   }
}