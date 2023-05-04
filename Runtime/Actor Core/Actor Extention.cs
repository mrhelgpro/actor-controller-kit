using UnityEngine;

namespace AssemblyActorCore
{
    public static class ActorExtention
    {
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
}