using System;
using UnityEngine;

namespace LD57.Collectibles {
   [CreateAssetMenu]
   public class CollectibleType : ScriptableObject {
      [SerializeField] private string displayName;
      [SerializeField] private int collectionScore = 50;
      [SerializeField] private float collectionInterest = 5;

      public int CollectionScore => collectionScore;
      public float CollectionInterest => collectionInterest;
      public string DisplayName => displayName;

      private void OnValidate() {
         if (string.IsNullOrEmpty(displayName)) displayName = name;
      }
   }
}