using UnityEngine;

namespace Actormachine
{
    /// <summary> 
    /// Do not use the standard Update, FixedUpdate methods,
    /// instead use the overrides UpdateLoop, FixedUpdateLoop methods
    /// </summary>
    [RequireComponent(typeof(State))]
    [RequireComponent(typeof(Activator))]
    public abstract class Presenter : StateBehaviour
    {
        /// <summary> Called once when "Presenter" starts running. </summary>
        public virtual void Enter() { }

        /// <summary> Called after Enter in Update. </summary>
        public abstract void UpdateLoop();

        /// <summary> Called after Enter in FixedUpdate. </summary>
        public virtual void FixedUpdateLoop() { }

        /// <summary> Called once when "Presenter" stops running. </summary>
        public virtual void Exit() { }
    }
}