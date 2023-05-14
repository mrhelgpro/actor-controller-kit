using System.Collections.Generic;

namespace Actormachine
{
    public enum StateType { Controller, Interaction, Forced, Irreversible, Required };

    /// <summary> State to update Presenters. </summary>
    public sealed class State : StateBehaviour
    {
        public string Name = "Controller";
        public StateType Type;

        private List<Presenter> _presenters = new List<Presenter>();

        public override void Initiation() 
        {
            _presenters = new List<Presenter>();

            foreach (Presenter controller in GetComponentsInChildren<Presenter>()) _presenters.Add(controller);
        }

        public void Enter()
        {
            foreach (Presenter controller in _presenters) controller.Enter();
        }

        public void UpdateLoop()
        {
            foreach (Presenter controller in _presenters) controller.UpdateLoop();
        }

        public void FixedUpdateLoop()
        {
            foreach (Presenter controller in _presenters) controller.FixedUpdateLoop();
        }

        public void Exit()
        {
            foreach (Presenter controller in _presenters) controller.Exit();
        }
    }
}