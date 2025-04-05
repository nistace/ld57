using UnityEngine;

namespace LD57.Aliens {
   public class AlienAnimator : MonoBehaviour {
      [SerializeField] private Animator animator;
      [SerializeField] private AlienController alienController;

      private static readonly int swimmingAnimParam = Animator.StringToHash("Swimming");
      private static readonly int propelPreparationTimeAnimParam = Animator.StringToHash("PropelPreparationTime");
      private static readonly int preparingPropelAnimParam = Animator.StringToHash("PreparingPropel");
      private static readonly int speedXAnimParam = Animator.StringToHash("SpeedX");
      private static readonly int speedYAnimParam = Animator.StringToHash("SpeedY");
      private static readonly int speedAnimParam = Animator.StringToHash("Speed");

      private void Reset() {
         animator = GetComponentInChildren<Animator>();
         alienController = GetComponentInParent<AlienController>();
      }

      private void Update() {
         animator.SetFloat(speedAnimParam, alienController.Velocity.magnitude);
         animator.SetFloat(speedXAnimParam, alienController.Velocity.x);
         animator.SetFloat(speedYAnimParam, alienController.Velocity.y);

         if (alienController.CurrentState is AlienSwimStateController swimStateController) {
            animator.SetBool(swimmingAnimParam, true);
            animator.SetBool(preparingPropelAnimParam, swimStateController.IsPreparingPropel);
            animator.SetFloat(propelPreparationTimeAnimParam, swimStateController.PropelPreparationTime);
         }
         else {
            animator.SetBool(swimmingAnimParam, false);
         }
      }
   }
}