using System.Collections.Generic;
using UnityEditorInternal;
using UnityEditor;
using UnityEngine;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Actor))]
    [CanEditMultipleObjects]
    public sealed class ActorInspector : ActormachineBaseInspector
    {
        Actor thisTarget;

        private void OnEnable()
        {
            thisTarget = (Actor)target;

            if (thisTarget)
            {
                // Checking for a single instance in children and destroy duplicates
                if (thisTarget.gameObject.CheckSingleInstanceInChildren<Actor>() == false) return;

                //Give object the "Actror" layer
                thisTarget.gameObject.layer = LayerMask.NameToLayer("Actor");

                // Move Component To Root
                ComponentUtility.MoveComponentUp(thisTarget);
                moveToRootTransform();

                // Set Igore Collision
                int actorLayer = LayerMask.NameToLayer("Actor");
                Physics.IgnoreLayerCollision(actorLayer, actorLayer);
            }
        }

        public override void OnInspectorGUI()
        {
            thisTarget = (Actor)target;

            // Draw in Edit mode
            if (Application.isPlaying == false)
            {
                base.OnInspectorGUI();

                return;
            }

            // Draw in Play mode
            Inspector.DrawHeader(thisTarget.Name);

            foreach (State state in thisTarget.GetStatesList)
            {
                BoxStyle style = state.IsActive == true ? BoxStyle.Active : BoxStyle.Default;
                Inspector.DrawInfoBox(state.Name, style);
            }

            EditorUtility.SetDirty(target);
        }

        /// <summary> Checks that the Actor is always on Root Transform. </summary>
        private void moveToRootTransform()
        {
            GameObject root = thisTarget.FindRootTransform.gameObject;

            if (root != thisTarget.gameObject)
            {
                root.AddRequiredComponent<Actor>();
                thisTarget.gameObject.RemoveComponent<Actor>();
            }
        }
    }
}

/*
    // How to hide some fields
    private bool foldoutInput = false;

    GUILayout.BeginVertical();
    foldoutInput = EditorGUILayout.Foldout(foldoutInput, "Foldout");
    if (foldoutInput)
    {
        EditorGUILayout.LabelField("Your ad could be here");
    }
    GUILayout.EndVertical();

    // To show a property
    EditorGUILayout.PropertyField(new SerializedObject(target).FindProperty("Input")); 

    // Always updated by the inspector
    EditorUtility.SetDirty(target);

    // Show script Link
    EditorGUI.BeginDisabledGroup(true);
    EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_myTarget), typeof(MonoScript), false);
    EditorGUI.EndDisabledGroup();

    Rect scriptRect = GUILayoutUtility.GetLastRect();
    EditorGUIUtility.AddCursorRect(scriptRect, MouseCursor.Arrow);

    if (GUI.Button(scriptRect, "", GUIStyle.none))
    {
        UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(_myTarget)), 0);
    }

    // Text Color = Red
    new GUIStyle() { normal = new GUIStyleState() { textColor = Color.red } }

    // Font Style = Bold;
    GUIStyle style = new GUIStyle(GUI.skin.label);
    style.fontStyle = FontStyle.Bold;
    EditorGUILayout.LabelField("My Component", style);
*/