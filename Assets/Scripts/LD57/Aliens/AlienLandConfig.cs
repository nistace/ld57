using UnityEngine;
using UnityEngine.InputSystem;

namespace LD57.Aliens {
   [CreateAssetMenu]
   public class AlienLandConfig : ScriptableObject {
      [SerializeField] private InputActionReference walkAction;
      [SerializeField] private float linearDamping = .2f;
      [SerializeField] private float walkSpeed = 2;
      [SerializeField] private float maxRotationPerSecond = 2;

      public InputActionReference WalkAction => walkAction;
      public float WalkSpeed => walkSpeed;
      public float MaxRotationPerSecond => maxRotationPerSecond;
      public float LinearDamping => linearDamping;
   }
}