using UnityEngine;

namespace LD57.Common {
   [CreateAssetMenu]
   public class SwimmingLeftRightConfig : ScriptableObject {
      [SerializeField] private float speed = 1;
      [SerializeField] private float acceleration = 1;

      public float Speed => speed;
      public float Acceleration => acceleration;
   }
}