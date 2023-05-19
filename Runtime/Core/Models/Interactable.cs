using UnityEngine;

namespace Actormachine
{
    public class Interactable : ActorBehaviour
    {
        public Target Target;

        public void SetTarget(Transform target)
        {
            Target = new Target(RootTransform, target);
        }

        public void SetTarget(Transform target, Vector3 position)
        {
            Target = new Target(RootTransform, target, position);
        }
    }
}