using UnityEngine;

namespace AssemblyActorCore
{
    public abstract class DetectedByTrigger : MonoBehaviour
    {
        public string TargetTag = "Any";

#if UNITY_EDITOR
        private void OnValidate()
        {
            TargetTag = TargetTag == "" ? "Any" : TargetTag;

            Collider collider = GetComponent<Collider>();
            
            if (collider)
            {
                collider.isTrigger = true;
            }
        }
#endif

        private void targetCheck(Transform target, bool enter)
        {
            if (TargetTag == "Any" ? true : TargetTag == target.tag)
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

        public abstract void OnTargetEnter(Transform target);
        public abstract void OnTargetExit(Transform target);

        private void OnTriggerEnter(Collider collider) => targetCheck(collider.transform, true);
        private void OnTriggerEnter2D(Collider2D collider) => targetCheck(collider.transform, true);
        private void OnTriggerExit(Collider collider) => targetCheck(collider.transform, false);
        private void OnTriggerExit2D(Collider2D collider) => targetCheck(collider.transform, false);
    }
}