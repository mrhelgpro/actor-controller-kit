
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
        private Actor _actor = null;
        private StatePresenterMachine _statePresenterMachine = null;

        private void OnEnable()
        {
            _actor = (Actor)target;

            AddRequireComponent();
        }

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("Name", _actor.Name);
            }
            else
            {
                DrawDefaultInspector();
            }
        }

        private void AddRequireComponent()
        {
            _statePresenterMachine = _actor.gameObject.GetComponentInChildren<StatePresenterMachine>();

            if (_statePresenterMachine == null)
            {
                GameObject statePresenterMachineObject = new GameObject();
                _statePresenterMachine = statePresenterMachineObject.AddComponent<StatePresenterMachine>();
            }

            _statePresenterMachine.gameObject.name = "State Presenter Machine";
            _statePresenterMachine.transform.parent = _actor.transform;
        }

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

/*
EXAMPLE:
GameObject childGameObject = EditorUtility.CreateGameObjectWithHideFlags("New Child GameObject", HideFlags.HideInHierarchy | HideFlags.HideInInspector);

HideFlags.HideInHierarchy and HideFlags.HideInInspector - are two of the values 
that can be set for the flags parameter of the EditorUtility.CreateGameObjectWithHideFlags() method.

HideFlags.HideInHierarchy - When this flag is set, the GameObject will be hidden in the hierarchy window, which means 
it will not be visible in the list of objects that are on the stage.

HideFlags.HideInInspector - When this flag is checked, the GameObject will be hidden in the inspector window, which means 
its components will not be displayed in the inspector window.
*/