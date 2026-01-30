using UnityEngine;

namespace Runtime.Utilities
{
    public static class Constants
    {
        #region Tags

        public static readonly string CannonBallTag = "Ball";
        public static readonly string ObstacleTag = "Obstacle";

        #endregion

        #region Layers

       
        public static readonly int ObstacleLayer = LayerMask.GetMask("Obstacle");
        

        #endregion

        #region Animator Hashes

        public static readonly int Fire = Animator.StringToHash("Fire");
        public static readonly int IsMainMenuIdle = Animator.StringToHash("isMainMenuIdle");

        #endregion
    }
}