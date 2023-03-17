using UnityEngine;

namespace AssemblyActorCore
{
    public enum SurfaceType { None, Ground, Grass, Water };

    public enum NormaleType { None, Straight, Stairs, Incline, Climb, Edge };

    public abstract class Positionable : MonoBehaviour
    {
        public bool IsGrounded;

        protected LayerMask groundLayer;

        private void Awake() => groundLayer = LayerMask.GetMask("Default");
    }
}