using UnityEngine;

namespace Actormachine
{
    public class InteractByHandable : StateBehaviour, IEnableState, IEnterState, IExitState, IInteractable
    {
        public bool IsAvailable(Transform rootTransform)
        {
            return rootTransform.GetComponentInChildren<Inventoriable>() != null;
        }

        public void OnEnableState()
        {
            Transform inventoryTransform = FindRootTransform.GetComponentInChildren<Inventoriable>().transform;

            ThisTransform.parent = inventoryTransform;
            ThisTransform.localPosition = Vector3.zero;
            ThisTransform.localEulerAngles = Vector3.zero;
        }

        public void OnEnterState()
        { 
        
        }

        public void OnExitState()
        { 
        
        }
    }
}