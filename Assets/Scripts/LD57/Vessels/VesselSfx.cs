using LD57.Audience;
using LD57.Audio;
using LD57.Collectibles;
using UnityEngine;
using Utils;

namespace LD57.Vessels {
   public class VesselSfx : SfxSource {
      [SerializeField] private AudienceController audienceController;
      [SerializeField] private CollectibleCollector collectibleCollector;
      [SerializeField] private AudioClip[] collectionFxs;

      private void Start() {
         collectibleCollector.OnCollectionStarted.AddListener(HandleCollectionStarted);
         audienceController.OnOverridingEmoteSet.AddListener(HandleOverridingEmoteSet);
      }

      private void OnDestroy() {
         collectibleCollector.OnCollectionStarted.RemoveListener(HandleCollectionStarted);
         audienceController.OnOverridingEmoteSet.RemoveListener(HandleOverridingEmoteSet);
      }

      private void HandleOverridingEmoteSet(AudienceEmote overridingEmote) => Play(overridingEmote.AudioClips.RandomOrDefault());
      private void HandleCollectionStarted(Collectible arg0) => Play(collectionFxs.RandomOrDefault());
   }
}