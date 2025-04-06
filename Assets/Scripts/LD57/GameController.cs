using System.Collections;
using LD57.Aliens;
using LD57.Audience;
using LD57.Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD57 {
   public class GameController : MonoBehaviour {
      [SerializeField] private AlienController alienController;
      [SerializeField] private TutorialController tutorial;
      [SerializeField] private AudienceController audienceController;

      private void Start() {
         alienController.SetControlled(false);
         tutorial.gameObject.SetActive(false);
         audienceController.OnInterestFullyLost.AddListener(HandleInterestFullyLost);
         audienceController.OnShowTimedOut.AddListener(HandleShowTimedOut);

         StartCoroutine(PlayIntro());
      }

      private void HandleShowTimedOut() => EndGame();

      private void HandleInterestFullyLost() => EndGame();

      private void OnDestroy() {
         audienceController.OnInterestFullyLost.RemoveListener(HandleInterestFullyLost);
         audienceController.OnShowTimedOut.RemoveListener(HandleShowTimedOut);
      }

      private IEnumerator PlayIntro() {
         yield return new WaitForSeconds(1);
         alienController.SetControlled(true);
         tutorial.gameObject.SetActive(true);
      }

      private void EndGame() {
         alienController.SetControlled(false);
         SceneManager.LoadScene(0);
      }
   }
}