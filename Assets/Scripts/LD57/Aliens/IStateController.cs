using UnityEngine;

namespace LD57.Aliens {
   public interface IStateController {
      float GravityScale { get; }

      void EnableState();
      void DisableState();
      void Tick(ref Vector2 velocity);
   }
}