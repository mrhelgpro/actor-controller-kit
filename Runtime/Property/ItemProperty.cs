using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace Actormachine
{
    public enum StorageType { HideInStorage, AddToInventory, TakeAndActivate }
    public enum ActiveType { StorageType, ActiveSlot }
    public enum ReplacementType { HideInStorage, RemoveFromInventory }

    [AddComponentMenu("Actormachine/Property/Item Property")]
    public class ItemProperty : Property, IInteractable
    {
        public StorageType StorageType = StorageType.HideInStorage;
        public ActiveType ActiveType = ActiveType.StorageType;
        public ReplacementType ReplacementType = ReplacementType.RemoveFromInventory;
        public int InventorySlotNumber = 0;
        public int ActiveSlotNumber = 0;

        private List<State> _childStates = new List<State>();

        private Transform _inventorySlot;
        private Transform _activeSlot;

        private bool _isTemporary = false;
        private bool _isEnter = false;

        private Storagable _storagable;

        private Collider _collider;
        private Collider2D _collider2D;
        private Rigidbody _rigidbody;
        private Rigidbody2D _rigidbody2D;
        private NavMeshAgent _navMeshAgent;

        // Property Methods
        public override void OnEnableState()
        {
            Debug.Log(gameObject.name + " 1.1 - OnEnableState ---------------------------");

            _childStates.Clear();
            _isTemporary = false;

            _storagable = FindRootTransform.GetComponentInChildren<Storagable>();

            if (StorageType == StorageType.HideInStorage)
            {
                _inventorySlot = _storagable.transform;
            }

            if (StorageType == StorageType.AddToInventory)
            {
                _inventorySlot = _storagable.GetInventorySlot(InventorySlotNumber);
            }

            if (StorageType == StorageType.TakeAndActivate || ActiveType == ActiveType.ActiveSlot)
            {
                _activeSlot = _storagable.GetActiveSlot(ActiveSlotNumber);
            }

            setPlacementInStorage();

            // Add all child States
            foreach (State state in GetComponentsInChildren<State>()) _childStates.Add(state);

            // Item Settings
            onEnableItem();
        }

        public override void OnEnterState()
        {
            setPlacementOnActive();

            _isEnter = true;
        }

        public override void OnInactiveState()
        {
            if (StorageType == StorageType.TakeAndActivate)
            {
                if (_isTemporary == false)
                {
                    _isTemporary = true;
                    actor.Activate(state);
                }

                if (_isEnter == true)
                {
                    // Check if at least one child state is active
                    foreach (State state in _childStates)
                    {
                        if (state.IsActive == true) return;
                    }

                    // If child states are not active
                    SetDisableItem();
                }
            }
            else
            {
                setPlacementInStorage();
            }
        }

        public override void OnExitState()
        {
            //_isEnter = false;
        }

        public override void OnDisableState()
        {
            _isEnter = false;
            onDisableItem();
        }

        public void SetDisableItem(Transform parent = null)
        {
            foreach (State state in _childStates) actor.Remove(state);

            if (parent != null)
            {
                ThisTransform.gameObject.SetActive(false);
                ThisTransform.parent = _storagable.transform;
            }

            ThisTransform.parent = parent;
        }

        // Item Methods
        public bool IsAvailable(Transform rootTransform)
        {
            Storagable storagable = rootTransform.GetComponentInChildren<Storagable>();

            if (storagable == null)
            {
                return false;
            }

            if (StorageType == StorageType.AddToInventory)
            {
                if (storagable.InventorySlots.Count <= InventorySlotNumber) return false;
            }

            if (ActiveType == ActiveType.ActiveSlot)
            {
                if (storagable.ActiveSlots.Count <= ActiveSlotNumber) return false;
            }

            return true;
        }

        private void setPlacementInStorage()
        {
            if (StorageType == StorageType.HideInStorage)
            {
                SetDisableItem(_storagable.transform);
            }
            else if (StorageType == StorageType.AddToInventory)
            {
                Transform previousItem = _storagable.GetInventoryItem(InventorySlotNumber);

                if (previousItem != null && previousItem != ThisTransform)
                {
                    if (ReplacementType == ReplacementType.HideInStorage)
                    {
                        previousItem.GetComponent<ItemProperty>().SetDisableItem(_storagable.transform);
                    }
                    else
                    {
                        previousItem.GetComponent<ItemProperty>().SetDisableItem(null);
                    }
                }

                setPosition(_inventorySlot, true);
            }
            else if (StorageType == StorageType.TakeAndActivate)
            {
                setPosition(_activeSlot, true);
            }
        }

        private void setPlacementOnActive()
        {
            if (ActiveType == ActiveType.StorageType)
            {
                setPlacementInStorage();
            }
            else if (ActiveType == ActiveType.ActiveSlot)
            {
                setPosition(_activeSlot, true);
            }
        }

        private void setPosition(Transform parent, bool activeState)
        {
            gameObject.SetActive(activeState);
            ThisTransform.parent = parent;
            ThisTransform.localPosition = Vector3.zero;
            ThisTransform.localEulerAngles = Vector3.zero;
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