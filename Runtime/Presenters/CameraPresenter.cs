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

                    _followable.VirtualCamera.Parameters.OrbitHorizontal += _inputable.LookDelta.x * Parameters.OrbitSensitivityX * deltaTimeMultiplier;
                    _followable.VirtualCamera.Parameters.OrbitVertical += _inputable.LookDelta.y * Parameters.OrbitSensitivityY * deltaTimeMultiplier;

                    _followable.VirtualCamera.Parameters.OrbitHorizontal = ClampAngle(_followable.VirtualCamera.Parameters.OrbitHorizontal, float.MinValue, float.MaxValue);
                    _followable.VirtualCamera.Parameters.OrbitVertical = ClampAngle(_followable.VirtualCamera.Parameters.OrbitVertical, -30, 80);
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
                    // Show Camera Parameters
                    EditorGUILayout.LabelField("Camera Parameters", EditorStyles.boldLabel);
                    
                    serializedObject.Update();

                    thisTarget.Parameters.FieldOfView = EditorGUILayout.IntSlider("Field Of View", thisTarget.Parameters.FieldOfView, 20, 80);
                    thisTarget.Parameters.Damping = EditorGUILayout.Vector3Field("Damping", thisTarget.Parameters.Damping);
                    thisTarget.Parameters.Offset = EditorGUILayout.Vector3Field("Offset", thisTarget.Parameters.Offset);
                    thisTarget.Parameters.Distance = EditorGUILayout.Slider("Distance", thisTarget.Parameters.Distance, 1f, 20f);

                    thisTarget.Parameters.CameraType = (CameraType)EditorGUILayout.EnumPopup("Camera Type", thisTarget.Parameters.CameraType);

                    if (thisTarget.Parameters.CameraType == CameraType.ThirdPersonFollow)
                    {
                        // Show Orbit Parameters
                        thisTarget.InputOrbitMode = (InputOrbitMode)EditorGUILayout.EnumPopup("Input Orbit Mode", thisTarget.InputOrbitMode);

                        if (thisTarget.InputOrbitMode != InputOrbitMode.Lock)
                        {
                            thisTarget.Parameters.OrbitSensitivityX = EditorGUILayout.Slider("Orbit Sensitivity X", thisTarget.Parameters.OrbitSensitivityX, 0f, 2f);
                            thisTarget.Parameters.OrbitSensitivityY = EditorGUILayout.Slider("Orbit Sensitivity Y", thisTarget.Parameters.OrbitSensitivityY, 0f, 2f);
                        }
                    }
                    else
                    {
                        // Show Framing Transposer Parameters
                        thisTarget.Parameters.DeadZoneWidth = EditorGUILayout.Slider("Dead Zone Width", thisTarget.Parameters.DeadZoneWidth, 0f, 1f);
                        thisTarget.Parameters.DeadZoneHeight = EditorGUILayout.Slider("Dead Zone Height", thisTarget.Parameters.DeadZoneHeight, 0f, 1f);
                        thisTarget.Parameters.DeadZoneDepth = EditorGUILayout.FloatField("Dead Zone Height", thisTarget.Parameters.DeadZoneDepth);
                        thisTarget.Parameters.SoftZoneWidth = EditorGUILayout.Slider("Soft Zone Width", thisTarget.Parameters.SoftZoneWidth, 0f, 1f);
                        thisTarget.Parameters.SoftZoneHeight = EditorGUILayout.Slider("Soft Zone Height", thisTarget.Parameters.SoftZoneHeight, 0f, 1f);
                    }

                    serializedObject.ApplyModifiedProperties();

                    // Update Camera Parameters
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