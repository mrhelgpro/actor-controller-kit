using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class Actionable : Model
    {
        public string GetName => _currentAction == null ? "None" : _currentAction.gameObject.name + " - " + _currentAction.Name;
        public bool IsEmpty => _currentAction == null;

        private Action _currentAction = null;
        private List<Action> _actions = new List<Action>();
        private List<Activator> _activators = new List<Activator>();

        private new  void Awake()
        {
            base.Awake();

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
        public void TryToActivate(Transform objectAction)
        {
            foreach (Action item in _actions)
            {
                if (objectAction.name == item.transform.name)
                {
                    InvokeActivate(item.transform);

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
            if (IsEmpty == true)
            {
                return true;
            }
            else
            {
                if (_currentAction.Type != ActionType.Required)
                {
                    if (action.Type == ActionType.Required)
                    {
                        return true;
                    }

                    if (_isIrreversible == false)
                    {
                        return _isController ? action.Type != ActionType.Controller : action.Type == ActionType.Forced;
                    }
                }
            }

            return false;
        }
        private void InvokeActivate(Transform objectAction)
        {
            //Action action = objectAction.GetComponent<Action>();

            Action action = objectAction.GetComponent<Action>();

            if (_isReady(action))
            {
                _currentAction?.Exit();
                _currentAction = action;
                _currentAction.Enter();

                Debug.Log(GetName);
            }
        }

        private void CreateAction(Transform objectAction)
        {
            GameObject instantiateAction = Instantiate(objectAction.gameObject, mainTransform);
            instantiateAction.name = objectAction.name;
            instantiateAction.transform.localPosition = Vector3.zero;
            instantiateAction.transform.localRotation = Quaternion.identity;
            _actions.Add(instantiateAction.GetComponent<Action>());

            InvokeActivate(instantiateAction.transform);
        }

        // At the end of the Action, remove it from the Actor
        public void Deactivate(Transform objectAction)
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
            if (Application.isPlaying)
            {
                Actionable myTarget = (Actionable)target;
                EditorGUILayout.LabelField("Action", myTarget.GetName);
                EditorUtility.SetDirty(target);
            }
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