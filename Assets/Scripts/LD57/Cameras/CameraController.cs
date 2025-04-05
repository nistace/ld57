using UnityEngine;

namespace LD57.Cameras {
   public class CameraController : MonoBehaviour {
      public static Camera GameCamera { get; private set; }

      [SerializeField] private Camera gameCamera;

      private void Awake() {
         GameCamera = gameCamera;
      }

      public static Vector2 CursorToWorldPoint(Vector2 cursorPoint) => GameCamera.ScreenToWorldPoint(cursorPoint);
   }
}