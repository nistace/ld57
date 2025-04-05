using UnityEngine;
using UnityEngine.InputSystem;

namespace LD57.Aliens {
   [CreateAssetMenu]
   public class AlienSwimConfig : ScriptableObject {
      [SerializeField] private InputActionReference propelAction;
      [SerializeField] private InputActionReference orientAction;
      [SerializeField] private AnimationCurve propelSpeedPerPreparationTime = AnimationCurve.Constant(0, 1, 1);
      [SerializeField] private float gravityScale = .05f;
      [SerializeField] private float decelerationRate = .2f;
      [SerializeField] private float brakingDecelerationRate = .5f;
      [SerializeField] private float maxRotationPerSecond = 360;

      public InputActionReference PropelAction => propelAction;
      public InputActionReference OrientAction => orientAction;
      public AnimationCurve PropelSpeedPerPreparationTime => propelSpeedPerPreparationTime;
      public float DecelerationRate => decelerationRate;
      public float BrakingDecelerationRate => brakingDecelerationRate;
      public float MaxRotationPerSecond => maxRotationPerSecond;
      public float GravityScale => gravityScale;
   }
}