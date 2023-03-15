using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public enum ActionType { Controller, Interaction, Forced, Irreversible };

    public class Actionable : MonoBehaviour
    {
        public string GetName => _currentAction.gameObject.name + " - " + _currentAction.Name;
        public ActionBehaviour GetAction => _currentAction;
        public bool IsController => _currentAction == null ? false : _currentAction.GetType == ActionType.Controller;
        public bool IsInteraction => _currentAction == null ? false : _currentAction.GetType == ActionType.Interaction;
        public bool IsForced => _currentAction == null ? false :_currentAction.GetType == ActionType.Forced;
        public bool IsIrreversible => _currentAction == null ? false : _currentAction.GetType == ActionType.Irreversible;

        private ActionBehaviour _currentAction = null;
        private List<ActionBehaviour> _actions = new List<ActionBehaviour>();

        private void Update()
        {
            foreach (ActionBehaviour item in _actions)
            {
                if (item != _currentAction)
                {
                    item.WaitLoop();
                }
            }

            if (_currentAction) _currentAction.UpdateLoop();
        }

        private void FixedUpdate()
        {
            if (_currentAction) _currentAction.FixedLoop();
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
                    Destroy(objectAction);

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
        private bool _isReady(ActionBehaviour action)
        {
            bool ready = false;

            if (_isEmpty == true)
            {
                ready = true;
            }
            else
            {
                if (IsIrreversible == false)
                {
                    ready = IsController ? action.GetType != ActionType.Controller : action.GetType == ActionType.Forced;
                }
            }

            return ready;
        }
        private bool _isEmpty => _currentAction == null;
        private void InvokeActivate(GameObject objectAction)
        {
            ActionBehaviour action = objectAction.GetComponent<ActionBehaviour>();

            if (_isReady(action))
            {
                if (_currentAction != null) _currentAction.Exit();

                _currentAction = action;
                _currentAction.gameObject.SetActive(true);
                _currentAction.Enter();
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

            if (action == _currentAction)
            {
                _currentAction.Exit();
                _currentAction = null;
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