using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    [Serializable]
    public class Actionable
    {
        public bool IsAction(Action action) => _action == action;
        public string GetName => _action.gameObject.name + " - " + _action.Name;

        private Action _action = null;
        private List<Action> _actions = new List<Action>();

        public void WaitList()
        {
            foreach (Action item in _actions)
            {
                if (item != _action)
                {
                    item.WaitLoop();
                }
            }
        }

        public void UpdateLoop()
        {
            if (_action)
            {
                _action.UpdateLoop();
            }
        }

        public void FixedLoop()
        {
            if (_action)
            {
                _action.FixedLoop();
            }
        }

        // Check the Action list for an equal
        // If it found an equal Action, destroy this GameObject
        // If no equal Action is found, add it to the action pool
        public void AddActionToPool(GameObject objectAction)
        {    
            foreach (Action item in _actions)
            {
                if (objectAction.name == item.gameObject.name)
                {
                    UnityEngine.Object.Destroy(objectAction);

                    return;
                }
            }

            _actions.Add(objectAction.GetComponent<Action>());
        }

        // Check the Actions list for a ready-made Action
        // If you find an equal GameObject Name, execute this Action
        // If you don't find an equal Action, create a new one
        public void Activate(GameObject objectAction, Action.EnumType type)
        {   
            foreach (Action item in _actions)
            {  
                if (objectAction.name == item.gameObject.name)
                {
                    InvokeActivate(objectAction, type);

                    return;
                }
            }
            
            CreateAction(objectAction, type);
        }

        // If the Action is empty, we can activate any other type
        // If the Action is of type Controller, we can replace it with any type other than Controller
        // If the Action is of a different type, only the Cancel type can replace it 
        private bool _isReady(Action action) => _isEmpty ? true : _action.Type == Action.EnumType.Controller ? action.Type != Action.EnumType.Controller : action.Type == Action.EnumType.Canceling;
        private bool _isEmpty => _action == null;
        private void InvokeActivate(GameObject objectAction, Action.EnumType type)
        {
            Action action = objectAction.GetComponent<Action>();

            action.Type = type;

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

        private void CreateAction(GameObject objectAction, Action.EnumType type)
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

            if (action == _action)
            {
                _action.Exit();
                _action = null;
            }
        }
    }

    public abstract class Action : MonoBehaviour
    {
        public enum EnumType { Controller, Interaction, Canceling, Uncanceling };

        public string Name = "Action";
        public EnumType Type;

        protected Inputable input;
        protected Actionable actionable;

        private void Awake()
        {
            input = GetComponentInParent<Actor>().Input;
            actionable = GetComponentInParent<Actor>().Actionable;
            actionable.AddActionToPool(gameObject);

            Initialization();
        }

        protected abstract void Initialization();
        
        public abstract void WaitLoop();
        public abstract void Enter();
        public abstract void UpdateLoop();
        public abstract void FixedLoop();
        public abstract void Exit();
    }

    public class Inputable
    {
        public enum Key { None, Down, Press, Click, DoubleClick }

        public Key Menu = Key.None;

        [Header("Key-PAD")]
        public Key A = Key.None;
        public Key B = Key.None;
        public Key X = Key.None;
        public Key Y = Key.None;

        [Header("Stick")]
        public Vector3 Direction;
        public Vector3 Rotation;

        [Header("Trigger")]
        public Key LT = Key.None;
        public Key RT = Key.None;

        [Header("Bumper")]
        public Key LB = Key.None;
        public Key RB = Key.None;

        [Header("D-PAD")]
        public Key L = Key.None;
        public Key R = Key.None;
        public Key U = Key.None;
        public Key D = Key.None;
    }

    public abstract class Input : MonoBehaviour
    {
        public Inputable GetInput => _actor.Input;

        private Actor _actor;

        private void Awake() => _actor = GetComponentInParent<Actor>();
    }
}