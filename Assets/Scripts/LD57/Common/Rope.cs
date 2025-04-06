using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace LD57.Common {
   public class Rope : MonoBehaviour {
      [SerializeField] private SpriteRenderer ropeChunkPrefab;
      [SerializeField] private Transform destination;
      [SerializeField] private float safeRadiusAroundExtremities = .1f;
      [SerializeField] private List<Vector3> points;
      [SerializeField] private List<SpriteRenderer> ropeChunks;
      [SerializeField] private LayerMask collidingLayers = ~0;

      private void Start() {
         ResetRope();
      }

      private void Update() {
         if (UpdateStartOfRope() | UpdateEndOfRope()) {
            UpdateVisuals();
         }
      }

      private bool UpdateStartOfRope() => UpdateRopeExtremity(0, transform.position, 1, PrependPoint);
      private bool UpdateEndOfRope() => UpdateRopeExtremity(points.Count - 1, destination.position, -1, points.Add);
      private void PrependPoint(Vector3 point) => points.Insert(0, point);

      private bool UpdateRopeExtremity(int chunkIndex, Vector3 expectedPosition, int chunkDelta, Action<Vector3> addPointAtExtremityAction) {
         if (points[chunkIndex] == expectedPosition) return false;

         var previousValidPosition = points[chunkIndex];
         points[chunkIndex] = expectedPosition;

         var secondChunkIndex = chunkIndex + chunkDelta;
         var thirdChunkIndex = secondChunkIndex + chunkDelta;

         var sightFromSecondToFirst = CheckSightFromPoint(points[secondChunkIndex], points[chunkIndex], out var secondToFirstHit);
         if (sightFromSecondToFirst || Vector3.Magnitude(points[chunkIndex] - points[secondChunkIndex]) < safeRadiusAroundExtremities) {
            if (points.Count > 2) {
               var sightFromThirdToFirst = CheckSightFromPoint(points[thirdChunkIndex], points[chunkIndex], out _);
               if (sightFromThirdToFirst || Vector3.Magnitude(points[chunkIndex] - points[thirdChunkIndex]) < safeRadiusAroundExtremities) {
                  points.RemoveAt(secondChunkIndex);
               }
            }

            return true;
         }

         var newPoint = Vector3.MoveTowards(points[secondChunkIndex], previousValidPosition, secondToFirstHit.distance);
         var valid = CheckSightFromPoint(newPoint, expectedPosition, out _);
         for (var ttl = 0; !valid && ttl < 20; ++ttl) {
            newPoint = Vector3.MoveTowards(newPoint, previousValidPosition, .1f);
            valid = CheckSightFromPoint(newPoint, expectedPosition, out _);
         }

         points[chunkIndex] = newPoint;
         addPointAtExtremityAction.Invoke(expectedPosition);

         return true;
      }

      private void UpdateVisuals() {
         for (var i = 0; i < points.Count - 1; ++i) {
            if (ropeChunks.Count <= i) {
               ropeChunks.Add(Instantiate(ropeChunkPrefab, points[i], Quaternion.identity, transform));
            }
            ropeChunks[i].enabled = true;
            RefreshRopeChunk(ropeChunks[i], points[i], points[i + 1]);
         }

         for (var i = points.Count - 1; i < ropeChunks.Count; ++i) {
            ropeChunks[i].enabled = false;
         }
      }

      private static void RefreshRopeChunk(SpriteRenderer chunk, Vector3 fromPoint, Vector3 toPoint) {
         var chunkVector = toPoint - fromPoint;
         chunk.transform.position = fromPoint;
         chunk.transform.rotation = quaternion.LookRotation(Vector3.forward, chunkVector);
         chunk.size = new Vector2(chunk.size.x, chunkVector.magnitude);
      }

      private bool CheckSightFromPoint(Vector3 fromPoint, Vector3 toPoint, out RaycastHit2D hit) {
         hit = default;
         if (fromPoint == toPoint) return true;
         var pointToDestination = toPoint - fromPoint;
         hit = Physics2D.Raycast(fromPoint, pointToDestination, pointToDestination.magnitude, collidingLayers);
         return !hit.collider;
      }

      public Vector2 GetRopeStartDirection() => (points[1] - points[0]).normalized;
      public Vector2 GetRopeEndDirection() => (points[^2] - points[^1]).normalized;
      public Vector2 GetRopeOneBeforeEnd() => points[^2];
      public Vector2 GetRopeEnd() => points[^1];

      public bool TryGetPointOnRopeFromEnd(float distance, out Vector2 point, out Vector2 tangent) {
         var ropeLength = 0f;
         for (var ropeChunkIndex = points.Count - 2; ropeChunkIndex >= 0; ropeChunkIndex--) {
            var ropeChunk = ropeChunks[ropeChunkIndex];
            if (distance <= ropeLength + ropeChunk.size.y) {
               tangent = ropeChunk.transform.up;
               point = (Vector2)ropeChunk.transform.position + tangent * (ropeChunk.size.y - (distance - ropeLength));
               return true;
            }
            ropeLength += ropeChunk.size.y;
         }
         point = default;
         tangent = default;
         return false;
      }

      public bool IsLongerThan(float length) {
         var ropeLength = 0f;
         for (var ropeChunkIndex = 0; ropeChunkIndex < ropeChunks.Count && ropeChunks[ropeChunkIndex].enabled; ropeChunkIndex++) {
            ropeLength += ropeChunks[ropeChunkIndex].size.y;
            if (length < ropeLength) return true;
         }
         return false;
      }

      public void ResetRope() {
         points.Clear();

         points.Add(transform.position);
         points.Add(transform.position);

         UpdateVisuals();
      }
   }
}