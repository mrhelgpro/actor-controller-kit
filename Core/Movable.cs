using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class Movable : MonoBehaviour
    {
        [Range(0, 1)] public float Slowing = 0;
        [Range(0, 10)] public float Acceleration = 0;
        [Range(0, 1f)] public float Gravity = 1;

        public float jumpForce = 1;

        protected Transform mainTransform;

        protected float getSpeedScale => _getAcceleration * _getSlowing * Time.fixedDeltaTime;
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

        public float HeightToForce(int height)
        {
            // force = 1 x = 4.528313f
            // force = 2 x = 3.181149f
            // force = 3 x = 2.590051f
            // force = 4 x = 2.23927f
            // force = 10 x = 1.410547
            // force = 20 x = 1

            float force = 0;

            switch (height)
            {
                case 1:
                    force = 4.528313f;
                    break;
                case 2:
                    force = 3.181149f;
                    break;
                case 3:
                    force = 2.590051f;
                    break;
                case 4:
                    force = 2.23927f;
                    break;
                default:
                    Debug.Log("Force on calculated for height " + force);
                    break;
            }

            return force * height;
        }
    }
}