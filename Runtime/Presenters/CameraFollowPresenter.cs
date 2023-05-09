using UnityEngine;
using UnityEditor;
using Cinemachine;

namespace AssemblyActorCore
{
    public enum InputOrbitMode { Free, LeftHold, MiddleHold, RightHold, Lock }
    
    public class CameraFollowPresenter : Presenter
    {
        // Model Parameters
        public InputOrbitMode InputOrbitMode;
        public CameraParameters Parameters = new CameraParameters();

        // Model Components
        private Inputable _inputable;
        private Followable _followable;

        protected override void Initiation()
        {
            // Get components using "GetComponentInRoot" to create them on <Actor>
            _followable = GetComponentInRoot<Followable>();
            _inputable = GetComponentInRoot<Inputable>();
        }

        public override void Enter()
        {
            _followable.Enter(Parameters);
        }

        public override void UpdateLoop()
        {
            if (_followable.ActorVirtualCamera)
            {
                if (Parameters.CameraType == CameraType.ThirdPersonFollow)
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
                            float deltaTimeMultiplier = 1.0f; // Make ckeck is Mouse or Gamepad

                            _followable.ActorVirtualCamera.Parameters.OrbitHorizontal += _inputable.LookDelta.x * Parameters.OrbitSensitivityX * deltaTimeMultiplier;
                            _followable.ActorVirtualCamera.Parameters.OrbitVertical += _inputable.LookDelta.y * Parameters.OrbitSensitivityY * deltaTimeMultiplier;

                            _followable.ActorVirtualCamera.Parameters.OrbitHorizontal = ActorMathf.ClampAngle(_followable.ActorVirtualCamera.Parameters.OrbitHorizontal, float.MinValue, float.MaxValue);
                            _followable.ActorVirtualCamera.Parameters.OrbitVertical = ActorMathf.ClampAngle(_followable.ActorVirtualCamera.Parameters.OrbitVertical, -30, 80);
                        }
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(CameraFollowPresenter))]
    public class CameraPresenterEditor : ModelEditor
    {
        public override void OnInspectorGUI()
        {
            CameraFollowPresenter thisTarget = (CameraFollowPresenter)target;

            Transform root = thisTarget.FindRootTransform;

            Followable followable = root.gameObject.GetComponentInChildren<Followable>();

            if (followable)
            {
                if (followable.ActorVirtualCamera == null)
                {
                    followable.FindActorVirtualCamera();
                }
                else
                {
                    // Show Camera Parameters
                    EditorGUILayout.LabelField("Camera Parameters", EditorStyles.boldLabel);
                    
                    serializedObject.Update();

                    thisTarget.Parameters.FieldOfView = EditorGUILayout.IntSlider("Field Of View", thisTarget.Parameters.FieldOfView, 20, 80);
                    thisTarget.Parameters.Distance = EditorGUILayout.Slider("Distance", thisTarget.Parameters.Distance, 1f, 20f);
                    thisTarget.Parameters.Damping = EditorGUILayout.Vector3Field("Damping", thisTarget.Parameters.Damping);

                    thisTarget.Parameters.CameraType = (CameraType)EditorGUILayout.EnumPopup("Camera Type", thisTarget.Parameters.CameraType);

                    if (thisTarget.Parameters.CameraType == CameraType.ThirdPersonFollow)
                    {
                        // Show Third Person Follow Parameters
                        thisTarget.Parameters.Offset = EditorGUILayout.Vector3Field("Offset", thisTarget.Parameters.Offset);
                        thisTarget.InputOrbitMode = (InputOrbitMode)EditorGUILayout.EnumPopup("Input Orbit Mode", thisTarget.InputOrbitMode);

                        if (thisTarget.InputOrbitMode != InputOrbitMode.Lock)
                        {
                            thisTarget.Parameters.OrbitSensitivityX = EditorGUILayout.Slider("Orbit Sensitivity X", thisTarget.Parameters.OrbitSensitivityX, 0.1f, 2f);
                            thisTarget.Parameters.OrbitSensitivityY = EditorGUILayout.Slider("Orbit Sensitivity Y", thisTarget.Parameters.OrbitSensitivityY, 0.1f, 2f);
                        }
                    }
                    else
                    {
                        // Show Framing Transposer Parameters
                        thisTarget.Parameters.OrbitHorizontal = EditorGUILayout.Slider("Orbit Horizontal", thisTarget.Parameters.OrbitHorizontal, -180f, 180f);
                        thisTarget.Parameters.OrbitVertical = EditorGUILayout.Slider("Orbit Vertical", thisTarget.Parameters.OrbitVertical, -90f, 90f);
                        thisTarget.Parameters.DeadZoneWidth = EditorGUILayout.Slider("Dead Zone Width", thisTarget.Parameters.DeadZoneWidth, 0f, 1f);
                        thisTarget.Parameters.DeadZoneHeight = EditorGUILayout.Slider("Dead Zone Height", thisTarget.Parameters.DeadZoneHeight, 0f, 1f);
                        thisTarget.Parameters.SoftZoneWidth = EditorGUILayout.Slider("Soft Zone Width", thisTarget.Parameters.SoftZoneWidth, 0f, 1f);
                        thisTarget.Parameters.SoftZoneHeight = EditorGUILayout.Slider("Soft Zone Height", thisTarget.Parameters.SoftZoneHeight, 0f, 1f);
                    }

                    // Update Camera Parameters
                    if (GUI.changed)
                    {
                        followable.Enter(thisTarget.Parameters, isPreview:true);
                    }

                    serializedObject.ApplyModifiedProperties();
                }

                return;
            }

            DrawModelBox("<Followable> is not found", BoxStyle.Error);
        }
    }
#endif
}