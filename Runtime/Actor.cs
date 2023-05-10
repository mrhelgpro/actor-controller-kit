
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace AssemblyActorCore
{
    public class Actor : ActorBehaviour
    {
        public string Name = "Actor";
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(Actor))]
    public class ActorEditor : ModelEditor
    {
        Actor thisTarget;

        public override void OnInspectorGUI()
        {
            thisTarget = (Actor)target;

            if (Application.isPlaying)
            {
                DrawHeader(thisTarget.Name);

                // Check StatePresenterMachine
                StatePresenterMachine statePresenterMachine = thisTarget.gameObject.GetComponentInChildren<StatePresenterMachine>();
                if (statePresenterMachine != null)
                {
                    List<StatePresenter> statePresentersList = statePresenterMachine.GetStatePresentersList;

                    foreach (StatePresenter statePresenter in statePresentersList)
                    {
                        bool state = statePresenterMachine.IsCurrentStateObject(statePresenter.gameObject) == true;
                        BoxStyle style = state == true ? BoxStyle.Active : BoxStyle.Default;
                        DrawModelBox(statePresenter.gameObject.name, style);
                    }
                }
            }
            else
            {
                thisTarget.Name = EditorGUILayout.TextField("Name", thisTarget.Name);

                Component component = thisTarget;
                ComponentUtility.MoveComponentUp(component);

                checkRootTransform();

                if (GUI.changed)
                {
                    EditorUtility.SetDirty(thisTarget);
                }
            }
        }

        private void checkRootTransform()
        {
            GameObject root = thisTarget.FindRootTransform.gameObject;

            if (root != thisTarget.gameObject)
            {
                root.AddRequiredComponent<Actor>();
                thisTarget.gameObject.RemoveComponent<Actor>();
            }
        }
    }
#endif
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