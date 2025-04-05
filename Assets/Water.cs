using LD57.Cameras;
using UnityEngine;

public class Water : MonoBehaviour {
   [SerializeField] private float maxY = -32;
   [SerializeField] private float z = 0;

   private void Update() {
      var position = CameraController.GameCamera.transform.position;
      position.y = Mathf.Min(position.y, maxY);
      position.z = z;
      transform.position = position;
   }
}