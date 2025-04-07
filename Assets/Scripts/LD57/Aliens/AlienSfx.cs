using LD57.Audio;
using UnityEngine;
using Utils;

namespace LD57.Aliens {
   public class AlienSfx : SfxSource {
      [SerializeField] private AlienSwimStateController swimStateController;
      [SerializeField] private AudioClip[] footsteps;
      [SerializeField] private float footstepsVolume = 1;
      [SerializeField] private AudioClip[] waterSplashes;
      [SerializeField] private AudioClip[] preparePropels;
      [SerializeField] private AudioClip[] propels;

      private void Start() {
         swimStateController.OnStateEnabled.AddListener(HandleSwimStarted);
         swimStateController.OnPreparePropelStarted.AddListener(HandlePreparePropelStarted);
         swimStateController.OnPropelTriggered.AddListener(HandlePropelTriggered);
      }

      private void OnDestroy() {
         swimStateController.OnStateEnabled.RemoveListener(HandleSwimStarted);
         swimStateController.OnPreparePropelStarted.RemoveListener(HandlePreparePropelStarted);
         swimStateController.OnPropelTriggered.RemoveListener(HandlePropelTriggered);
      }

      private void HandlePropelTriggered() => Play(propels.RandomOrDefault());
      private void HandlePreparePropelStarted() => Play(preparePropels.RandomOrDefault());
      private void HandleSwimStarted() => Play(waterSplashes.RandomOrDefault());

      public void PlayFootstep() => Play(footsteps.RandomOrDefault(), footstepsVolume);
   }
}