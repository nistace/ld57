using System.Collections.Generic;
using System.Linq;
using LD57.Collectibles;
using UnityEngine;
using UnityEngine.Events;

namespace LD57.Audience {
   public class AudienceController : MonoBehaviour {
      [SerializeField] private AudienceConfig config;
      [SerializeField] private CollectibleCollector collectibleCollector;
      [SerializeField] private PointOfInterestFinder pointOfInterestFinder;

      public bool ShowRunning { get; private set; }
      private float ShowElapsedTime { get; set; }
      private float ShowTimeProgressRatio => Mathf.Clamp01(ShowElapsedTime / config.ShowDuration);
      public float ShowRemainingTimeRatio => 1 - ShowTimeProgressRatio;
      public bool RanOutOfTime => Mathf.Approximately(ShowRemainingTimeRatio, 0);
      private readonly HashSet<Collectible> collectedCollectibles = new HashSet<Collectible>();

      private float CurrentInterest { get; set; }
      public bool IsInterestFullLost => Mathf.Approximately(CurrentInterestRatio, 0);
      public int Score { get; private set; }
      public float CurrentInterestRatio => Mathf.Clamp01(CurrentInterest / config.MaxInterest);

      public AudienceEmote CurrentEmote =>
         OverridingEmote && ShowElapsedTime < OverridingEmoteStartTime + config.OverridingEmotesDuration ? OverridingEmote : config.GetInterestEmote(CurrentInterestRatio);

      private AudienceEmote OverridingEmote { get; set; }
      private float OverridingEmoteStartTime { get; set; }

      private List<AudienceHighlight> Highlights { get; } = new List<AudienceHighlight>();

      public UnityEvent OnShowStarted { get; } = new UnityEvent();
      public UnityEvent OnShowEnded { get; } = new UnityEvent();
      public UnityEvent OnInterestFullyLost { get; } = new UnityEvent();
      public UnityEvent OnShowTimedOut { get; } = new UnityEvent();
      public UnityEvent<int> OnScoreChanged { get; } = new UnityEvent<int>();
      public UnityEvent<AudienceEmote> OnOverridingEmoteSet { get; } = new UnityEvent<AudienceEmote>();

      public void Start() {
         ResetAllParameters();
         collectibleCollector.OnCollected.AddListener(HandleCollectorCollected);
         pointOfInterestFinder.OnPointOfInterestFound.AddListener(HandlePointOfInterestFound);
      }

      private void OnDestroy() {
         collectibleCollector.OnCollected.RemoveListener(HandleCollectorCollected);
         pointOfInterestFinder.OnPointOfInterestFound.RemoveListener(HandlePointOfInterestFound);
      }

      public bool TryGetBestHighlight(out AudienceHighlight audienceHighlight) {
         audienceHighlight = Highlights.OrderByDescending(t => t.Score).Take(5).OrderBy(_ => Random.value).FirstOrDefault();
         return audienceHighlight != null;
      }

      private void HandleCollectorCollected(Collectible collectedCollectible) {
         var collectibleType = collectedCollectible.Type;
         var timesCollected = collectedCollectibles.Count(t => t.Type == collectibleType);
         var collectibleReaction = config.GetCollectibleReaction(collectibleType);

         collectedCollectibles.Add(collectedCollectible);

         OverridingEmote = collectibleReaction.GetEmote(timesCollected == 0);
         OverridingEmoteStartTime = ShowElapsedTime;
         OnOverridingEmoteSet.Invoke(OverridingEmote);

         ChangeInterest(Mathf.CeilToInt(collectibleReaction.GetInterestCoefficient(timesCollected) * collectibleType.CollectionInterest));

         Score += collectibleType.CollectionScore;
         OnScoreChanged.Invoke(Score);

         Highlights.Add(new AudienceHighlight($"found {collectedCollectible.Type.ADisplayName}", collectibleType.CollectionScore));
      }

      private void HandlePointOfInterestFound(IPointOfInterest pointOfInterest) {
         OverridingEmote = config.PointOfInterestReaction;
         OverridingEmoteStartTime = ShowElapsedTime;
         OnOverridingEmoteSet.Invoke(OverridingEmote);

         ChangeInterest(pointOfInterest.Interest);

         Score += pointOfInterest.FindingScore;
         OnScoreChanged.Invoke(Score);

         Highlights.Add(new AudienceHighlight($"brought the cameras close to {pointOfInterest.DisplayName}", pointOfInterest.FindingScore));
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