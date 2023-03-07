using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public class Actionable
    {
        public bool IsAction(Action action) => _action == action;
        public string GetName => _action.gameObject.name + " - " + _action.Name;

        private Action _action = null;
        private List<Action> _actions = new List<Action>();

        public void AddActionToPool(GameObject objectAction)
        {
            // Check the Action list for an equal
            foreach (Action item in _actions)
            {
                // If it found an equal Action, destroy this GameObject
                if (objectAction.name == item.gameObject.name)
                {
                    UnityEngine.Object.Destroy(objectAction);

                    return;
                }
            }

            // If no equal Action is found, add it to the action pool
            _actions.Add(objectAction.GetComponent<Action>());
        }

        public void Activate(GameObject objectAction, Action.EnumType type)
        {
            // Check the Actions list for a ready-made Action
            foreach (Action item in _actions)
            {
                // If you find an equal GameObject Name, execute this Action
                if (objectAction.name == item.gameObject.name)
                {
                    InvokeActivate(objectAction, type);

                    return;
                }
            }

            // If you don't find an equal Action, create a new one
            CreateAction(objectAction, type);
        }

        public void Deactivate(GameObject objectAction)
        {
            Action action = objectAction.GetComponent<Action>();

            // At the end of the Action, remove it from the Actor
            if (action == _action) _action = null;

            // If the Action is of type Uncanceling, make the GameObject inactive
            if (action.Type == Action.EnumType.Uncanceling) action.gameObject.SetActive(false);
        }

        // If the Action is empty, we can activate any other type
        // If the Action is of type Controller, we can replace it with any type other than Controller
        // If the Action is of a different type, only the Cancel type can replace it 
        private bool _isReady => _isEmpty ? true : _action.Type == Action.EnumType.Controller ? _action.Type != Action.EnumType.Controller : _action.Type == Action.EnumType.Canceling;
        private bool _isEmpty => _action == null;
        private void InvokeActivate(GameObject objectAction, Action.EnumType type)
        {
            Action action = objectAction.GetComponent<Action>();

            action.Type = type;

            // If the Action is ready for execution, make it active
            if (_isReady)
            {
                _action = action;
                _action.gameObject.SetActive(true);
            }

            // If the Action is of type Uncanceling, then we do not replace the executing Action, but the one executed in the Check method 
            if (type == Action.EnumType.Uncanceling)
            {
                action.gameObject.SetActive(true);
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
    }

    public abstract class Action : MonoBehaviour
    {
        public enum EnumType { Controller, Interaction, Canceling, Uncanceling };

        public string Name = "Action";
        public EnumType Type;
        protected Inputable Input => _actor.Input;

        private Actionable _actionable => _actor.Actionable;
        private Actor _actor;

        private void Awake()
        {
            _actor = GetComponentInParent<Actor>();
            _actionable.AddActionToPool(gameObject);

            Initialization();
        }

        public void Activate() => _actionable.Activate(gameObject, Type);
        public abstract void Initialization();
        public abstract void CheckLoop();
        public abstract void UpdateLoop();
        public abstract void FixedLoop();
        public void Deactivate() => _actionable.Deactivate(gameObject);

        private void Update()
        {
            if (_actionable.IsAction(this) == true)
            {
                UpdateLoop();
            }
            else
            {
                CheckLoop();
            }
        }

        private void FixedUpdate()
        {
            if (_actionable.IsAction(this) == true) FixedLoop();
        }

        private void OnDisable() => Deactivate();
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
        public EnumMode Mode => _actor.Mode;

        private Actor _actor;

        private void Awake() => _actor = GetComponentInParent<Actor>();
    }
}