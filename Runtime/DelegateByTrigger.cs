using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public class DelegateByTrigger : Delegator
    {
        public string Tag = "Any";
        public LayerMask Layer;

        public float During = 0;
        public bool Single = false;

        private List<Actionable> _actionables = new List<Actionable>();

        protected bool isTarget(GameObject target)
        {
            if (Tag == "Any" ? true : Tag == target.tag)
            {
                if ((Layer.value & (1 << target.layer)) != 0)
                {
                    if (target.GetComponent<Actionable>())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void OnValidate()
        {
            Tag = Tag == "" ? "Any" : Tag;
        }

        private void OnEnable() 
        {
            if (During > 0)
            {
                Invoke(nameof(deactive), During);
            }
        }

        private void activate(GameObject target)
        {
            if (isTarget(target))
            {
                Actionable actionable = target.GetComponent<Actionable>();

                foreach (Actionable item in _actionables)
                {
                    if (actionable.gameObject == item.gameObject)
                    {
                        return;
                    }
                }

                _actionables.Add(actionable);

                TryToActivate(actionable);

                if (During == 0)
                {
                    deactive();
                    return;
                }

                if (Single)
                {
                    deactive();
                    return;
                }
            }
        }

        private void deactive()
        {
            _actionables.Clear();
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider collider) => activate(collider.gameObject);
        private void OnTriggerEnter2D(Collider2D collider) => activate(collider.gameObject);
    }
}
