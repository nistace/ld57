using UnityEngine;

namespace LD57.Aliens {
   public class AlienCustomization : MonoBehaviour {
      [SerializeField] private string alienName;
      [SerializeField] private SpriteRenderer bodyRenderer;
      [SerializeField] private SpriteRenderer eyeRenderer;

      public string AlienName {
         get => alienName;
         set => alienName = value;
      }

      public Color BodyColor {
         get => bodyRenderer.color;
         set => bodyRenderer.color = value;
      }

      public float BodyHue {
         get {
            Color.RGBToHSV(bodyRenderer.color, out var h, out _, out _);
            return h;
         }
         set {
            Color.RGBToHSV(bodyRenderer.color, out _, out var s, out var v);
            bodyRenderer.color = Color.HSVToRGB(value, s, v);
         }
      }

      public Color EyeColor {
         get => eyeRenderer.color;
         set => eyeRenderer.color = value;
      }

      public float EyeHue {
         get {
            Color.RGBToHSV(eyeRenderer.color, out var h, out _, out _);
            return h;
         }
         set {
            Color.RGBToHSV(eyeRenderer.color, out _, out var s, out var v);
            eyeRenderer.color = Color.HSVToRGB(value, s, v);
         }
      }

      public void RandomizeName() => alienName =
         $"{(char)('A' + Random.Range(0, 26))}{(char)('A' + Random.Range(0, 26))}{Random.Range(0, 10)}-{(char)('A' + Random.Range(0, 25))}{Random.Range(0, 10)}{Random.Range(0, 10)}";
   }
}