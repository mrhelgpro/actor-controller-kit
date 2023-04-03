using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public enum ActionType { Controller, Interaction, Forced, Irreversible };

    public class Actionable : MonoBehaviour
    {
        public string GetName => _currentAction == null ? "None" : _currentAction.gameObject.name + " - " + _currentAction.Name;
        public bool IsEmpty => _currentAction == null;

        private Action _currentAction = null;
        private List<Action> _actions = new List<Action>();
        private List<Activator> _activators = new List<Activator>();

        private void Awake()
        {
            foreach (Action action in GetComponentsInChildren<Action>()) _actions.Add(action);
            foreach (Activator activator in GetComponentsInChildren<Activator>()) _activators.Add(activator);
        }

        private void Update()
        {
            foreach (Activator item in _activators)
            {
                if (item.gameObject != _currentAction?.gameObject)
                {
                    item.UpdateActivate();
                }
            }

            _currentAction?.UpdateLoop();
        }

        private void FixedUpdate()
        {
            _currentAction?.FixedLoop();
        }

        // Check the Actions list for a ready-made Action
        // If you find an equal GameObject Name, execute this Action
        // If you don't find an equal Action, create a new one
        public void TryToActivate(GameObject objectAction)
        {
            foreach (Action item in _actions)
            {
                if (objectAction.name == item.gameObject.name)
                {
                    InvokeActivate(item.gameObject);

                    return;
                }
            }

            CreateAction(objectAction);
        }

        // If the Action is empty, we can activate any other type
        // If the Action is of type Controller, we can replace it with any type other than Controller
        // If the Action is of a different type, only the Cancel type can replace it 
        private bool _isController => _currentAction == null ? false : _currentAction.Type == ActionType.Controller;
        private bool _isIrreversible => _currentAction == null ? false : _currentAction.Type == ActionType.Irreversible;
        private bool _isReady(Action action)
        {
            bool ready = false;

            if (IsEmpty == true)
            {
                ready = true;
            }
            else
            {
                if (_isIrreversible == false)
                {
                    ready = _isController ? action.Type != ActionType.Controller : action.Type == ActionType.Forced;
                }
            }

            return ready;
        }
        private void InvokeActivate(GameObject objectAction)
        {
            Action action = objectAction.GetComponent<Action>();

            if (_isReady(action))
            {
                _currentAction?.Exit();
                _currentAction = action;
                _currentAction.Enter();

                Debug.Log(GetName);
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
            Action action = objectAction.GetComponent<Action>();

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
        public override void OnInspectorGUI()
        {
            Actionable myTarget = (Actionable)target;
            EditorGUILayout.LabelField("Action", myTarget.GetName);
            EditorUtility.SetDirty(target);
        }
    }
#endif
}

namespace AssemblyActorCore
{
    public class ActionEmptyExample : Action
    {
        public override void Enter() => movable.FreezRotation();

        public override void UpdateLoop()
        {

        }

        public override void FixedLoop()
        {

        }

        public override void Exit() => movable.FreezAll();
    }
}