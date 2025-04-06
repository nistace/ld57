using UnityEngine;

namespace LD57.Vessels.Anchors {
   public interface IAnchorAttachable {
      Transform transform { get; }
      Transform AnchorAttachPointReference { get; }
      WinchEffect EffectOnWinchActivation { get; }

      public enum WinchEffect {
         None = 0,
         MoveAnchorOwner = 1,
         MoveAnchor = 2
      }

      void Move(Vector2 force);
   }
}