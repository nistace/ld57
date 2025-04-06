
using UnityEngine;

namespace LD57.Common {
   public class SwimmingLeftRight : MonoBehaviour {
      [SerializeField] private SwimmingLeftRightConfig config;
      [SerializeField] private Rigidbody2D selfBody;
      [SerializeField] private SpriteRenderer selfRenderer;
      [SerializeField] private bool goesRight;

      private void FixedUpdate() {
         selfRenderer.flipX = goesRight;
         selfBody.linearVelocity = Vector3.MoveTowards(selfBody.linearVelocity, config.Speed * (goesRight ? Vector3.right : Vector3.left), config.Acceleration * Time.deltaTime);
      }

      private void OnCollisionEnter2D() => goesRight = !goesRight;
   }
}