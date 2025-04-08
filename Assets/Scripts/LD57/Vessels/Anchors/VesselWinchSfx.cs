using LD57.Audio;
using UnityEngine;

namespace LD57.Vessels.Anchors {
   public class VesselWinchSfx : SfxSource {
      [SerializeField] private AnchorWinchControl winchControl;

      private void Start() {
         winchControl.OnActivated.AddListener(HandleWinchActivated);
         winchControl.OnDeactivated.AddListener(HandleWinchDeactivated);
      }

      private void OnDestroy() {
         winchControl.OnActivated.RemoveListener(HandleWinchActivated);
         winchControl.OnDeactivated.RemoveListener(HandleWinchDeactivated);
      }

      private void HandleWinchDeactivated() => Source.Stop();
      private void HandleWinchActivated() => Source.Play();
   }
}