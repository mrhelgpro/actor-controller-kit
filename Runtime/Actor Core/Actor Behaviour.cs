using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    /// <summary> All classes that are part of the Actor must inherit from this class. </summary>
    public abstract class ActorBehaviour : MonoBehaviour
    {
        public Transform RootTransform { get; private set; }
        public Transform ThisTransform { get; private set; }

        protected void Awake()
        {
            RootTransform = FindRootTransform;
            ThisTransform = transform;
        }

        /// <summary> Returns or creates the required component on the Actor. </summary>
        public T GetComponentInRoot<T>() where T : Component
        {
            GameObject root = FindRootTransform.gameObject;

            return root.GetComponentInChildren<T>() == null ? root.AddComponent<T>() : root.GetComponentInChildren<T>();
        }

        /// <summary> Finds the highest object in the hierarchy that contains "ActorBehaviour" and uses it as a marker for the root object. </summary>
        public Transform FindRootTransform
        {
            get 
            {
                ActorBehaviour actorBehaviour = this;

                Transform rootTransform = transform;

                while (rootTransform.parent != null)
                {
                    rootTransform = rootTransform.parent;

                    if (rootTransform.GetComponent<ActorBehaviour>())
                    {
                        actorBehaviour = rootTransform.GetComponent<ActorBehaviour>();
                    }
                }

                return actorBehaviour.transform;
            }
        }
    }

    public static class ActorBehaviourExtention
    {
        public static T AddRequiredComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() == null ? gameObject.AddComponent<T>() : gameObject.GetComponent<T>();
        }

        public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() != null) Object.DestroyImmediate(gameObject.GetComponent<T>());
        }
    }

    public enum BoxStyle { Default, Error, Warning, Active }
#if UNITY_EDITOR
        [ExecuteInEditMode]
    [CustomEditor(typeof(ActorBehaviour))]
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

        public void DrawHeader(string info, int fontSize = 16)
        {
            // Font Style
            GUIStyle fontStyle = new GUIStyle(EditorStyles.label);
            fontStyle.fontSize = fontSize;
            fontStyle.fontStyle = FontStyle.Bold;
            fontStyle.alignment = TextAnchor.MiddleCenter;

            EditorGUILayout.LabelField(info, fontStyle, GUILayout.Height(20), GUILayout.Width(80), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        }

        public void DrawModelBox(string info, BoxStyle boxStyle = BoxStyle.Default)
        {
            Color backgroundColor = new Color(0.75f, 0.75f, 0.75f, 1f);
            Color textColor = new Color(0.22f, 0.22f, 0.22f, 1f);
            
            if (boxStyle == BoxStyle.Error)
            {
                backgroundColor = new Color(0.85f, 0.38f, 0.35f, 1f);
                textColor = new Color(0.22f, 0.22f, 0.22f, 1f);
            }
            else if (boxStyle == BoxStyle.Warning)
            {
                backgroundColor = new Color(0.90f, 0.80f, 0.48f, 1f);
                textColor = new Color(0.22f, 0.22f, 0.22f, 1f);
            }
            else if (boxStyle == BoxStyle.Active)
            {
                backgroundColor = new Color(0.5f, 0.8f, 0.5f, 1f);
                textColor = new Color(0.85f, 0.95f, 0.85f, 1f);
            }

            drawBox(info, backgroundColor, textColor);
        }

        public bool CheckBootstrap<T>() where T : Component
        { 
            T requireComponent = GameObject.FindAnyObjectByType<T>();

            if (requireComponent == null)
            {
                DrawModelBox("<" + typeof(T).ToString() + "> is not found", BoxStyle.Error);

                return true;
            }

            return false;
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