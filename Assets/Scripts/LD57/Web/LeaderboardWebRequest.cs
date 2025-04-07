using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LD57.Web {
   public static class LeaderboardWebRequest {
      public static IEnumerator Get(GetData data, UnityAction<GetResult> callback, UnityAction errorCallback) => WebRequests.Get(WebRequests.GetUri("leaderboard"), data, callback, errorCallback);
      public static IEnumerator Post(PostData data, UnityAction<PostResult> callback, UnityAction errorCallback) => WebRequests.Post(WebRequests.GetUri("leaderboard"), data, callback, errorCallback);

      [Serializable]
      public class GetData { }

      [Serializable]
      public class GetResult {
         public Entry[] entries;

         [Serializable]
         public class Entry {
            public string alienName;
            public int bodyHue;
            public int eyeHue;
            public int score;
            public int deathPositionX;
            public int deathPositionY;

            public float NormalizedBodyHue => bodyHue * .01f;
            public float NormalizedEyeHue => eyeHue * .01f;
            public Vector2 DeathPosition => new Vector2(deathPositionX * .01f, deathPositionY * .01f);
         }
      }

      [Serializable]
      public class PostData {
         public string alienName;
         public int bodyHue;
         public int eyeHue;
         public int score;
         public int deathPositionX;
         public int deathPositionY;

         public float NormalizedBodyHue {
            set => bodyHue = Mathf.RoundToInt(value * 100);
         }

         public float NormalizedEyeHue {
            set => eyeHue = Mathf.RoundToInt(value * 100);
         }

         public Vector2 DeathPosition {
            set {
               deathPositionX = Mathf.RoundToInt(value.x * 100);
               deathPositionY = Mathf.RoundToInt(value.y * 100);
            }
         }
      }

      [Serializable]
      public class PostResult {
         public bool success;
         public int id;
      }
   }
}