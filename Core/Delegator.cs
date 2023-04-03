using UnityEngine;

namespace AssemblyActorCore
{
    public class Delegator : MonoBehaviour
    {
        protected void TryToActivate(Actionable actionable, GameObject action) => actionable.TryToActivate(action);
    }
}
