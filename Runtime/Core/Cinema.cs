using UnityEngine;
using Cinemachine;

namespace Actormachine
{
    public static class Cinema
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