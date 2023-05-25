using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public class ActivatorByGrabable : Activator
    {
        // Model Components
        private List<Transform> _targets = new List<Transform>();
        
        public bool IsExists(Transform target) => _targets.Exists(t => t == target);
        
        public override void UpdateLoop()
        {
            SetActive(_targets.Count > 0);
        }

        public override void Enter()
        {
            Transform target = _targets[0];

            _targets.Remove(target);

            target.parent = ThisTransform.parent;
            target.localPosition = Vector3.zero;
            target.localEulerAngles = Vector3.zero;

            State[] states = target.GetComponentsInChildren<State>();

            foreach (State state in states) state.Enable();

            state.Deactivate();
        }

        private void OnTriggerEnter(Collider collider)
        {
            Grabbable grabbable = collider.GetComponent<Grabbable>();

            if (grabbable)
            {
                if (IsExists(collider.transform) == false)
                {
                    _targets.Add(collider.transform);
                }
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            Grabbable grabbable = collider.GetComponent<Grabbable>();

            if (grabbable)
            {
                if (IsExists(collider.transform))
                {
                    _targets.Remove(collider.transform);
                }
            }
        }
    }
}