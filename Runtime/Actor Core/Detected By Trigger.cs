using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class DetectedByTrigger : MonoBehaviour
    {
        public string Tag = "Any";
        public LayerMask Layer;

#if UNITY_EDITOR
        private void OnValidate()
        {
            Tag = Tag == "" ? "Any" : Tag;
        }
#endif

        private void targetCheck(Transform target, bool enter)
        {
            if (Tag == "Any" ? true : Tag == target.tag)
            {
                if ((Layer.value & (1 << target.gameObject.layer)) != 0)
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

        public abstract void OnTargetEnter(Transform target);
        public abstract void OnTargetExit(Transform target);

        private void OnTriggerEnter(Collider collider) => targetCheck(collider.transform, true);
        private void OnTriggerEnter2D(Collider2D collider) => targetCheck(collider.transform, true);
        private void OnTriggerExit(Collider collider) => targetCheck(collider.transform, false);
        private void OnTriggerExit2D(Collider2D collider) => targetCheck(collider.transform, false);
    }
}