using UnityEngine;

namespace LD57.Audience {
   [CreateAssetMenu]
   public class AudienceEmote : ScriptableObject {
      [SerializeField] private Sprite sprite;

      public Sprite Sprite => sprite;
   }
}