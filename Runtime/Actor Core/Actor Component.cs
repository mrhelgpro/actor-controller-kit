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

            if (actor)
            {
                return actor.gameObject.GetComponentInChildren<T>() == null ? actor.gameObject.AddComponent<T>() : actor.gameObject.GetComponentInChildren<T>();
            }

            Debug.LogWarning(gameObject.name + " - is not found <Actor>");
            gameObject.SetActive(false);

            return null;
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(ActorComponent))]
    public class ModelEditor : Editor
    {
        public void DefaultModelStyle(string info)
        {
            EditorGUILayout.Space(2);

            Color backgroundColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            Color textColor = new Color(0.25f, 0.25f, 0.25f, 1f);

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