
using UnityEditor.SceneManagement;
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
        protected GameObject gameObject;
        private Actor _actor;
        private bool _isPlaying;

        private void OnEnable()
        {
            _actor = (Actor)target;
            gameObject = _actor.gameObject;

            _actor.AddComponents();
        }

        private void OnDestroy()
        {
            if (_isPlaying == false)
            {
                if (_actor == null)
                {
                    if (gameObject)
                    {
                        RemoveComponents();
                        Debug.Log("Remove Components");
                    }
                }
            }

            if (Application.isPlaying == false) _isPlaying = false;
        }

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("Name", _actor.Name);
                _isPlaying = true;
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
            // EXAMPLE: Always updated by the inspector - EditorUtility.SetDirty(target);
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