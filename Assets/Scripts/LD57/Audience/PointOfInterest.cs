using UnityEngine;

namespace LD57.Audience {
   public class PointOfInterest : MonoBehaviour, IPointOfInterest {
      [SerializeField] private string displayName = "a point of interest";
      [SerializeField] private int interest = 10;
      [SerializeField] private int findingScore = 10;

      public string DisplayName => displayName;
      public int Interest => interest;
      public int FindingScore => findingScore;
   }
}