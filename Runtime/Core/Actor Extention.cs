using UnityEngine;
using Cinemachine;

namespace Actormachine
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
        /// Returns "true" if there is one instance of the class on the Scene, 
        /// and "false" if there is null or more than one. 
        /// </summary>
        public static bool IsSingleInstanceOnScene<T>() where T : Component
        {
            return IsSingleInstance<T>(GameObject.FindObjectsOfType<T>());
        }

        /// <summary> 
        /// Returns "true" if there is one instance of the class on gameObject, 
        /// and "false" if there is null or more than one. 
        /// </summary>
        public static bool IsSingleInstanceOnObject<T>(this GameObject gameObject) where T : Component
        {
            return IsSingleInstance<T>(gameObject.GetComponents<T>());
        }

        /// <summary> 
        /// Returns "true" if there is one instance of the class in children, 
        /// and "false" if there is null or more than one. 
        /// </summary>
        public static bool IsSingleInstanceInChildren<T>(this GameObject gameObject) where T : Component
        {
            return IsSingleInstance<T>(gameObject.GetComponentsInChildren<T>());
        }

        /// <summary> 
        /// Returns "true" if there is one instance, 
        /// and "false" if there is null or more than one. 
        /// </summary>
        public static bool IsSingleInstance<T>(T[] instances) where T : Component
        {
            if (instances.Length > 1)
            {
                Debug.LogWarning("<" + typeof(T).ToString() + "> should be a single Instance");

                return false;
            }

            return instances.Length == 1;
        }
    }

    public static class CinemachineExtantion
    {
        public static void SwitchPriority(CinemachineVirtualCameraBase switchedVirtualCamera)
        {
            CinemachineBrain cinemachineBrain = GameObject.FindAnyObjectByType<CinemachineBrain>();

            if (cinemachineBrain != null && cinemachineBrain.ActiveVirtualCamera != null)
            {
                CinemachineVirtualCameraBase currentVirtualCamera = cinemachineBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCameraBase>();
                currentVirtualCamera.Priority = 0;
            }

            switchedVirtualCamera.Priority = 100;
        }
    }
}