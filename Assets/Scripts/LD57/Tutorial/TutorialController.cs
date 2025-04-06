using System;
using LD57.Common;
using UnityEngine;

namespace LD57.Tutorial {
   public class TutorialController : MonoBehaviour {
      [SerializeField] private TutorialItem[] items;
      [SerializeField] private TriggerChecker endTutorialTrigger;

      private void OnEnable() {
         endTutorialTrigger.OnValidStarted.AddListener(HandleEndTutorialValid);

         foreach (var item in items) {
            item.Enable();
         }
      }

      private void HandleEndTutorialValid() => gameObject.SetActive(false);

      private void OnDisable() {
         endTutorialTrigger.OnValidStarted.RemoveListener(HandleEndTutorialValid);
         foreach (var item in items) {
            item.Disable();
         }
      }

      [Serializable]
      private class TutorialItem {
         [SerializeField] private GameObject tutorial;
         [SerializeField] private TriggerChecker checker;

         public void Enable() {
            Refresh();
            checker.OnValidStarted.AddListener(Refresh);
            checker.OnValidEnded.AddListener(Refresh);
         }

         private void Refresh() => tutorial.SetActive(checker.IsValid);

         public void Disable() {
            checker.OnValidStarted.RemoveListener(Refresh);
            checker.OnValidEnded.RemoveListener(Refresh);
         }
      }
   }
}