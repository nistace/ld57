using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LD57.Audience {
   public class PointOfInterestFinder : MonoBehaviour {
      private HashSet<Collider2D> Found { get; } = new HashSet<Collider2D>();
      private HashSet<IPointOfInterest> FoundPointsOfInterest { get; } = new HashSet<IPointOfInterest>();

      public UnityEvent<IPointOfInterest> OnPointOfInterestFound { get; } = new UnityEvent<IPointOfInterest>();

      private void OnTriggerEnter2D(Collider2D other) {
         if (!Found.Add(other)) return;

         var pointOfInterest = other.GetComponentInParent<IPointOfInterest>();
         if (pointOfInterest == null) return;

         if (!FoundPointsOfInterest.Add(pointOfInterest)) return;

         OnPointOfInterestFound.Invoke(pointOfInterest);
      }
   }
}