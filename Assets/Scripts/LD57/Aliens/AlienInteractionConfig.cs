using UnityEngine;
using UnityEngine.InputSystem;

namespace LD57.Aliens {
   [CreateAssetMenu]
   public class AlienInteractionConfig : ScriptableObject {
      [SerializeField] private InputActionReference interactionAction;

      public InputActionReference InteractionAction => interactionAction;
   }
}