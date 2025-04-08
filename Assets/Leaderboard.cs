using System;
using System.Linq;
using LD57.MainControllers;
using LD57.Web;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Leaderboard : MonoBehaviour {
   [SerializeField] private Line[] lines;

   private void Start() {
      GameController.OnLeaderboardUpdated.AddListener(HandleLeaderboardReceived);
      gameObject.SetActive(false);
   }

   private void OnDestroy() {
      GameController.OnLeaderboardUpdated.RemoveListener(HandleLeaderboardReceived);
   }

   private void HandleLeaderboardReceived(LeaderboardWebRequest.GetResult.Entry[] entries) {
      gameObject.SetActive(entries is { Length: > 0 });
      if (!gameObject.activeSelf) return;

      var displayedEntries = entries.OrderByDescending(t => t.score).Take(lines.Length).ToArray();
      for (var i = 0; i < displayedEntries.Length; ++i) {
         lines[i].Set(displayedEntries[i].alienName, displayedEntries[i].score);
      }
      for (var i = displayedEntries.Length; i < lines.Length; ++i) {
         lines[i].Unset();
      }
      gameObject.SetActive(true);
   }

   [Serializable] private class Line {
      [SerializeField] private GameObject gameObject;
      [SerializeField] private TMP_Text episodeNumberText;
      [SerializeField] private TMP_Text alienNameText;
      [SerializeField] private TMP_Text scoreText;

      public Line() { }

      public Line(Transform transform) {
         gameObject = transform.gameObject;
         episodeNumberText = transform.GetChild(0).GetComponent<TMP_Text>();
         alienNameText = transform.GetChild(1).GetComponent<TMP_Text>();
         scoreText = transform.GetChild(2).GetComponent<TMP_Text>();
      }

      public void Set(string alienName, int score) {
         gameObject.SetActive(true);
         episodeNumberText.text = $"{gameObject.transform.GetSiblingIndex()}.";
         alienNameText.text = alienName;
         scoreText.text = $"{score}";
      }

      public void Unset() {
         gameObject.SetActive(false);
      }
   }

#if UNITY_EDITOR
   [ContextMenu("Load Lines")]
   private void LoadLines() {
      var children = Enumerable.Range(1, transform.childCount - 1).Select(t => transform.GetChild(t)).ToArray();
      lines = children.Select(t => new Line(t)).ToArray();
      EditorUtility.SetDirty(gameObject);
   }
#endif
}