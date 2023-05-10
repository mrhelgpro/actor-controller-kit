using System;
using UnityEngine;
using UnityEditor;
using Cinemachine;

namespace AssemblyActorCore
{
    /// <summary> 
    /// A camera that only works with Player.
    /// There should be only one on the scene.
    /// </summary>
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class ActorVirtualCamera : MonoBehaviour
    {
        public bool IsLock = false;
        public CameraParameters Parameters = new CameraParameters();
        public CinemachineVirtualCamera VirtualCamera;
        
        private bool _isEnter = false;

        private void Awake()
        {
            VirtualCamera = GetComponent<CinemachineVirtualCamera>();
            VirtualCamera.Priority = 10;
        }

        private void LateUpdate()
        {
            updateRotation();
        }

        /// <summary> Entering with setting Camera Parameters. It is not recommended to call in Update.</summary>
        public void Enter(Transform enterFollow, CameraParameters enterParameters, bool isPreview = false)
        {
            VirtualCamera = GetComponent<CinemachineVirtualCamera>();
            VirtualCamera.Follow = enterFollow;

            // Set Camera Parameters
            Parameters.CameraType = enterParameters.CameraType;
            Parameters.FieldOfView = enterParameters.FieldOfView;
            Parameters.Distance = enterParameters.Distance;
            Parameters.Damping = enterParameters.Damping;
            
            if (Parameters.CameraType == CameraType.ThirdPersonFollow)
            {
                // Third Person Follow Parameters
                Parameters.Offset = enterParameters.Offset;

                // Add a component to immediately update the parameters in preview mode
                if (isPreview) VirtualCamera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();

                updateThirdPersonFollow();
            }
            else
            {
                // Framing Transposer Parameters
                Parameters.OrbitHorizontal = enterParameters.OrbitHorizontal;
                Parameters.OrbitVertical = enterParameters.OrbitVertical;
                Parameters.DeadZoneWidth = enterParameters.DeadZoneWidth;
                Parameters.DeadZoneHeight = enterParameters.DeadZoneHeight;
                Parameters.SoftZoneWidth = enterParameters.SoftZoneWidth;
                Parameters.SoftZoneHeight = enterParameters.SoftZoneHeight;

                // Add a component to immediately update the parameters in preview mode
                if (isPreview) VirtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();

                updateFramingTransposer();
            }

            // Update Cinemachine State
            if (VirtualCamera.Follow)
            {
                updateRotation();

                VirtualCamera.UpdateCameraState(VirtualCamera.Follow.position, CinemachineCore.CurrentTime);

                _isEnter = true;
            }
        }

        public void updateThirdPersonFollow()
        {
            Cinemachine3rdPersonFollow thirdPerson = VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

            if (thirdPerson == null)
            {
                thirdPerson = VirtualCamera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
            }

            VirtualCamera.m_Lens.FieldOfView = Parameters.FieldOfView;
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
            CinemachineFramingTransposer framingTransposer = VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            if (framingTransposer == null)
            {
                framingTransposer = VirtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
            }

            VirtualCamera.m_Lens.FieldOfView = Parameters.FieldOfView;
            framingTransposer.m_XDamping = Parameters.Damping.x;
            framingTransposer.m_YDamping = Parameters.Damping.y;
            framingTransposer.m_ZDamping = Parameters.Damping.z;
            framingTransposer.m_CameraDistance = Parameters.Distance;
            framingTransposer.m_DeadZoneWidth = Parameters.DeadZoneWidth;
            framingTransposer.m_DeadZoneHeight = Parameters.DeadZoneHeight;
            framingTransposer.m_DeadZoneDepth = 0;
            framingTransposer.m_SoftZoneWidth = Parameters.SoftZoneWidth;
            framingTransposer.m_SoftZoneHeight = Parameters.SoftZoneHeight;
        }

        private void updateRotation()
        {
            if (IsLock == false)
            {
                if (_isEnter)
                {

                }

                // Third Person Follow Rotation
                VirtualCamera.Follow.rotation = Quaternion.Euler(Parameters.OrbitVertical, Parameters.OrbitHorizontal, 0.0f);

                // Framing Transposer Rotation
                if (Parameters.CameraType == CameraType.FramingTransposer)
                {
                    transform.rotation = Quaternion.Euler(Parameters.OrbitVertical, Parameters.OrbitHorizontal, 0);
                    transform.position = transform.rotation * new Vector3(Parameters.Offset.x, Parameters.Offset.y, -Parameters.Distance) + VirtualCamera.Follow.transform.position;
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
        [Range(20, 80)] public int FieldOfView = 60;
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

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(ActorVirtualCamera))]
    public class VirtualCameraEditor : ModelEditor
    {
        public override void OnInspectorGUI()
        {
            ActorVirtualCamera thisTarget = (ActorVirtualCamera)target;

            thisTarget.gameObject.hideFlags = HideFlags.NotEditable;

            if (CheckBootstrap<CameraBootstrap>()) return;

            DrawModelBox("Edited in the Presenter");
        }
    }
#endif
}