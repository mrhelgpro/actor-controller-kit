using UnityEngine;

namespace AssemblyActorCore
{
    /*
    public enum SurfaceType { None, Ground, Grass, Water };

    public enum NormaleType { None, Straight, Stairs, Incline, Climb, Edge };

    public abstract class Positionable : MonoBehaviour
    {
        public bool IsGrounded;
    }
    */

    public class Surface
    { 
    
    }

    public abstract class Movable : MonoBehaviour
    {
        [Range(0, 1)] public float Slowing = 0;
        [Range(0, 10)] public float Acceleration = 0;
        [Range(0, 2)] public float Gravity = 1;
        
        public bool IsGrounded;

        protected Transform mainTransform;

        public float GetSpeedScale => _getAcceleration * _getSlowing * Time.fixedDeltaTime;
        private float _getSlowing => Slowing > 0 ? (Slowing <= 1 ? 1 - Slowing : 0) : 1;
        private float _getAcceleration => Acceleration > 0 ? Acceleration + 1 : 1;

        protected void Awake() => mainTransform = transform;

        public abstract void FreezAll();
        public abstract void FreezRotation();

        public abstract void MoveToDirection(Vector3 direction, float speed);
        public abstract void Jump(float force);

        public void MoveToPosition(Vector3 direction, float speed)
        {
            direction.Normalize();

            mainTransform.position += direction * speed * Time.fixedDeltaTime;
        }
    }
}