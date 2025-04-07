using System.Linq;
using LD57.Aliens;
using LD57.Audience;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;

namespace LD57.MainControllers {
   public class EndGameCanvas : MonoBehaviour {
      [SerializeField] private AudienceController audienceController;
      [SerializeField] private AlienCustomization alienCustomization;
      [SerializeField] private string[] interestLostLines = { "Viewer metrics have flatlined. The show is over." };
      [SerializeField] private string[] timesUpLines = { "Time's up! The episode ends here..." };
      [SerializeField] private string[] viewerIntroductions = { "A viewer said about this episode:", "Incoming transmission from a fan:" };
      [SerializeField] private string[] viewerComments;
      [SerializeField] private TMP_Text text;
      [SerializeField] private Button newGameButton;
      [SerializeField] private Button quitButton;
      [SerializeField] private ContentSizeFitter contentSizeFitter;

      public UnityEvent OnNewGameClicked => newGameButton.onClick;
      public UnityEvent OnQuitClicked => quitButton.onClick;

      public void RefreshMessage(int[] leaderboardScores) {
         text.text = "";

         text.text += audienceController.IsInterestFullLost ? $"{interestLostLines.RandomOrDefault()}<br><br>" :
            audienceController.RanOutOfTime ? $"{timesUpLines.RandomOrDefault()}<br><br>" : string.Empty;

         if (audienceController.TryGetBestHighlight(out var highlight)) {
            text.text += $"{viewerIntroductions.Random()}<br>";
            text.text += $"\"{viewerComments.Random().Replace("{alien}", alienCustomization.AlienName).Replace("{acted}", highlight.Action)}\"<br><br>";
         }

         text.text += $"This episode ranks #{leaderboardScores.Count(t => t > audienceController.Score) + 1} in this season!";
      }

      private void Update() {
         contentSizeFitter.enabled = !contentSizeFitter.enabled;
      }

      public void SetButtonsVisible(bool visible) {
         newGameButton.gameObject.SetActive(visible);
         quitButton.gameObject.SetActive(visible);
      }
      
   }
}