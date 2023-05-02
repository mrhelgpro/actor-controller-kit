
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class Actor : MonoBehaviour
    {
        public string Name = "Actor";
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(Actor))]
    public class ActorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Actor component = (Actor)target;

            hideChildObjects(component.transform, isPrefab(component.gameObject));

            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("Name", component.Name);

                return;
            }

            component.Name = EditorGUILayout.TextField("Name", component.Name);
        }

        private void hideChildObjects(Transform transform, bool state)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.hideFlags = state == true ? HideFlags.HideInHierarchy : HideFlags.None;
            }
        }

        private bool isPrefab(GameObject gameObject)
        {
            PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(gameObject);
            return assetType == PrefabAssetType.Regular || assetType == PrefabAssetType.Variant;
        }
    }
#endif
}

/*
    EXAMPLE: GameObject will be hidden in the inspector window
    GameObject childGameObject = EditorUtility.CreateGameObjectWithHideFlags("New Child GameObject", HideFlags.HideInHierarchy | HideFlags.HideInInspector);

    HideFlags.HideInHierarchy and HideFlags.HideInInspector - are two of the values 
    that can be set for the flags parameter of the EditorUtility.CreateGameObjectWithHideFlags() method.

    HideFlags.HideInHierarchy - When this flag is set, the GameObject will be hidden in the hierarchy window, which means 
    it will not be visible in the list of objects that are on the stage.

    HideFlags.HideInInspector - When this flag is checked, the GameObject will be hidden in the inspector window, which means 
    its components will not be displayed in the inspector window.


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
    EditorGUILayout.ObjectField("Model", MonoScript.FromMonoBehaviour(_myTarget), typeof(MonoScript), false);
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