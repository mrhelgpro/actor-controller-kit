using System.Collections.Generic;
using UnityEditorInternal;
using UnityEditor;
using UnityEngine;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Actor))]
    public sealed class ActorInspector : ActorBehaviourInspector
    {
        Actor thisTarget;

        private void OnEnable()
        {
            thisTarget = (Actor)target;

            if (thisTarget)
            {
                // Checking for a single instance in children and destroy duplicates
                if (thisTarget.gameObject.CheckSingleInstanceInChildren<Actor>() == false) return;

                //Give all child objects the "Actror" layer
                thisTarget.SetActorLayer(thisTarget.transform);


                // Move Component To Root
                Component component = thisTarget;
                ComponentUtility.MoveComponentUp(component);

                moveToRootTransform();
            }
        }

        public override void OnInspectorGUI()
        {
            thisTarget = (Actor)target;

            if (Application.isPlaying)
            {
                Inspector.DrawHeader(thisTarget.Name);

                // Check State Machine
                List<State> statesList = thisTarget.GetStatesList;

                foreach (State state in statesList)
                {
                    bool isStateActive = thisTarget.IsCurrentState(state) == true;
                    BoxStyle style = isStateActive == true ? BoxStyle.Active : BoxStyle.Default;
                    Inspector.DrawModelBox(state.gameObject.name, style);
                }

                EditorUtility.SetDirty(target);
            }
            else
            {
                thisTarget.Name = EditorGUILayout.TextField("Name", thisTarget.Name);
            }
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