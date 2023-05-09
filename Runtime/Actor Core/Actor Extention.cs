using UnityEngine;

namespace AssemblyActorCore
{
    public static class ActorMathf
    {
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }

        /// <summary> For calculating the exact height of the jump, based on gravity. </summary>
        public static float HeightToForce(this int height, float gravityScale = 1)
        {
            float force;

            switch (height)
            {
                case 0:
                    force = 0.0f;
                    break;
                case 1:
                    force = 4.532f;
                    break;
                case 2:
                    force = 6.375f;
                    break;
                case 3:
                    force = 7.777f;
                    break;
                case 4:
                    force = 8.965f;
                    break;
                case 5:
                    force = 10.01f;
                    break;
                default:
                    force = height * 2;
                    Debug.LogWarning("Force not calculated for height " + height);
                    break;
            }

            float gravity = 0.425f * gravityScale + 0.575f;

            return force * gravity;
        }
    }

    public static class ActorExtantion
    {
        /// <summary> 
        /// Returns "true" if there is one instance of the class on the scene, 
        /// and "false" if there is null or more than one. 
        /// </summary>
        public static bool CheckSingleInstance<T>() where T : Component
        {
            T[] instances = GameObject.FindObjectsOfType<T>();

            if (instances.Length > 1)
            {
                Debug.LogWarning("Should be a single instance of <" + typeof(T).ToString() + ">");

                return false;
            }

            return instances.Length > 0;
        }
    }
}