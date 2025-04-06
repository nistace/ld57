using System;
using LD57.Aliens;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LD57.Environment {
   [ExecuteAlways]
   public class EnvironmentGenerator : MonoBehaviour {
      [SerializeField] private bool generateInEditor;
      [SerializeField] private TilemapGenerator[] tilemapGenerators;
      [SerializeField] private int generationAroundX = 0;
      [SerializeField] private int generationHalfWidth = 50;
      [SerializeField] private int maxGenerationY = -30;
      [SerializeField] private int generationRadius = 20;
      [SerializeField] private int generationOffsetY = -20;
      [SerializeField] private int generationStepY = 10;
      [SerializeField] private Transform generationTarget;

      [SerializeField] private int lowestGeneratedY;

      private void Start() {
         if (!generationTarget) generationTarget = FindFirstObjectByType<AlienController>().transform;
      }

      private void Update() {
         if (!Application.isPlaying && !generateInEditor) return;

         var maxY = maxGenerationY;

         if (Application.isPlaying) {
            maxY = Mathf.Min(maxY, lowestGeneratedY - 1);
         }

         var y = generationStepY * Mathf.FloorToInt(Mathf.FloorToInt(generationTarget.position.y) * 1f / generationStepY) + generationOffsetY;

         var topLeft = new Vector2Int(generationAroundX - generationHalfWidth, Mathf.Min(maxY, y + generationRadius));
         var bottomRight = new Vector2Int(generationAroundX + generationHalfWidth, Mathf.Min(maxY + 1, y - generationRadius));

         if (bottomRight.y > topLeft.y) return;

         foreach (var tilemapGenerator in tilemapGenerators) {
            if (tilemapGenerator.IsValid) {
               tilemapGenerator.GenerateInRect(topLeft, bottomRight);
            }
         }

         lowestGeneratedY = bottomRight.y;
      }

      [Serializable]
      private class TilemapGenerator {
         [SerializeField] private Tilemap map;
         [SerializeField] private TileBase tile;
         [SerializeField] private float tileProbability = .4f;
         [SerializeField] private float seedXOffset = 78.8f;
         [SerializeField] private float seedYOffset;
         [SerializeField] private float seedXCoefficient = .0894f;
         [SerializeField] private float seedYCoefficient = .0816f;
         [SerializeField] private int xRange = 40;

         public bool IsValid => map && tile;

         public void GenerateInRect(Vector2Int topLeft, Vector2Int bottomRight) {
            for (var x = topLeft.x; x <= bottomRight.x; ++x)
            for (var y = bottomRight.y; y <= topLeft.y; ++y) {
               var position = new Vector2Int(x, y);
               map.SetTile((Vector3Int)position, HasTileAt(position) ? tile : default);
            }
         }

         private bool HasTileAt(Vector2Int position) {
            if (position.x < -xRange) return true;
            if (position.x > xRange) return true;
            return Mathf.PerlinNoise(seedXOffset + position.x * seedXCoefficient, seedYOffset + position.y * seedYCoefficient) < tileProbability;
         }
      }
   }
}