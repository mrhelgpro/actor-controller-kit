using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class Actionable : MonoBehaviour
    {
        public string GetName => _action.gameObject.name + " - " + _action.Name;
        public ActionBehaviour GetAction => _action;

        private ActionBehaviour _action = null;
        private List<ActionBehaviour> _actions = new List<ActionBehaviour>();

        private void Update()
        {
            foreach (ActionBehaviour item in _actions)
            {
                if (item != _action)
                {
                    item.WaitLoop();
                }
            }

            if (_action) _action.UpdateLoop();
        }

        private void FixedUpdate()
        {
            if (_action) _action.FixedLoop();
        }

        // Check the Action list for an equal
        // If it found an equal Action, destroy this GameObject
        // If no equal Action is found, add it to the action pool
        public void AddActionToPool(GameObject objectAction)
        {
            foreach (ActionBehaviour item in _actions)
            {
                if (objectAction.name == item.gameObject.name)
                {
                    UnityEngine.Object.Destroy(objectAction);

                    return;
                }
            }

            _actions.Add(objectAction.GetComponent<ActionBehaviour>());
        }

        // Check the Actions list for a ready-made Action
        // If you find an equal GameObject Name, execute this Action
        // If you don't find an equal Action, create a new one
        public void Activate(GameObject objectAction)
        {
            foreach (ActionBehaviour item in _actions)
            {
                if (objectAction.name == item.gameObject.name)
                {
                    InvokeActivate(objectAction);

                    return;
                }
            }

            CreateAction(objectAction);
        }

        // If the Action is empty, we can activate any other type
        // If the Action is of type Controller, we can replace it with any type other than Controller
        // If the Action is of a different type, only the Cancel type can replace it 
        private bool _isReady(ActionBehaviour action) => _isEmpty ? true : _action.GetType == ActionType.Controller ? action.GetType != ActionType.Controller : action.GetType == ActionType.Canceling;
        private bool _isEmpty => _action == null;
        private void InvokeActivate(GameObject objectAction)
        {
            ActionBehaviour action = objectAction.GetComponent<ActionBehaviour>();

            //action.GetType = type;

            if (_isReady(action))
            {
                if (_action != null)
                {
                    _action.Exit();
                }

                _action = action;
                _action.gameObject.SetActive(true);
                _action.Enter();
            }
        }

        private void CreateAction(GameObject objectAction)
        {
            //GameObject instantiateAction = Instantiate(instantiateAction, new Vector3(0, 0, 0), Quaternion.identity);
            // instantiateAction.name = objectAction.name;
            //Instantiate(instantiateAction, new Vector3(0, 0, 0), Quaternion.identity);
            //Action action = instantiateAction.GetComponent<Action>();
            //AddActionToPool(action);
            //instantiateAction.SetActive(true);

            //InvokeActivate(objectAction, item, type);

            Debug.LogWarning("ADD INSTANTIATE");
        }

        // At the end of the Action, remove it from the Actor
        public void Deactivate(GameObject objectAction)
        {
            ActionBehaviour action = objectAction.GetComponent<ActionBehaviour>();

            if (action == _action)
            {
                _action.Exit();
                _action = null;
            }
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(Actionable))]
    public class ActionableEditor : Editor
    {
        private Actionable myTarget;

        public override void OnInspectorGUI()
        {
            myTarget = (Actionable)target;

            EditorGUILayout.LabelField("Action", myTarget.GetAction == null ? "None" : myTarget.GetName);
        }
    }
#endif
}