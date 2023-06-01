using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public enum StatePriority { Default, Prepare, Action };

    [AddComponentMenu("Actormachine/Core/State")]
    public sealed class State : ActorBehaviour
    {
        public string Name => gameObject.name + " (" + Priority.ToString() + ")";
        public bool IsActive { get; private set; } = false;

        public StatePriority Priority = StatePriority.Default;

        private List<Property> _properties = new List<Property>();

        // State Methods
        public void OnEnableState()
        {
            // Clear lists
            _properties.Clear();

            // Add and Enable properties
            foreach (Property property in GetComponents<Property>()) _properties.Add(property);

            foreach (Property property in _properties)
            {
                property.Enable();
                property.OnEnableState();
            }
        }
        public void OnInactiveState()
        {
            foreach (Property property in _properties) property.OnInactiveState();
        }
        public void OnEnterState()
        {
            IsActive = true;

            foreach (Property property in _properties) property.OnEnterState();
        }
        public void OnActiveState()
        {
            foreach (Property property in _properties) property.OnActiveState();
        }
        public void OnFixedActiveState()
        {
            foreach (Property property in _properties) property.OnFixedActiveState();
        }
        public void OnExitState()
        {
            foreach (Property property in _properties) property.OnExitState();

            IsActive = false;
        }
        public void OnDisableState()
        {
            foreach (Property property in _properties) property.OnDisableState();

            IsActive = false;
        }
    }

    [RequireComponent(typeof(State))]
    public abstract class Property : ActorBehaviour
    {
        protected Actor actor;
        protected State state;

        public Transform RootTransform { get; private set; }

        public void Enable()
        {
            actor = GetComponentInParent<Actor>();
            state = GetComponent<State>();

            RootTransform = actor.transform;
        }

        public virtual void OnEnableState() { }
        public virtual void OnInactiveState() { }
        public virtual void OnEnterState() { }
        public virtual void OnActiveState() { }
        public virtual void OnFixedActiveState() { }
        public virtual void OnExitState() { }
        public virtual void OnDisableState() { }
    }

    /// <summary> To activate if State is not activated. </summary>
    public abstract class Activator : Property
    {
        private bool _currentAvailable = false;
        public bool IsAvailable() => _currentAvailable;

        public override void OnEnterState()
        {
            _currentAvailable = true;
        }

        public void TryActive(bool value)
        {
            if (value != _currentAvailable)
            {
                _currentAvailable = value;

                if (_currentAvailable == true)
                {
                    checkAllAvailable();
                }
            }
        }

        private void checkAllAvailable()
        {
            int amountOfAvailable = 0;

            Activator[] activators = GetComponents<Activator>();

            foreach (Activator activator in activators) amountOfAvailable += activator.IsAvailable() ? 1 : 0;

            if (amountOfAvailable == activators.Length) actor.Activate(state);
        }
    }

    /// <summary> To deactivate if State is active. </summary>
    public abstract class Deactivator : Property
    {
        public void Deactivate() => actor.Deactivate(state);
    }

    /// <summary> Mark for the ability to interact. </summary>
    public interface IInteractable
    {
        /// <summary> Checks if the object is available for interaction. </summary>
        public bool IsAvailable(Transform rootTransform);
    }
}