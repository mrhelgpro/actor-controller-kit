using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace Actormachine
{
    public enum StorageType { Hide, Inventory, Hand }
    public enum UseType { Storage, Hand }

    public class ItemProperty : Property, IInteractable
    {
        public StorageType StorageType = StorageType.Hide;
        public UseType UseType = UseType.Storage;

        private List<State> _childStates = new List<State>();

        private Transform _inventoryTransform;
        private Transform _handTransform;

        private bool _isTemporary = false;

        private Collider _collider;
        private Collider2D _collider2D;
        private Rigidbody _rigidbody;
        private Rigidbody2D _rigidbody2D;
        private NavMeshAgent _navMeshAgent;

        public bool IsAvailable(Transform rootTransform)
        {
            if (StorageType == StorageType.Hand)
            {
                return rootTransform.GetComponentInChildren<Handable>() != null;
            }

            if (UseType == UseType.Hand)
            {
                if (rootTransform.GetComponentInChildren<Handable>() == null)
                {
                    return false;
                }
            }

            return rootTransform.GetComponentInChildren<Inventoriable>() != null;
        }

        public override void OnEnableState()
        {
            _isTemporary = false;

            Inventoriable inventoriable = FindRootTransform.GetComponentInChildren<Inventoriable>();

            if (inventoriable)
            {
                _inventoryTransform = inventoriable.transform;
            }

            Handable handable = FindRootTransform.GetComponentInChildren<Handable>();

            if (handable)
            {
                _handTransform = handable.transform;
            }

            setPlacementInStorage();

            // Child states
            _childStates.Clear();

            foreach (State state in GetComponentsInChildren<State>()) _childStates.Add(state);

            // Item Settings
            onEnableItem();
        }

        public override void OnEnterState()
        {
            setPlacementForUse();
        }

        public override void OnInactiveState()
        {     
            if (StorageType == StorageType.Hand)
            {
                if (_isTemporary == false)
                {
                    _isTemporary = true;
                    actor.Activate(state);
                }
            }

            // Check if at least one child state is active
            foreach (State state in _childStates)
            {
                if (state.IsActive == true) return;
            }

            // If child states are not active
            if (StorageType == StorageType.Hand)
            {
                setDisableTemporary();
            }
            else
            {
                setPlacementInStorage();
            }
        }

        public override void OnDisableState()
        {
            onDisableItem();
        }

        private void setDisableTemporary()
        {
            foreach (State state in _childStates) actor.Remove(state); //state.OnDisableState();

            ThisTransform.parent = null;
        }

        private void setPlacementInStorage()
        {
            if (StorageType == StorageType.Hide)
            {
                gameObject.SetActive(false);
                ThisTransform.parent = _inventoryTransform;
            }
            else if (StorageType == StorageType.Inventory)
            {
                gameObject.SetActive(true);
                ThisTransform.parent = _inventoryTransform;
                ThisTransform.localPosition = Vector3.zero;
                ThisTransform.localEulerAngles = Vector3.zero;
            }
            else if (StorageType == StorageType.Hand)
            {
                gameObject.SetActive(true);
                ThisTransform.parent = _handTransform;
                ThisTransform.localPosition = Vector3.zero;
                ThisTransform.localEulerAngles = Vector3.zero;
            }
        }

        private void setPlacementForUse()
        {
            if (UseType == UseType.Storage)
            {
                setPlacementInStorage();
            }
            else if (UseType == UseType.Hand)
            {
                gameObject.SetActive(true);
                ThisTransform.parent = _handTransform;
                ThisTransform.localPosition = Vector3.zero;
                ThisTransform.localEulerAngles = Vector3.zero;
            }
        }

        private void onEnableItem()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();

            _collider2D = GetComponent<Collider2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _navMeshAgent = GetComponent<NavMeshAgent>();

            if (_collider) _collider.enabled = false;
            if (_rigidbody) _rigidbody.isKinematic = true;

            if (_collider2D) _collider2D.enabled = false;
            if (_rigidbody2D) _rigidbody2D.isKinematic = true;

            if (_navMeshAgent) _navMeshAgent.enabled = false;
        }

        private void onDisableItem()
        {
            if (_collider) _collider.enabled = true;
            if (_rigidbody) _rigidbody.isKinematic = false;

            if (_collider2D) _collider2D.enabled = true;
            if (_rigidbody2D) _rigidbody2D.isKinematic = false;

            if (_navMeshAgent) _navMeshAgent.enabled = true;
        }
    }
}