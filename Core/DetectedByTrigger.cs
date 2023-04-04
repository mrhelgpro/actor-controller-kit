using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class DetectedByTrigger : MonoBehaviour
    {
        public string Tag = "Any";
        public LayerMask Layer;

        private void OnValidate()
        {
            Tag = Tag == "" ? "Any" : Tag;
        }

        private void targetCheck(GameObject target, bool enter)
        {
            if (Tag == "Any" ? true : Tag == target.tag)
            {
                if ((Layer.value & (1 << target.layer)) != 0)
                {
                    if (enter == true)
                    {
                        OnTargetEnter(target);
                    }
                    else
                    {
                        OnTargetExit(target);
                    }
                }
            }
        }

        public abstract void OnTargetEnter(GameObject target);
        public abstract void OnTargetExit(GameObject target);

        private void OnTriggerEnter(Collider collider) => targetCheck(collider.gameObject, true);
        private void OnTriggerEnter2D(Collider2D collider) => targetCheck(collider.gameObject, true);
        private void OnTriggerExit(Collider collider) => targetCheck(collider.gameObject, false);
        private void OnTriggerExit2D(Collider2D collider) => targetCheck(collider.gameObject, false);
    }
}