using UnityEngine;
using UnityEngine.InputSystem;

namespace LD57.Aliens {
   [CreateAssetMenu]
   public class AlienLandConfig : ScriptableObject {
      [SerializeField] private InputActionReference walkAction;
      [SerializeField] private float walkMaxSpeed = 2;
      [SerializeField] private float walkAcceleration = 2;
      [SerializeField] private float walkDeceleration = 300;
      [SerializeField] private float flightMaxSpeed = .3f;
      [SerializeField] private float flightAcceleration = .3f;
      [SerializeField] private float maxRotationPerSecond = 2;

      public InputActionReference WalkAction => walkAction;
      public float WalkMaxSpeed => walkMaxSpeed;
      public float WalkAcceleration => walkAcceleration;
      public float WalkDeceleration => walkDeceleration;
      public float FlightMaxSpeed => flightMaxSpeed;
      public float FlightAcceleration => flightAcceleration;
      public float MaxRotationPerSecond => maxRotationPerSecond;
   }
}