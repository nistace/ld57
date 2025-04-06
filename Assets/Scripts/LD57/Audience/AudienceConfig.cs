using System;
using System.Linq;
using LD57.Collectibles;
using UnityEngine;

namespace LD57.Audience {
   [CreateAssetMenu]
   public class AudienceConfig : ScriptableObject {
      [SerializeField] private float interestOnStart = 70;
      [SerializeField] private float maxInterest = 100;
      [SerializeField] private AnimationCurve audienceInterestLossOverTime = AnimationCurve.Constant(0, 1, 1);
      [SerializeField] private float showDuration = 5 * 60;
      [SerializeField] private float overridingEmotesDuration = 1;
      [SerializeField] private InterestEmote[] interestEmotes;
      [SerializeField] private InterestEmote defaultInterestEmote;
      [SerializeField] private CollectibleReaction[] collectibleReactions;
      [SerializeField] private CollectibleReaction defaultCollectibleReaction;

      public float InterestOnStart => interestOnStart;
      public float MaxInterest => maxInterest;
      public AnimationCurve AudienceInterestLossOverTime => audienceInterestLossOverTime;
      public float OverridingEmotesDuration => overridingEmotesDuration;
      public float ShowDuration => showDuration;

      public AudienceEmote GetInterestEmote(float interestRatio) {
         foreach (var emote in interestEmotes) {
            if (interestRatio >= emote.AboveRatio) return emote.Emote;
         }
         return defaultInterestEmote.Emote;
      }

      public CollectibleReaction GetCollectibleReaction(CollectibleType type) => collectibleReactions.FirstOrDefault(t => t.IsReactionTo(type)) ?? defaultCollectibleReaction;

      private void OnValidate() {
         interestEmotes = interestEmotes.OrderByDescending(t => t.AboveRatio).ToArray();
      }

      [Serializable]
      public class InterestEmote {
         [SerializeField] private AudienceEmote emote;
         [SerializeField] private float aboveRatio;

         public AudienceEmote Emote => emote;
         public float AboveRatio => aboveRatio;
      }

      [Serializable]
      public class CollectibleReaction {
         [SerializeField] private CollectibleType[] collectibleTypes;
         [SerializeField] private AudienceEmote firstTimeEmote;
         [SerializeField] private AudienceEmote defaultEmote;
         [SerializeField] private float interestCoefficientPerTimesCollected = .5f;

         public bool IsReactionTo(CollectibleType type) => collectibleTypes.Contains(type);

         public AudienceEmote GetEmote(bool firstTime) => firstTime ? firstTimeEmote : defaultEmote;

         public float GetInterestCoefficient(int timesCollected) => timesCollected <= 0 ? 1 : Mathf.Pow(interestCoefficientPerTimesCollected, timesCollected);
      }
   }
}