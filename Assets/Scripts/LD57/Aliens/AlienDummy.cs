using LD57.Audience;
using UnityEngine;

namespace LD57.Aliens {
   public class AlienDummy : MonoBehaviour, IPointOfInterest {
      private static readonly int swimmingAnimParam = Animator.StringToHash("Swimming");
      [SerializeField] private AlienCustomization customization;
      [SerializeField] private Animator animator;

      public void Setup(string alienName, float bodyHue, float eyeHue, Vector2 position, int score) {
         transform.position = position;
         customization.AlienName = alienName;
         customization.BodyHue = bodyHue;
         customization.EyeHue = eyeHue;
         animator.SetBool(swimmingAnimParam, transform.position.y < 0);
         FindingScore = score;
      }

      public bool Detected { get; set; }
      public string DisplayName => $"the star of the show {customization.AlienName}";
      public int Interest => FindingScore / 10;
      public int FindingScore { get; private set; }
   }
}