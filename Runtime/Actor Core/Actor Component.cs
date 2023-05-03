using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public abstract class ActorComponent : MonoBehaviour
    {
        public Transform RootTransform { get; private set; }
        public Transform ThisTransform { get; private set; }


        protected void Awake()
        {
            Actor actor = GetComponentInParent<Actor>();

            RootTransform = actor == null ? transform : actor.transform;
            ThisTransform = transform;
        }

        /// <summary> Returns or creates the required component on the Actor. </summary>
        public T GetComponentInActor<T>() where T : Component
        {
            Actor actor = GetComponentInParent<Actor>();

            GameObject root = actor == null ? gameObject : actor.gameObject;

            return root.GetComponentInChildren<T>() == null ? root.AddComponent<T>() : root.GetComponentInChildren<T>();
        }
    }

    public static class ActorComponentExtention
    {
        // Finds the required Component on <Actor> gets or instantiates
        public static T AddRequiredComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() == null ? gameObject.AddComponent<T>() : gameObject.GetComponent<T>();
        }

        public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() != null) Object.DestroyImmediate(gameObject.GetComponent<T>());
        }
    }

#if UNITY_EDITOR
        [ExecuteInEditMode]
    [CustomEditor(typeof(ActorComponent))]
    public class ModelEditor : Editor
    {
        public bool IsActor(GameObject gameObject) => gameObject.GetComponentInParent<Actor>();

        public void ShowLink(MonoBehaviour monoScript)
        {
            // Show script Link
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(monoScript), typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            Rect scriptRect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(scriptRect, MouseCursor.Arrow);

            if (GUI.Button(scriptRect, "", GUIStyle.none))
            {
                UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(monoScript)), 0);
            }
        }

        public void DefaultModelStyle(string info)
        {
            drawBox(info, new Color(0.5f, 0.5f, 0.5f, 1f), new Color(0.25f, 0.25f, 0.25f, 1f));
        }

        public void ErrorMessage(string info, string name = "")
        {
            drawBox(info, new Color(0.8f, 0.4f, 0.4f, 1f), new Color(0.25f, 0.25f, 0.25f, 1f));

            Debug.LogWarning(name + info);
        }

        private void drawBox(string info, Color backgroundColor, Color textColor)
        {
            EditorGUILayout.Space(2);

            // Font Style
            GUIStyle fontStyle = new GUIStyle(EditorStyles.label);
            fontStyle.fontSize = 10;
            fontStyle.fontStyle = FontStyle.Bold;
            fontStyle.alignment = TextAnchor.MiddleCenter;
            fontStyle.normal.textColor = textColor;

            // Create a Texture2D object
            Texture2D backgroundTexture = new Texture2D(1, 1);
            backgroundTexture.SetPixel(0, 0, backgroundColor);
            backgroundTexture.Apply();

            // Create a Box
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.border = new RectOffset(8, 8, 8, 8);
            boxStyle.normal.background = backgroundTexture;

            EditorGUILayout.BeginVertical(boxStyle);
            EditorGUILayout.LabelField(info, fontStyle, GUILayout.Height(10), GUILayout.Width(80), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.EndVertical();
        }
    }
#endif
}