using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour {
   [SerializeField] private float waterY = 0;
   [SerializeField] private Transform detectorSource;
   [SerializeField] private LayerMask safeAreaMask;

   public bool IsInWater => detectorSource.position.y < waterY && ContainingSafeAreas.Count == 0;

   private HashSet<GameObject> ContainingSafeAreas { get; } = new HashSet<GameObject>();

   private void OnTriggerEnter2D(Collider2D other) {
      if ((safeAreaMask & (1 << other.gameObject.layer)) > 0) {
         ContainingSafeAreas.Add(other.gameObject);
      }
   }

   private void OnTriggerExit2D(Collider2D other) {
      if ((safeAreaMask & (1 << other.gameObject.layer)) > 0) {
         ContainingSafeAreas.Remove(other.gameObject);
      }
   }
}