using System.Collections;
using System.Linq;
using LD57.Aliens;
using LD57.Audience;
using LD57.Cameras;
using LD57.Tutorial;
using LD57.Web;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD57.MainControllers {
   public class GameController : MonoBehaviour {
      [SerializeField] private CameraController cameraController;
      [SerializeField] private AlienController alienController;
      [SerializeField] private AlienCustomization alienCustomization;
      [SerializeField] private NewGameCanvas newGameCanvas;
      [SerializeField] private EndGameCanvas endGameCanvas;
      [SerializeField] private Leaderboard[] leaderboards;
      [SerializeField] private TutorialController tutorial;
      [SerializeField] private AudienceController audienceController;
      [SerializeField] private AlienDummy alienDummyPrefab;

      private LeaderboardWebRequest.GetResult.Entry[] leaderboardEntries = { };

      private void Start() {
         cameraController.ActivateCamera(CameraController.CameraTarget.Alien);

         alienController.SetControlled(false);
         tutorial.gameObject.SetActive(false);
         newGameCanvas.gameObject.SetActive(false);
         endGameCanvas.gameObject.SetActive(false);

         audienceController.OnInterestFullyLost.AddListener(HandleInterestFullyLost);
         audienceController.OnShowTimedOut.AddListener(HandleShowTimedOut);
         newGameCanvas.OnPlayClicked.AddListener(HandleNewGamePlayClicked);
         endGameCanvas.OnNewGameClicked.AddListener(HandleNewGameClicked);
         endGameCanvas.OnQuitClicked.AddListener(HandleQuitClicked);

         StartCoroutine(PlayIntro());
      }

      private void OnDestroy() {
         audienceController.OnInterestFullyLost.RemoveListener(HandleInterestFullyLost);
         audienceController.OnShowTimedOut.RemoveListener(HandleShowTimedOut);
         newGameCanvas.OnPlayClicked.RemoveListener(HandleNewGamePlayClicked);
         endGameCanvas.OnNewGameClicked.RemoveListener(HandleNewGameClicked);
         endGameCanvas.OnQuitClicked.RemoveListener(HandleQuitClicked);
      }

      private static void HandleQuitClicked() => Application.Quit();
      private static void HandleNewGameClicked() => SceneManager.LoadScene(0);

      private void HandleShowTimedOut() => EndGame();
      private void HandleInterestFullyLost() => EndGame();

      private void HandleNewGamePlayClicked() {
         newGameCanvas.gameObject.SetActive(false);
         if (string.IsNullOrEmpty(alienCustomization.AlienName)) {
            alienCustomization.RandomizeName();
         }

         alienController.SetControlled(true);
         tutorial.gameObject.SetActive(true);
      }

      private IEnumerator PlayIntro() {
         yield return StartCoroutine(LeaderboardWebRequest.Get(new LeaderboardWebRequest.GetData(), HandleLeaderboardReceived, HandleLeaderboardFailed));
         newGameCanvas.gameObject.SetActive(true);
      }

      private void HandleLeaderboardFailed() {
         foreach (var leaderboard in leaderboards) {
            leaderboard.Initialize(null);
         }
         Debug.Log("Failed to get leaderboard");
      }

      private void HandleLeaderboardReceived(LeaderboardWebRequest.GetResult requestResult) {
         leaderboardEntries = requestResult.entries;
         foreach (var dummy in leaderboardEntries.OrderByDescending(t => t.score).Take(10)) {
            var dummyInstance = Instantiate(alienDummyPrefab);
            dummyInstance.Setup(dummy.alienName, dummy.NormalizedBodyHue, dummy.NormalizedEyeHue, dummy.DeathPosition, Mathf.CeilToInt(dummy.score * .1f));
         }

         Debug.LogError("Leaderboard");

         foreach (var leaderboard in leaderboards) {
            leaderboard.Initialize(leaderboardEntries);
         }
      }

      private void EndGame() {
         alienController.SetControlled(false);
         audienceController.EndShow();
         endGameCanvas.gameObject.SetActive(true);
         endGameCanvas.SetButtonsVisible(false);
         if (leaderboardEntries != null) {
            leaderboardEntries = leaderboardEntries.Append(new LeaderboardWebRequest.GetResult.Entry { alienName = alienCustomization.AlienName, score = audienceController.Score }).ToArray();
            foreach (var leaderboard in leaderboards) {
               leaderboard.Initialize(leaderboardEntries);
            }
         }
         endGameCanvas.RefreshMessage(leaderboardEntries.Select(t => t.score).ToArray());
         cameraController.ActivateCamera(CameraController.CameraTarget.Vessel);
         StartCoroutine(LeaderboardWebRequest.Post(GenerateLeaderboardPostData(), HandleLeaderboardPostCallback, HandleLeaderboardPostFailed));
      }

      private void HandleLeaderboardPostFailed() {
         Debug.Log("Failed to post leaderboard");
         endGameCanvas.SetButtonsVisible(true);
      }

      private LeaderboardWebRequest.PostData GenerateLeaderboardPostData() {
         var leaderboardData = new LeaderboardWebRequest.PostData { alienName = alienCustomization.AlienName, score = audienceController.Score };
         Color.RGBToHSV(alienCustomization.BodyColor, out var bodyHue, out _, out _);
         leaderboardData.NormalizedBodyHue = bodyHue;
         Color.RGBToHSV(alienCustomization.EyeColor, out var eyeHue, out _, out _);
         leaderboardData.NormalizedEyeHue = eyeHue;
         leaderboardData.DeathPosition = alienController.transform.position;
         return leaderboardData;
      }

      private void HandleLeaderboardPostCallback(LeaderboardWebRequest.PostResult result) {
         if (!result.success) Debug.Log("Leaderboard post not successful");
         endGameCanvas.SetButtonsVisible(true);
      }
   }
}