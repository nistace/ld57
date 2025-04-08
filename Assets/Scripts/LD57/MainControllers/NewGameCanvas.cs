using LD57.Aliens;
using LD57.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LD57.MainControllers {
   public class NewGameCanvas : MonoBehaviour {
      [SerializeField] private AlienCustomization alien;
      [SerializeField] private TMP_InputField nameInput;
      [SerializeField] private ColorHueSlider bodySlider;
      [SerializeField] private ColorHueSlider eyeSlider;
      [SerializeField] private Button playButton;

      public UnityEvent OnPlayClicked => playButton.onClick;

      private void Start() {
         alien.RandomizeName();

         nameInput.text = string.Empty;
         bodySlider.SetColor(alien.BodyColor);
         eyeSlider.SetColor(alien.EyeColor);

         nameInput.onValueChanged.AddListener(HandleNameChanged);
         bodySlider.OnValueChanged.AddListener(HandleBodyValueChanged);
         eyeSlider.OnValueChanged.AddListener(HandleEyeValueChanged);
      }

      private void HandleNameChanged(string newName) => alien.AlienName = newName;
      private void HandleBodyValueChanged(Color color) => alien.BodyColor = color;
      private void HandleEyeValueChanged(Color color) => alien.EyeColor = color;
   }
}