using UnityEngine;

namespace LD57.Audio {
   [RequireComponent(typeof(AudioSource))]
   public abstract class SfxSource : MonoBehaviour {
      private static float Volume { get; set; } = .6f;

      public static void SetVolume(float volume) => Volume = volume;

      [SerializeField] private AudioSource source;

      public AudioSource Source => source;

      protected virtual void Reset() {
         source = GetComponent<AudioSource>();
         source.spatialBlend = 1;
         source.minDistance = 5;
         source.maxDistance = 20;
         source.playOnAwake = false;
      }

      protected void Play(AudioClip clip, float volumeScale = 1) => source.PlayOneShot(clip, Volume * volumeScale);
   }
}