using System;
using UnityEngine;

namespace LD57.Audience {
   [Serializable]
   public class AudienceHighlight {
      [SerializeField] private string action;
      [SerializeField] private int score;

      public string Action => action;

      public int Score => score;

      public AudienceHighlight() { }

      public AudienceHighlight(string action, int score) {
         this.action = action;
         this.score = score;
      }
   }
}