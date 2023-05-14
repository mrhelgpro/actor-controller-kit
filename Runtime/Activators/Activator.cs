using UnityEngine;

namespace Actormachine
{
    /// <summary> To activate the Presenters. </summary>
    public class Activator : StateBehaviour
    {
        /// <summary> Called in Update to check to activate Presenter. </summary>
        public virtual void CheckLoop()
        {
            if (ActorIsFree)
            {
                TryToActivate();
            }
        }

        /// <summary> Called once during Awake. Use "GetComponentInRoot". </summary>
        public override void Initiation() { }
    }
}