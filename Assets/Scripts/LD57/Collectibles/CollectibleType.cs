using System;
using UnityEngine;

namespace LD57.Collectibles {
   [CreateAssetMenu]
   public class CollectibleType : ScriptableObject {
      [SerializeField] private string displayName;
      [SerializeField] private string aDisplayName;
      [SerializeField] private int collectionScore = 50;
      [SerializeField] private float collectionInterest = 5;

      public int CollectionScore => collectionScore;
      public float CollectionInterest => collectionInterest;
      public string DisplayName => displayName;
      public string ADisplayName => aDisplayName;

      private void OnValidate() {
         if (string.IsNullOrEmpty(displayName)) displayName = name;
         if (string.IsNullOrEmpty(aDisplayName)) aDisplayName = "a " + displayName;
      }
   }
}