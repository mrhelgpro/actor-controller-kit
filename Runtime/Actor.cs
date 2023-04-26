
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class Actor : MonoBehaviour
    {
        public string Name = "Actor";

        public virtual void AddComponents() { }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(Actor))]
    public class ActorEditor : Editor
    {
        private Actor actor;
        protected GameObject gameObject;

        private void OnEnable()
        {
            actor = (Actor)target;
            gameObject = actor.gameObject;

            actor.AddComponents();
        }

        private void OnDisable()
        {
            if (actor == null)
            {
                if (gameObject)
                {
                    RemoveComponents();
                }
            }
        }

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("Name", actor.Name);
            }
            else
            {
                DrawDefaultInspector();
            }
        }

        public virtual void RemoveComponents() { }

        // Examples of showing fields
        private bool foldoutInput = false;
        private void Example()
        {
            // EXAMPLE: Example of how to hide some fields
            GUILayout.BeginVertical();
            foldoutInput = EditorGUILayout.Foldout(foldoutInput, "Foldout");
            if (foldoutInput)
            {
                EditorGUILayout.LabelField("Your ad could be here");
            }
            GUILayout.EndVertical();

            // EXAMPLE: To show a property - EditorGUILayout.PropertyField(new SerializedObject(target).FindProperty("Input")); 
        }
    }
#endif
}

// Add - using UnityEditor;
// Ctrl+H -> "Example" - "Replace Name" -> Alt+R
namespace AssemblyActorCore
{
    public class ActorExample : Actor
    {
        public override void AddComponents() 
        {
            gameObject.AddThisComponent<Inputable>();
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(ActorExample))]
    public class ActorExampleEditor : ActorEditor
    {
        public override void RemoveComponents()
        {
            gameObject.RemoveComponent<Inputable>();
        }
    }
#endif
}