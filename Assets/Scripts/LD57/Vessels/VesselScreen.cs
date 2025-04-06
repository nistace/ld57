using LD57.Audience;
using UnityEngine;

namespace LD57.Vessels {
   public class VesselScreen : MonoBehaviour {
      [SerializeField] private AudienceController audienceController;

      [SerializeField] private SpriteRenderer timeBar;
      [SerializeField] private float timeBarMaxHeight = .625f;
      [SerializeField] private SpriteRenderer onFlag;
      [SerializeField] private SpriteRenderer[] digits;
      [SerializeField] private Sprite[] digitSprites;
      [SerializeField] private SpriteRenderer interestBar;
      [SerializeField] private float interestBarMaxWidth = .625f;
      [SerializeField] private SpriteRenderer emote;

      private void Start() {
         SetScoreDisplay(0);
         RefreshStatus();

         audienceController.OnShowStarted.AddListener(HandleShowStarted);
         audienceController.OnShowEnded.AddListener(HandleShowEnded);
         audienceController.OnScoreChanged.AddListener(SetScoreDisplay);
      }

      private void OnDestroy() {
         audienceController.OnShowStarted.RemoveListener(HandleShowStarted);
         audienceController.OnShowEnded.RemoveListener(HandleShowEnded);
         audienceController.OnScoreChanged.RemoveListener(SetScoreDisplay);
      }

      private void HandleShowStarted() => RefreshStatus();
      private void HandleShowEnded() => RefreshStatus();

      private void RefreshStatus() {
         timeBar.enabled = audienceController.ShowRunning;
         onFlag.enabled = audienceController.ShowRunning;
         emote.enabled = audienceController.ShowRunning;
         interestBar.enabled = audienceController.ShowRunning;

         if (audienceController.ShowRunning) {
            Refresh();
         }
      }

      private void Update() {
         if (!audienceController.ShowRunning) return;

         Refresh();
      }

      private void Refresh() {
         timeBar.size = new Vector2(timeBar.size.x, timeBarMaxHeight * audienceController.ShowRemainingTimeRatio);
         interestBar.size = new Vector2(interestBarMaxWidth * audienceController.CurrentInterestRatio, interestBar.size.y);
         emote.sprite = audienceController.CurrentEmote.Sprite;
      }

      private void SetScoreDisplay(int score) {
         if (score >= Mathf.Pow(10, digits.Length)) {
            foreach (var digit in digits) {
               digit.sprite = digitSprites[9];
            }
            return;
         }

         var remainingScore = score;
         for (var index = digits.Length - 1; index >= 0; index--) {
            digits[index].sprite = digitSprites[remainingScore % 10];
            remainingScore /= 10;
         }
      }
   }
}