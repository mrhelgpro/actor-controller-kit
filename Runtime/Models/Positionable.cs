using UnityEngine;

namespace AssemblyActorCore
{
    public class Positionable : Model
    {
        public bool IsGrounded;
        public bool IsObstacle;
        public bool IsSliding;
        public string SurfaceType = "None";
        public float SurfaceSlope;

        protected Vector3 surfaceNormal;
        protected LayerMask groundLayer;
        protected int layerMask;

        protected new void Awake()
        {
            base.Awake();

            groundLayer = LayerMask.GetMask("Default");
            layerMask = ~(1 << LayerMask.NameToLayer("Actor"));
        } 
    }
}