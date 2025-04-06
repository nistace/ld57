using LD57.Vessels.Anchors;
using UnityEngine;

namespace LD57.Vessels {
   public class VesselController : MonoBehaviour {
      [SerializeField] private Anchor anchor;
      [SerializeField] private Rigidbody2D vesselBody;
      [SerializeField] private float resetRotationTorqueForce = 10;

      private void FixedUpdate() {
         var angleWithUp = Vector3.SignedAngle(transform.up, Vector3.up, Vector3.forward);

         vesselBody.AddTorque(angleWithUp * resetRotationTorqueForce);
      }

      private void Update() {
         vesselBody.gravityScale = transform.position.y > 0 ? 1 : 0;
      }
   }
}