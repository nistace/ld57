using LD57.Cameras;
using UnityEngine;

namespace LD57.Aliens {
   [RequireComponent(typeof(AlienLandStateController))]
   [RequireComponent(typeof(AlienSwimStateController))]
   public class AlienController : MonoBehaviour {
      [SerializeField] private Rigidbody2D bodyRigid;
      [SerializeField] private AlienLandStateController landStateController;
      [SerializeField] private AlienSwimStateController swimStateController;
      [SerializeField] private AlienInteractionController interactionController;
      [SerializeField] private WaterDetector waterDetector;

      public IStateController CurrentState { get; private set; }
      public Vector2 Velocity => bodyRigid.linearVelocity;
      public AlienInteractionController InteractionController => interactionController;

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
         if (CurrentState == null) return;
         interactionController.Tick();
         CurrentState.Tick();
      }

      [ContextMenu("Iterate through states")]
      private void IterateStates() {
         ChangeState(CurrentState switch {
            AlienLandStateController => swimStateController,
            AlienSwimStateController => landStateController,
            _ => landStateController
         });
      }

      public void SetControlled(bool controlled) {
         ChangeState(controlled ? landStateController : default);
         interactionController.SetEnabled(controlled);
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
         if (CurrentState is AlienSwimStateController alienSwimStateController) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(CameraController.CursorToWorldPoint(alienSwimStateController.Config.OrientAction.action.ReadValue<Vector2>()), .5f);
         }
         if (interactionController.TryGetCurrentInteractableInteractionPoint(out var point)) {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(point, .5f);
         }
      }
   }
}