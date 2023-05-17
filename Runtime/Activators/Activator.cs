using UnityEngine;

namespace Actormachine
{
    /// <summary> To activate the Presenters. </summary>
    public class Activator : StateBehaviour
    {
        public bool IsReady = false;    

        /// <summary> Called in Update to check to activate Presenter. </summary>
        public virtual void UpdateLoop()
        {
            IsReady = ActorIsFree;
            Activate();
        }
    }
}