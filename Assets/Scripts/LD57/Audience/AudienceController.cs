using System.Collections.Generic;
using System.Linq;
using LD57.Collectibles;
using UnityEngine;
using UnityEngine.Events;

namespace LD57.Audience {
   public class AudienceController : MonoBehaviour {
      [SerializeField] private AudienceConfig config;
      [SerializeField] private CollectibleCollector collectibleCollector;

      public bool ShowRunning { get; set; }
      private float ShowElapsedTime { get; set; }
      private float ShowTimeProgressRatio => Mathf.Clamp01(ShowElapsedTime / config.ShowDuration);
      public float ShowRemainingTimeRatio => 1 - ShowTimeProgressRatio;
      private readonly HashSet<Collectible> collectedCollectibles = new HashSet<Collectible>();

      private float CurrentInterest { get; set; }
      private bool IsInterestFullLost => Mathf.Approximately(CurrentInterestRatio, 0);
      public int Score { get; set; }
      public float CurrentInterestRatio => Mathf.Clamp01(CurrentInterest / config.MaxInterest);

      public AudienceEmote CurrentEmote =>
         OverridingEmote && ShowElapsedTime < OverridingEmoteStartTime + config.OverridingEmotesDuration ? OverridingEmote : config.GetInterestEmote(CurrentInterestRatio);

      private AudienceEmote OverridingEmote { get; set; }
      private float OverridingEmoteStartTime { get; set; }

      public UnityEvent OnShowStarted { get; } = new UnityEvent();
      public UnityEvent OnShowEnded { get; } = new UnityEvent();
      public UnityEvent OnInterestFullyLost { get; } = new UnityEvent();
      public UnityEvent OnShowTimedOut { get; } = new UnityEvent();
      public UnityEvent<int> OnScoreChanged { get; } = new UnityEvent<int>();

      public void Start() {
         ResetAllParameters();
         collectibleCollector.OnCollected.AddListener(HandleCollectorCollected);
      }

      private void OnDestroy() {
         collectibleCollector.OnCollected.RemoveListener(HandleCollectorCollected);
      }

      private void HandleCollectorCollected(Collectible collectedCollectible) {
         var collectibleType = collectedCollectible.Type;
         var timesCollected = collectedCollectibles.Count(t => t.Type == collectibleType);
         var collectibleReaction = config.GetCollectibleReaction(collectibleType);

         collectedCollectibles.Add(collectedCollectible);

         OverridingEmote = collectibleReaction.GetEmote(timesCollected == 0);
         OverridingEmoteStartTime = ShowElapsedTime;

         ChangeInterest(Mathf.CeilToInt(collectibleReaction.GetInterestCoefficient(timesCollected) * collectibleType.CollectionInterest));

         Score += collectibleType.CollectionScore;
         OnScoreChanged.Invoke(Score);
      }

      private void ChangeInterest(float amount) {
         var wasInterestFullyLost = IsInterestFullLost;

         CurrentInterest = Mathf.Clamp(CurrentInterest + amount, 0, config.MaxInterest);

         if (!wasInterestFullyLost && IsInterestFullLost) {
            OnInterestFullyLost.Invoke();
         }
      }

      private void ResetAllParameters() {
         ShowRunning = false;
         CurrentInterest = config.InterestOnStart;
         ShowElapsedTime = 0;
         Score = 0;
      }

      public void StartShow() {
         if (ShowRunning) return;
         ShowRunning = true;
         OnShowStarted.Invoke();
      }

      public void EndShow() {
         if (!ShowRunning) return;
         ShowRunning = false;
         OnShowEnded.Invoke();
      }

      private void Update() {
         if (!ShowRunning) return;

         ShowElapsedTime += Time.deltaTime;

         if (Mathf.Approximately(ShowRemainingTimeRatio, 0)) {
            OnShowTimedOut.Invoke();
         }

         ChangeInterest(-config.AudienceInterestLossOverTime.Evaluate(ShowElapsedTime) * Time.deltaTime);
      }
   }
}