using UnityEngine;
using UnityEngine.InputSystem;

namespace LD57.Aliens {
   [CreateAssetMenu]
   public class AlienInteractionConfig : ScriptableObject {
      [SerializeField] private InputActionReference interactionAction;
      [SerializeField] private InputActionReference dropAction;

      public InputActionReference InteractionAction => interactionAction;
      public InputActionReference DropAction => dropAction;
   }
}