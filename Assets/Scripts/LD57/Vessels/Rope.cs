using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace LD57.Vessels {
   public class Rope : MonoBehaviour {
      [SerializeField] private SpriteRenderer ropeChunkPrefab;
      [SerializeField] private Transform origin;
      [SerializeField] private Transform destination;
      [SerializeField] private List<Vector3> points;
      [SerializeField] private List<SpriteRenderer> ropeChunks;
      [SerializeField] private LayerMask collidingLayers = ~0;

      private Vector3 lastValidPosition;

      private void Start() {
         lastValidPosition = origin.position;
         points.Add(origin.position);
         ropeChunks.Add(Instantiate(ropeChunkPrefab, origin.position, Quaternion.identity, transform));
      }

      private void Update() {
         var currentDestinationPosition = destination.position;

         if (CheckSightFromPoint(points[^1], out var hit)) {
            lastValidPosition = currentDestinationPosition;

            if (points.Count >= 2 && CheckSightFromPoint(points[^2], out hit)) {
               points.RemoveAt(points.Count - 1);
               ropeChunks[points.Count].enabled = false;
            }
         }
         else {
            var newPoint = Vector3.MoveTowards(points[^1], lastValidPosition, hit.distance);
            var valid = CheckSightFromPoint(newPoint, out _);
            for (var ttl = 0; !valid && ttl < 20; ++ttl) {
               newPoint = Vector3.MoveTowards(newPoint, lastValidPosition, .1f);
               valid = CheckSightFromPoint(newPoint, out _);
            }

            RefreshRopeChunk(ropeChunks[points.Count - 1], points[^1], newPoint);

            points.Add(newPoint);
            if (ropeChunks.Count < points.Count) {
               ropeChunks.Add(Instantiate(ropeChunkPrefab, lastValidPosition, Quaternion.identity, transform));
            }
            ropeChunks[points.Count - 1].enabled = true;
         }

         RefreshRopeChunk(ropeChunks[points.Count - 1], points[^1], destination.position);
      }

      private void RefreshRopeChunk(SpriteRenderer chunk, Vector3 fromPoint, Vector3 toPoint) {
         var chunkVector = toPoint - fromPoint;
         chunk.transform.position = fromPoint;
         chunk.transform.rotation = quaternion.LookRotation(Vector3.forward, chunkVector);
         chunk.size = new Vector2(chunk.size.x, chunkVector.magnitude);
      }

      private bool CheckSightFromPoint(Vector3 point, out RaycastHit2D hit) {
         hit = default;
         if (point == destination.position) return true;
         var pointToDestination = destination.position - point;
         hit = Physics2D.Raycast(point, pointToDestination, pointToDestination.magnitude, collidingLayers);
         return !hit.collider;
      }
   }
}