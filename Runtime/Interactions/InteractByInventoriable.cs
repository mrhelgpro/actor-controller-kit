using UnityEngine;

namespace Actormachine
{
    public enum InventoryType { Hide, Place }

    public class InteractByInventoriable : StateBehaviour, IEnableState, IInteractable
    {
        public InventoryType InventoryType = InventoryType.Hide;

        public bool IsAvailable(Transform rootTransform)
        {
            return rootTransform.GetComponentInChildren<Inventoriable>() != null;
        }

        public void OnEnableState()
        {
            Transform inventoryTransform = FindRootTransform.GetComponentInChildren<Inventoriable>().transform;
            
            ThisTransform.parent = inventoryTransform;
            
            if (InventoryType == InventoryType.Hide)
            {
                gameObject.SetActive(false);
            }
            else if (InventoryType == InventoryType.Place)
            {
                gameObject.SetActive(true);
                ThisTransform.localPosition = Vector3.zero;
                ThisTransform.localEulerAngles = Vector3.zero;
            }
        }
    }
}