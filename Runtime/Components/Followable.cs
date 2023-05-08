using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    /// <summary> Model - to control the Camera Parameters. </summary>
    public class Followable : ActorComponent
    {
        public VirtualCamera VirtualCamera;
        public void Enter(CameraParameters enterParameters) => VirtualCamera.Enter(transform, enterParameters);
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(Followable))]
    public class FollowableEditor : ModelEditor
    {
        public override void OnInspectorGUI()
        {   
            Followable thisTarget = (Followable)target;
            
            serializedObject.Update();
            SerializedProperty virtualCameraProp = serializedObject.FindProperty("VirtualCamera");

            // Check VirtualCamera
            if (thisTarget.VirtualCamera == null)
            {
                DrawModelBox("Virtual Camera must be added", BoxStyle.Error);
                EditorGUILayout.PropertyField(virtualCameraProp);
            }
            else
            {
                DrawModelBox("Edited in the Presenter");
                thisTarget.VirtualCamera.gameObject.name = "Virtual Camera (" + thisTarget.FindRootTransform.name + ")";
                thisTarget.VirtualCamera.Enter(thisTarget.transform, thisTarget.VirtualCamera.Parameters);

                // Draw Button
                GUILayout.FlexibleSpace(); 
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Detach Virtual Camera", GUILayout.Width(160), GUILayout.Height(18)))
                {
                    thisTarget.VirtualCamera.gameObject.name = "Virtual Camera (Null)";
                    thisTarget.VirtualCamera.Enter(null, thisTarget.VirtualCamera.Parameters);
                    thisTarget.VirtualCamera = null;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}