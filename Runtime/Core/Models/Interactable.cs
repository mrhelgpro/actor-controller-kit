using System.Collections.Generic;
using System;
using UnityEngine;

namespace Actormachine
{
    public class Interactable : ModelBehaviour
    {
        public Transform Target;

        private List<Transform> _targets = new List<Transform>();
        public bool IsExists(Transform target) => _targets.Exists(t => t == target);

        public void Add(Transform target)
        {
            _targets.Add(target);
        }

        public void Remove(Transform target)
        {
            _targets.Remove(target);
        }

        public void SetTargetByType<T>() where T : ModelBehaviour
        {
            foreach (Transform target in _targets)
            {
                if (target.GetComponent<T>() == null) break;

                Target = target;

                return;
            }
        }

        public void SetNearestTargetByType<T>() where T : ModelBehaviour
        {
            //TODO
        }     
    }
}