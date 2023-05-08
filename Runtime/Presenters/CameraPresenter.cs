using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public enum InputOrbitMode { Free, LeftHold, MiddleHold, RightHold, Lock }
    public enum InputLookMode { Free, LeftHold, MiddleHold, RightHold, Lock }
    public enum InputOffsetMode { Free, LeftHold, MiddleHold, RightHold, Lock }
    public class CameraPresenter : Presenter
    {
        // Model Parameters
        public InputOrbitMode InputOrbitMode;
        //public InputLookMode InputLookMode;
        //public InputOffsetMode InputOffsetMode;
        
        public CameraParameters Parameters = new CameraParameters();

        // Model Components
        private Inputable _inputable;
        private Followable _followable;

        protected override void Initiation()
        {
            // Get components using "GetComponentInRoot" to create them on <Actor>
            _followable = GetComponentInRoot<Followable>();
            _inputable = GetComponentInRoot<Inputable>();

            //_followable.transform.rotation = Quaternion.identity;
        }

        public override void Enter()
        {
            _followable.Enter(Parameters);
        }

        public override void UpdateLoop()
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

            if (isRotable)
            {
                if (_inputable.LookDelta.sqrMagnitude >= 0.01f)
                {
                    float deltaTimeMultiplier = 1.0f;

                    _followable.VirtualCamera.Parameters.Orbit.Horizontal += _inputable.LookDelta.x * Parameters.Orbit.SensitivityX * deltaTimeMultiplier;
                    _followable.VirtualCamera.Parameters.Orbit.Vertical += _inputable.LookDelta.y * Parameters.Orbit.SensitivityY * deltaTimeMultiplier;

                    _followable.VirtualCamera.Parameters.Orbit.Horizontal = ClampAngle(_followable.VirtualCamera.Parameters.Orbit.Horizontal, float.MinValue, float.MaxValue);
                    _followable.VirtualCamera.Parameters.Orbit.Vertical = ClampAngle(_followable.VirtualCamera.Parameters.Orbit.Vertical, -30, 80);
                    
                    /*
                    Parameters.Orbit.Horizontal += _inputable.LookDelta.x * Parameters.Orbit.SensitivityX * deltaTimeMultiplier;
                    Parameters.Orbit.Vertical += _inputable.LookDelta.y * Parameters.Orbit.SensitivityY * deltaTimeMultiplier;

                    Parameters.Orbit.Horizontal = ClampAngle(Parameters.Orbit.Horizontal, float.MinValue, float.MaxValue);
                    Parameters.Orbit.Vertical = ClampAngle(Parameters.Orbit.Vertical, -30, 80);
                    */
                }
            }

            //_followable.UpdateParameters();
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(CameraPresenter))]
    public class CameraPresenterEditor : ModelEditor
    {
        public override void OnInspectorGUI()
        {
            CameraPresenter thisTarget = (CameraPresenter)target;

            Transform root = thisTarget.FindRootTransform;

            Followable followable = root.gameObject.GetComponentInChildren<Followable>();

            if (followable)
            {
                if (followable.VirtualCamera == null)
                {
                    DrawModelBox("Add Virtual Camera to <Followable>", BoxStyle.Error);
                }
                else
                {
                    DrawDefaultInspector();

                    if (Application.isPlaying == false)
                    {
                        if (GUI.changed)
                        {
                            followable.Enter(thisTarget.Parameters);
                        }
                    }
                }

                return;
            }

            DrawModelBox("<Followable> is not found", BoxStyle.Error);
        }
    }
#endif
}