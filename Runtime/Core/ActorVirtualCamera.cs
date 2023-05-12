using System;
using UnityEngine;
using Cinemachine;

namespace Actormachine
{
    /// <summary> 
    /// A camera that only works with Player.
    /// There should be only one on the scene.
    /// </summary>
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class ActorVirtualCamera : MonoBehaviour
    {
        public bool IsLock = false;
        public CameraParameters CurrentParameters = new CameraParameters();
        public CinemachineVirtualCamera VirtualCamera;
        Cinemachine3rdPersonFollow thirdPersonComponent;
        CinemachineFramingTransposer framingTransposerComponent;

        private void Awake()
        {
            VirtualCamera = GetComponent<CinemachineVirtualCamera>();
            VirtualCamera.Priority = 10;
        }

        private void LateUpdate() => updateRotation();

        /// <summary> Entering with setting Camera Parameters. It is not recommended to call in Update.</summary>
        public void Enter(Transform enterFollow, CameraParameters enterParameters)
        {
            VirtualCamera = GetComponent<CinemachineVirtualCamera>();
            VirtualCamera.Follow = enterFollow;

            // Set Camera Parameters
            CurrentParameters.CameraType = enterParameters.CameraType;
            CurrentParameters.FieldOfView = enterParameters.FieldOfView;
            CurrentParameters.Distance = enterParameters.Distance;
            CurrentParameters.Damping = enterParameters.Damping;
            
            if (CurrentParameters.CameraType == CameraType.ThirdPersonFollow)
            {
                // Third Person Follow Parameters
                if (thirdPersonComponent == null)
                {
                    thirdPersonComponent = VirtualCamera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
                }

                CurrentParameters.Offset = enterParameters.Offset;

                updateThirdPersonFollow();
            }
            else
            {
                // Framing Transposer Parameters
                if (framingTransposerComponent == null)
                {
                    framingTransposerComponent = VirtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
                }

                CurrentParameters.OrbitHorizontal = enterParameters.OrbitHorizontal;
                CurrentParameters.OrbitVertical = enterParameters.OrbitVertical;
                CurrentParameters.DeadZoneWidth = enterParameters.DeadZoneWidth;
                CurrentParameters.DeadZoneHeight = enterParameters.DeadZoneHeight;
                CurrentParameters.SoftZoneWidth = enterParameters.SoftZoneWidth;
                CurrentParameters.SoftZoneHeight = enterParameters.SoftZoneHeight;

                updateFramingTransposer();
            }

            // Update Cinemachine State
            if (VirtualCamera.Follow)
            {
                updateRotation();

                VirtualCamera.UpdateCameraState(VirtualCamera.Follow.position, CinemachineCore.CurrentTime);
            }
        }

        public void SetFieldOfView(float value)
        {
            CurrentParameters.FieldOfView = Mathf.Clamp(CurrentParameters.FieldOfView + value, 20, 80);

            VirtualCamera.m_Lens.FieldOfView = CurrentParameters.FieldOfView;
        }

        public void SetDistance(float value)
        {
            CurrentParameters.Distance = Mathf.Clamp(CurrentParameters.Distance + value, 1, 20);

            if (CurrentParameters.CameraType == CameraType.ThirdPersonFollow)
            {
                thirdPersonComponent.CameraDistance = CurrentParameters.Distance;
            }
            else
            {
                framingTransposerComponent.m_CameraDistance = CurrentParameters.Distance;
            }
        }

        private void updateThirdPersonFollow()
        {
            VirtualCamera.m_Lens.FieldOfView = CurrentParameters.FieldOfView;
            thirdPersonComponent.Damping = CurrentParameters.Damping;
            thirdPersonComponent.VerticalArmLength = 0;
            thirdPersonComponent.CameraSide = 1;
            thirdPersonComponent.ShoulderOffset = CurrentParameters.Offset;
            thirdPersonComponent.CameraDistance = CurrentParameters.Distance;
            thirdPersonComponent.CameraCollisionFilter = LayerMask.GetMask("Default");
            thirdPersonComponent.IgnoreTag = "Player";
            thirdPersonComponent.CameraRadius = 0.1f;
            thirdPersonComponent.DampingIntoCollision = 0;
            thirdPersonComponent.DampingFromCollision = 0.5f;
        }

        private void updateFramingTransposer()
        {
            VirtualCamera.m_Lens.FieldOfView = CurrentParameters.FieldOfView;
            framingTransposerComponent.m_XDamping = CurrentParameters.Damping.x;
            framingTransposerComponent.m_YDamping = CurrentParameters.Damping.y;
            framingTransposerComponent.m_ZDamping = CurrentParameters.Damping.z;
            framingTransposerComponent.m_CameraDistance = CurrentParameters.Distance;
            framingTransposerComponent.m_DeadZoneWidth = CurrentParameters.DeadZoneWidth;
            framingTransposerComponent.m_DeadZoneHeight = CurrentParameters.DeadZoneHeight;
            framingTransposerComponent.m_DeadZoneDepth = 0;
            framingTransposerComponent.m_SoftZoneWidth = CurrentParameters.SoftZoneWidth;
            framingTransposerComponent.m_SoftZoneHeight = CurrentParameters.SoftZoneHeight;
        }

        private void updateRotation()
        {
            if (VirtualCamera.Follow)
            {
                // Third Person Follow Rotation
                VirtualCamera.Follow.rotation = Quaternion.Euler(CurrentParameters.OrbitVertical, CurrentParameters.OrbitHorizontal, 0.0f);

                // Framing Transposer Rotation
                if (CurrentParameters.CameraType == CameraType.FramingTransposer)
                {
                    transform.rotation = Quaternion.Euler(CurrentParameters.OrbitVertical, CurrentParameters.OrbitHorizontal, 0);
                    transform.position = transform.rotation * new Vector3(CurrentParameters.Offset.x, CurrentParameters.Offset.y, -CurrentParameters.Distance) + VirtualCamera.Follow.transform.position;
                }
            }
        }
    }

    public enum CameraType { ThirdPersonFollow, FramingTransposer }

    /// <summary> Camera position and settings. </summary>
    [Serializable]
    public class CameraParameters
    {
        public CameraType CameraType = CameraType.ThirdPersonFollow;

        // Default Parameters
        [Range(20, 80)] public float FieldOfView = 60;
        [Range(1, 20)] public float Distance = 10;
        public Vector3 Damping = Vector3.zero;

        // Third Person Follow Parameters
        public Vector3 Offset = Vector3.zero;
        [Range(0, 5)] public float OrbitSensitivityX = 1.0f;
        [Range(0, 5)] public float OrbitSensitivityY = 0.5f;      

        // Framing Transposer Parameters
        [Range(-180, 180)] public float OrbitHorizontal;
        [Range(-90, 90)] public float OrbitVertical;
        [Range(0, 1)] public float DeadZoneWidth = 0.1f;
        [Range(0, 1)] public float DeadZoneHeight = 0.1f;
        [Range(0, 1)] public float SoftZoneWidth = 0.8f;
        [Range(0, 1)] public float SoftZoneHeight = 0.8f;
    }
}