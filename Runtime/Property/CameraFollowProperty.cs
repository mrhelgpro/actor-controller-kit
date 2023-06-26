using UnityEngine;
using Cinemachine;

namespace Actormachine
{
    public enum CameraPresetMode { Default, Parameter }
    public enum InputOrbitMode { Free, LeftHold, MiddleHold, RightHold, Lock }

    [AddComponentMenu("Actormachine/Property/CameraFollow Property")]
    public sealed class CameraFollowProperty : Property
    {
        // Model Parameters
        public CameraPresetMode Preset = CameraPresetMode.Default;
        public Transform Follow;
        public InputOrbitMode InputOrbitMode;
        public CameraParameters EnterParameters = new CameraParameters();

        // Model Components
        private Inputable _inputable;
        private ActorVirtualCamera _actorVirtualCamera;

        public override void OnEnableState()
        {
            // Finds or creates a Camera
            Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            if (camera == null)
            {
                GameObject instantiate = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
                instantiate.transform.position = new Vector3(0f, 0f, -10f);
                instantiate.transform.rotation = Quaternion.identity;
                camera = instantiate.GetComponent<Camera>();
            }

            camera.gameObject.name = "Main Camera (Cinemachine Brain)";
            camera.gameObject.tag = "MainCamera";
            camera.orthographic = false;
            camera.transform.SetAsFirstSibling();

            // Finds or creates a Cinemachine Brain
            CinemachineBrain cinemachineBrain = FindAnyObjectByType<CinemachineBrain>();

            if (cinemachineBrain == null)
            {
                cinemachineBrain = camera.gameObject.AddComponent<CinemachineBrain>();
                cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
                cinemachineBrain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.FixedUpdate;
            }

            // Finds or creates a Actor Virtual Camera
            ActorVirtualCamera actorVirtualCamera = FindAnyObjectByType<ActorVirtualCamera>();

            if (actorVirtualCamera == null)
            {
                GameObject instantiate = new GameObject("Cinemachine Virtual Camera", typeof(CinemachineVirtualCamera), typeof(ActorVirtualCamera));
                instantiate.transform.position = new Vector3(0f, 0f, -10f);
                instantiate.transform.rotation = Quaternion.identity;
                instantiate.hideFlags = HideFlags.NotEditable;
                actorVirtualCamera = instantiate.GetComponent<ActorVirtualCamera>();
            }

            actorVirtualCamera.gameObject.name = "Cinemachine Virtual Camera (Actor)";
            actorVirtualCamera.transform.SetAsFirstSibling();

            // Check Single Instance
            Instance.IsSingleInstanceOnScene<CinemachineBrain>();
            Instance.IsSingleInstanceOnScene<ActorVirtualCamera>();

            // Check Required Component
            if (Follow == null)
            {
                Followable followable = RootTransform.GetComponentInChildren<Followable>();

                if (followable == null)
                {
                    Debug.LogWarning(gameObject.name + " - You need to add a Follow (Transform)");
                }
                else
                {
                    Follow = followable.transform;
                }
            }
        }

        public override void OnEnterState()
        {
            // Add or Get comppnent in the Root
            _inputable = AddComponentInRoot<Inputable>();
            _actorVirtualCamera = FindAnyObjectByType<ActorVirtualCamera>();

            // Check Follow
            if (Follow)
            {
                if (Preset == CameraPresetMode.Parameter)
                {
                    _actorVirtualCamera.Enter(Follow, EnterParameters);
                }
            }
        }

        public override void OnActiveState()
        {
            if (_actorVirtualCamera.IsLock == false)
            {
                //fieldOfViewUpdate();
                //distaceUpdate();
                rotationUpdate();
            }
        }

        // Make this !!!
        private void fieldOfViewUpdate()
        {
            if (_inputable.ActionMiddleState == true)
            {
                if (_inputable.LookDelta.sqrMagnitude >= 0.01f)
                {
                    float value = _inputable.LookDelta.y * Time.deltaTime * 10;

                    _actorVirtualCamera.SetFieldOfView(value);
                }
            }
        }

        private void distaceUpdate()
        {
            if (_inputable.ActionMiddleState == true)
            {
                if (_inputable.LookDelta.sqrMagnitude >= 0.01f)
                {
                    float value = _inputable.LookDelta.y * Time.deltaTime * 10;
                    
                    _actorVirtualCamera.SetDistance(value);
                }
            }
        }

        private void rotationUpdate()
        {
            if (EnterParameters.CameraType == CameraType.ThirdPersonFollow)
            {
                bool isRotable = false;

                if (InputOrbitMode == InputOrbitMode.Free)
                {
                    isRotable = true;
                }
                else if (InputOrbitMode == InputOrbitMode.LeftHold)
                {
                    isRotable = _inputable.ActionLeftState;
                }
                else if (InputOrbitMode == InputOrbitMode.MiddleHold)
                {
                    isRotable = _inputable.ActionMiddleState;
                }
                else if (InputOrbitMode == InputOrbitMode.RightHold)
                {
                    isRotable = _inputable.ActionRightState;
                }

                if (isRotable == true)
                {
                    if (_inputable.LookDelta.sqrMagnitude >= 0.01f)
                    {
                        float deltaTimeMultiplier = 0.25f; // Make ckeck is Mouse or Gamepad

                        _actorVirtualCamera.CurrentParameters.OrbitHorizontal += _inputable.LookDelta.x * EnterParameters.OrbitSensitivityX * deltaTimeMultiplier;
                        _actorVirtualCamera.CurrentParameters.OrbitVertical += _inputable.LookDelta.y * EnterParameters.OrbitSensitivityY * deltaTimeMultiplier;

                        _actorVirtualCamera.CurrentParameters.OrbitHorizontal = Maths.ClampAngle(_actorVirtualCamera.CurrentParameters.OrbitHorizontal, float.MinValue, float.MaxValue);
                        _actorVirtualCamera.CurrentParameters.OrbitVertical = Maths.ClampAngle(_actorVirtualCamera.CurrentParameters.OrbitVertical, -30, 80);
                    }
                }
            }
        }
    }
}