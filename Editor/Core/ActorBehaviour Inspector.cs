using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    public enum BoxStyle { Default, Error, Warning, Active }
    public enum ButtonStyle { Main, Active, Default }

    [ExecuteInEditMode]
    [CustomEditor(typeof(ActorBehaviour))]
    public class ActorBehaviourInspector : UnityEditor.Editor
    {
        Color Dark = new Color(0.22f, 0.22f, 0.22f, 1f);
        Color Gray = new Color(0.75f, 0.75f, 0.75f, 1f);
        Color Red = new Color(0.85f, 0.38f, 0.35f, 1f);
        Color Yellow = new Color(0.90f, 0.80f, 0.48f, 1f);
        Color Blue = new Color(0.45f, 0.70f, 0.85f, 1f);
        Color Green = new Color(0.50f, 0.80f, 0.50f, 1f);
        Color LightGreen = new Color(0.85f, 0.95f, 0.85f, 1f);

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

        ///<summary> Checking for a single instance on Scene. </summary>
        public bool CheckSingleInstanceOnScene<T>() where T : Component
        {
            T requireComponent = GameObject.FindAnyObjectByType<T>();

            if (requireComponent == null)
            {
                DrawModelBox("<" + typeof(T).ToString() + "> is not found", BoxStyle.Error);

                return true;
            }

            return false;
        }

        ///<summary> Checking for a single instance on gameObject and destroy duplicates. </summary>
        public bool CheckSingleInstanceOnObject<T>(GameObject gameObject) where T : Component
        {
            if (gameObject.IsSingleInstanceOnObject<T>() == false)
            {
                T[] requireComponent = gameObject.GetComponents<T>();

                DestroyImmediate(requireComponent[1]);

                return false;
            }

            return true;
        }

        ///<summary> Checking for a single instance in children and destroy duplicates. </summary>
        public bool CheckSingleInstanceInChildren<T>(GameObject gameObject) where T : Component
        {
            if (gameObject.IsSingleInstanceInChildren<T>() == false)
            {
                T[] requireComponent = gameObject.GetComponentsInChildren<T>();

                DestroyImmediate(requireComponent[1]);

                return false;
            }

            return true;
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

        public void DrawLinkButton(string name, GameObject gameObject, ButtonStyle buttonStyle = ButtonStyle.Default)
        {
            Color backgroundColor = Gray;
            Color textColor = Dark;
            float height = 20;
            int fontSize = 12;

            if (buttonStyle == ButtonStyle.Main)
            {
                backgroundColor = Blue;
                textColor = Dark;
                height = 30;
                fontSize = 16;
            }
            else if (buttonStyle == ButtonStyle.Active)
            {
                backgroundColor = Green;
                textColor = LightGreen;
                height = 20;
                fontSize = 12;
            }
            else if (buttonStyle == ButtonStyle.Default)
            {
                backgroundColor = Gray;
                textColor = Dark;
                height = 20;
                fontSize = 12;
            }

            GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.normal.background = buttonTexture(2, 2, backgroundColor);
            guiStyle.fontSize = fontSize;
            guiStyle.fontStyle = FontStyle.Bold;
            guiStyle.alignment = TextAnchor.MiddleCenter;
            guiStyle.normal.textColor = textColor;

            EditorGUILayout.Space();

            if (GUILayout.Button(name, guiStyle, GUILayout.ExpandWidth(true), GUILayout.Height(height)))
            {
                EditorGUIUtility.PingObject(gameObject);
            }
        }

        public void DrawModelBox(string info, BoxStyle boxStyle = BoxStyle.Default)
        {
            Color backgroundColor = Gray;
            Color textColor = Dark;

            if (boxStyle == BoxStyle.Error)
            {
                backgroundColor = Red;
                textColor = Dark;
            }
            else if (boxStyle == BoxStyle.Warning)
            {
                backgroundColor = Yellow;
                textColor = Dark;
            }
            else if (boxStyle == BoxStyle.Active)
            {
                backgroundColor = Green;
                textColor = LightGreen;
            }

            drawBox(info, backgroundColor, textColor);
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

        private Texture2D buttonTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }
    }
}