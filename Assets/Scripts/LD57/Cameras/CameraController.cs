using Unity.Cinemachine;
using UnityEngine;

namespace LD57.Cameras {
   public class CameraController : MonoBehaviour {
      public static Camera GameCamera { get; private set; }

      public enum CameraTarget {
         Alien = 0,
         Vessel = 1
      }

      [SerializeField] private Camera gameCamera;
      [SerializeField] private CinemachineCamera alienCamera;
      [SerializeField] private CinemachineCamera vesselCamera;

      private void Awake() {
         GameCamera = gameCamera;
      }

      public static Vector2 CursorToWorldPoint(Vector2 cursorPoint) => GameCamera.ScreenToWorldPoint(cursorPoint);

      public void ActivateCamera(CameraTarget target) {
         alienCamera.enabled = target == CameraTarget.Alien;
         vesselCamera.enabled = target == CameraTarget.Vessel;
      }
   }
}