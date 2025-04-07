using System.Collections.Generic;
using UnityEngine;

namespace LD57.Audience {
   [CreateAssetMenu]
   public class AudienceEmote : ScriptableObject {
      [SerializeField] private Sprite sprite;
      [SerializeField] private AudioClip[] audioClips;

      public Sprite Sprite => sprite;
      public IReadOnlyList<AudioClip> AudioClips => audioClips;
   }
}