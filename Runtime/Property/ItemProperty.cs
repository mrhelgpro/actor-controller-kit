using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace Actormachine
{
    public enum StorageType { HideInInventory, PrepareInSlot, TakeAndActivate }
    public enum ActiveType { StorageType, ActiveSlot }
    public enum ReplacementType { HideInInventory, RemoveFromInventory }

    public class ItemProperty : Property, IInteractable
    {
        public StorageType StorageType = StorageType.HideInInventory;
        public ActiveType ActiveType = ActiveType.StorageType;
        public ReplacementType ReplacementType = ReplacementType.RemoveFromInventory;
        public int InventorySlotNumber = 0;
        public int ActiveSlotNumber = 0;

        private List<State> _childStates = new List<State>();

        private Transform _inventorySlot;
        private Transform _activeSlot;

        private bool _isTemporary = false;

        private Storagable _storagable;

        private Collider _collider;
        private Collider2D _collider2D;
        private Rigidbody _rigidbody;
        private Rigidbody2D _rigidbody2D;
        private NavMeshAgent _navMeshAgent;

        public bool IsAvailable(Transform rootTransform)
        {
            Storagable storagable = rootTransform.GetComponentInChildren<Storagable>();

            if (storagable == null)
            {
                return false;
            }

            if (StorageType == StorageType.PrepareInSlot)
            {
                if (storagable.InventorySlots.Count <= InventorySlotNumber) return false;
            }

            if (ActiveType == ActiveType.ActiveSlot)
            {
                if (storagable.ActiveSlots.Count <= ActiveSlotNumber) return false;
            }

            return true;
        }

        public override void OnEnableState()
        {
            _childStates.Clear();
            _isTemporary = false;

            _storagable = FindRootTransform.GetComponentInChildren<Storagable>();

            if (StorageType == StorageType.HideInInventory)
            {
                _inventorySlot = _storagable.transform;
            }

            if (StorageType == StorageType.PrepareInSlot)
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
            }

            // Check if at least one child state is active
            foreach (State state in _childStates)
            {
                if (state.IsActive == true) return;
            }

            // If child states are not active
            if (StorageType == StorageType.TakeAndActivate)
            {
                SetDisableItem();
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

        private void setPlacementInStorage()
        {
            if (StorageType == StorageType.HideInInventory)
            {
                SetDisableItem(_storagable.transform);
            }
            else if (StorageType == StorageType.PrepareInSlot)
            {
                Transform previousItem = _storagable.GetInventoryItem(InventorySlotNumber);

                if (previousItem != null && previousItem != ThisTransform)
                {
                    if (ReplacementType == ReplacementType.HideInInventory)
                    {
                        previousItem.GetComponent<ItemProperty>().SetDisableItem(_storagable.transform);
                    }
                    else
                    {
                        previousItem.GetComponent<ItemProperty>().SetDisableItem(null);
                    }
                }

                gameObject.SetActive(true);
                ThisTransform.parent = _inventorySlot;
                ThisTransform.localPosition = Vector3.zero;
                ThisTransform.localEulerAngles = Vector3.zero;
            }
            else if (StorageType == StorageType.TakeAndActivate)
            {
                gameObject.SetActive(true);
                ThisTransform.parent = _activeSlot;
                ThisTransform.localPosition = Vector3.zero;
                ThisTransform.localEulerAngles = Vector3.zero;
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
                gameObject.SetActive(true);
                ThisTransform.parent = _activeSlot;
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