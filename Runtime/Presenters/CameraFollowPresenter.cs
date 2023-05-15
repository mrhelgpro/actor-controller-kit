using UnityEngine;

namespace Actormachine
{
    public enum InputOrbitMode { Free, LeftHold, MiddleHold, RightHold, Lock }
    
    public sealed class CameraFollowPresenter : Presenter
    {
        // Model Parameters
        public Transform Follow;
        public InputOrbitMode InputOrbitMode;
        public CameraParameters EnterParameters = new CameraParameters();

        // Model Components
        private Inputable _inputable;
        private ActorVirtualCamera _actorVirtualCamera;

        protected override void Initiation()
        {
            // Check Bootstrap
            Bootstrap.Create<BootstrapCamera>();
            
            // Get components using "GetComponentInRoot" to create them on <Actor>
            _inputable = GetComponentInRoot<Inputable>();
            _actorVirtualCamera = FindAnyObjectByType<ActorVirtualCamera>();

            // Check Required Component
            if (Follow == null)
            {
                Followable followable = RootTransform.GetComponentInChildren<Followable>();

                if (followable)
                {
                    Follow = followable.transform;

                    return;
                }

                Debug.LogWarning(gameObject.name + " - You need to add a Follow (Transform)");
            } 
        }

        public override void Enter()
        {
            if (Follow)
            {
                _actorVirtualCamera.Enter(Follow, EnterParameters);
            }
        }

        public override void UpdateLoop()
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
                        float deltaTimeMultiplier = 1.0f; // Make ckeck is Mouse or Gamepad

                        _actorVirtualCamera.CurrentParameters.OrbitHorizontal += _inputable.LookDelta.x * EnterParameters.OrbitSensitivityX * deltaTimeMultiplier;
                        _actorVirtualCamera.CurrentParameters.OrbitVertical += _inputable.LookDelta.y * EnterParameters.OrbitSensitivityY * deltaTimeMultiplier;

                        _actorVirtualCamera.CurrentParameters.OrbitHorizontal = ActorMathf.ClampAngle(_actorVirtualCamera.CurrentParameters.OrbitHorizontal, float.MinValue, float.MaxValue);
                        _actorVirtualCamera.CurrentParameters.OrbitVertical = ActorMathf.ClampAngle(_actorVirtualCamera.CurrentParameters.OrbitVertical, -30, 80);
                    }
                }
            }
        }
    }
}