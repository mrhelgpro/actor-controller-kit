using System;
using UnityEngine;
using UnityEditor;
using Cinemachine;

namespace AssemblyActorCore
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCamera : MonoBehaviour
    {
        public CameraParameters Parameters = new CameraParameters();
        protected CinemachineVirtualCamera cinemachineVirtualCamera;

        private void LateUpdate()
        {
            UpdateRotation();
        }

        protected void UpdateRotation() => cinemachineVirtualCamera.Follow.rotation = Quaternion.Euler(Parameters.Orbit.Vertical, Parameters.Orbit.Horizontal, 0.0f);

        public void Enter(Transform enterFollow, CameraParameters enterParameters)
        {
            cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            cinemachineVirtualCamera.Follow = enterFollow;

            // Set Camera Parameters
            Parameters.CameraType = enterParameters.CameraType;
            Parameters.FieldOfView = enterParameters.FieldOfView;
            Parameters.Damping = enterParameters.Damping;
            Parameters.Offset = enterParameters.Offset;
            Parameters.Distance = enterParameters.Distance;

            // Update Cinemachine Parameters
            if (Parameters.CameraType == CameraType.ThirdPersonFollow)
            {
                updateThirdPersonFollow();
            }
            else
            {
                updateFramingTransposer();
            }

            // Update Cinemachine State
            if (cinemachineVirtualCamera.Follow)
            {
                UpdateRotation();

                cinemachineVirtualCamera.UpdateCameraState(cinemachineVirtualCamera.Follow.position, CinemachineCore.CurrentTime);
            }
        }  

        public void updateThirdPersonFollow()
        {
            Cinemachine3rdPersonFollow thirdPerson = cinemachineVirtualCamera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();

            cinemachineVirtualCamera.m_Lens.FieldOfView = Parameters.FieldOfView;
            thirdPerson.Damping = Parameters.Damping;
            thirdPerson.VerticalArmLength = 0;
            thirdPerson.CameraSide = 1;
            thirdPerson.ShoulderOffset = Parameters.Offset;
            thirdPerson.CameraDistance = Parameters.Distance;
            thirdPerson.CameraCollisionFilter = LayerMask.GetMask("Default");
            thirdPerson.IgnoreTag = "Player";
            thirdPerson.CameraRadius = 0.1f;
            thirdPerson.DampingIntoCollision = 0;
            thirdPerson.DampingFromCollision = 0.5f;
        }

        private void updateFramingTransposer()
        {
            CinemachineFramingTransposer framingTransposer = cinemachineVirtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();

            cinemachineVirtualCamera.m_Lens.FieldOfView = Parameters.FieldOfView;
            framingTransposer.m_XDamping = Parameters.Damping.x;
            framingTransposer.m_YDamping = Parameters.Damping.y;
            framingTransposer.m_ZDamping = Parameters.Damping.z;
            framingTransposer.m_CameraDistance = Parameters.Distance;
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(VirtualCamera))]
    public class VirtualCameraEditor : ModelEditor
    {
        public override void OnInspectorGUI()
        {
            VirtualCamera thisTarget = (VirtualCamera)target;

            CinemachineVirtualCamera cinemachineVirtualCamera = thisTarget.gameObject.GetComponent<CinemachineVirtualCamera>();

            // Set Degfault Parameters
            if (Application.isPlaying == false)
            {
                if (GUI.changed)
                {
                    Cinemachine3rdPersonFollow thirdPerson = cinemachineVirtualCamera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
                    cinemachineVirtualCamera.m_Lens.FieldOfView = thisTarget.Parameters.FieldOfView;
                    thirdPerson.Damping = thisTarget.Parameters.Damping;
                    thirdPerson.VerticalArmLength = 0;
                    thirdPerson.ShoulderOffset = thisTarget.Parameters.Offset;
                    thirdPerson.CameraDistance = thisTarget.Parameters.Distance;
                    thirdPerson.CameraCollisionFilter = LayerMask.GetMask("Default");
                }
            }

            // Check Follow
            if (cinemachineVirtualCamera.Follow == null)
            {
                DrawModelBox("Virtual Camera must be added to <Followable>", BoxStyle.Error);
            }
            else
            {
                DrawModelBox("Edited in the Presenter");
            }

            thisTarget.gameObject.hideFlags = HideFlags.NotEditable;
            //thisTarget.gameObject.hideFlags = HideFlags.None;
        }
    }
#endif

    public enum CameraType { ThirdPersonFollow, FramingTransposer }

    /// <summary> Camera position and settings. </summary>
    [Serializable]
    public class CameraParameters
    {
        public CameraType CameraType = CameraType.ThirdPersonFollow;
        public Vector2Orbit Orbit = new Vector2Orbit(1.0f, 0.5f);
        [Range(20, 80)] public int FieldOfView = 60;
        public Vector3 Damping = Vector3.zero;
        public Vector3 Offset = Vector3.zero;
        [Range(1, 20)] public float Distance = 10;
    }

    [Serializable]
    public struct Vector2Orbit
    {
        [Range(-180, 180)] public float Horizontal;
        [Range(-90, 90)] public float Vertical;
        [Range(0, 5)] public float SensitivityX;
        [Range(0, 5)] public float SensitivityY;

        public Vector2Orbit(float x, float y)
        {
            Horizontal = 0;
            Vertical = 0;
            SensitivityX = x;
            SensitivityY = y;
        }
    }
}