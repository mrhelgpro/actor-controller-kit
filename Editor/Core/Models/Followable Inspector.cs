using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Followable))]
    [CanEditMultipleObjects]
    public class FollowableInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawSubtitle("MARK FOR CAMERA");
            /*
            // Draw a Inspector
            Followable thisTarget = (Followable)target;

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
            */
        }
    }
}