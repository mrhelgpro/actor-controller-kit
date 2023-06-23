using UnityEngine;
using UnityEditor;
using Cinemachine;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(CameraFollowProperty))]
    [CanEditMultipleObjects]
    public class CameraFollowPropertyInspector : ActormachineComponentBaseInspector
    {
        private void OnEnable()
        {
            CameraFollowProperty thisTarget = (CameraFollowProperty)target;

            thisTarget.OnEnableState();
        }

        public override void OnInspectorGUI()
        {
            // Draw a Inspector
            CameraFollowProperty thisTarget = (CameraFollowProperty)target;
            
            // Check Follow
            thisTarget.Follow = EditorGUILayout.ObjectField("Follow", thisTarget.Follow, typeof(Transform), true) as Transform;

            if (thisTarget.Follow == false)
            {
                Inspector.DrawSubtitle("To edit parameter settings, add Follow", BoxStyle.Error);
                return;
            }

            thisTarget.Preset = (CameraPresetMode)EditorGUILayout.EnumPopup("Preset", thisTarget.Preset);

            if (thisTarget.Preset == CameraPresetMode.Parameter)
            {
                // Show Camera Parameters
                thisTarget.EnterParameters.FieldOfView = EditorGUILayout.Slider("Field Of View", thisTarget.EnterParameters.FieldOfView, 20, 80);
                thisTarget.EnterParameters.Distance = EditorGUILayout.Slider("Distance", thisTarget.EnterParameters.Distance, 1f, 20f);
                thisTarget.EnterParameters.Damping = EditorGUILayout.Vector3Field("Damping", thisTarget.EnterParameters.Damping);

                // Check Camera Type
                thisTarget.EnterParameters.CameraType = (CameraType)EditorGUILayout.EnumPopup("Camera Type", thisTarget.EnterParameters.CameraType);

                if (thisTarget.EnterParameters.CameraType == CameraType.ThirdPersonFollow)
                {
                    // Show Third Person Follow Parameters
                    thisTarget.EnterParameters.Offset = EditorGUILayout.Vector3Field("Offset", thisTarget.EnterParameters.Offset);
                    thisTarget.InputOrbitMode = (InputOrbitMode)EditorGUILayout.EnumPopup("Input Orbit Mode", thisTarget.InputOrbitMode);

                    if (thisTarget.InputOrbitMode != InputOrbitMode.Lock)
                    {
                        thisTarget.EnterParameters.OrbitSensitivityX = EditorGUILayout.Slider("Orbit Sensitivity X", thisTarget.EnterParameters.OrbitSensitivityX, 0.0f, 1f);
                        thisTarget.EnterParameters.OrbitSensitivityY = EditorGUILayout.Slider("Orbit Sensitivity Y", thisTarget.EnterParameters.OrbitSensitivityY, 0.0f, 1f);
                    }
                }
                else
                {
                    // Show Framing Transposer Parameters
                    thisTarget.EnterParameters.OrbitHorizontal = EditorGUILayout.Slider("Orbit Horizontal", thisTarget.EnterParameters.OrbitHorizontal, -180f, 180f);
                    thisTarget.EnterParameters.OrbitVertical = EditorGUILayout.Slider("Orbit Vertical", thisTarget.EnterParameters.OrbitVertical, -90f, 90f);
                    thisTarget.EnterParameters.DeadZoneWidth = EditorGUILayout.Slider("Dead Zone Width", thisTarget.EnterParameters.DeadZoneWidth, 0f, 1f);
                    thisTarget.EnterParameters.DeadZoneHeight = EditorGUILayout.Slider("Dead Zone Height", thisTarget.EnterParameters.DeadZoneHeight, 0f, 1f);
                    thisTarget.EnterParameters.SoftZoneWidth = EditorGUILayout.Slider("Soft Zone Width", thisTarget.EnterParameters.SoftZoneWidth, 0f, 1f);
                    thisTarget.EnterParameters.SoftZoneHeight = EditorGUILayout.Slider("Soft Zone Height", thisTarget.EnterParameters.SoftZoneHeight, 0f, 1f);
                }

                // Update Camera Parameters
                if (GUI.changed)
                {
                    Cinema.SwitchPriority(FindAnyObjectByType<ActorVirtualCamera>().GetComponent<CinemachineVirtualCameraBase>());

                    thisTarget.OnEnterState();
                }
            }
        }
    }
}
