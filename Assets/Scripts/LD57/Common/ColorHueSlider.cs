using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LD57.Common {
   [RequireComponent(typeof(Slider))]
   public class ColorHueSlider : MonoBehaviour {
      [SerializeField] private Slider slider;
      [SerializeField] private Color outputColor;
      [SerializeField] private Image previewImage;

      public UnityEvent<Color> OnValueChanged { get; } = new UnityEvent<Color>();

      private void Reset() {
         slider = GetComponent<Slider>();
         slider.minValue = 0;
         slider.maxValue = 1;
         SetColor(outputColor);
      }

      public void SetColor(Color color) {
         outputColor = color;
         Color.RGBToHSV(outputColor, out var hue, out _, out _);
         slider.value = hue;
         previewImage.color = outputColor;
      }

      private void Start() {
         SetColor(outputColor);
         slider.onValueChanged.AddListener(HandleValueChanged);
      }

      private void OnDestroy() {
         slider.onValueChanged.RemoveListener(HandleValueChanged);
      }

      private void HandleValueChanged(float value) {
         Color.RGBToHSV(outputColor, out _, out var saturation, out var colorValue);
         outputColor = Color.HSVToRGB(value, saturation, colorValue);
         previewImage.color = outputColor;
         OnValueChanged?.Invoke(outputColor);
      }

      private void OnValidate() {
         Color.RGBToHSV(outputColor, out var hue, out _, out _);
         slider.value = hue;
         if (previewImage) previewImage.color = outputColor;
      }
   }
}