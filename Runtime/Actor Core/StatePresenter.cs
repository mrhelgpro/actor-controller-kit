using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public class StatePresenter : MonoBehaviour
    {
        public ControllerType Type;
        public string Name = "Controller";

        private List<Presenter> _presenters = new List<Presenter>();
        private List<GameObject> _childObjects = new List<GameObject>();

        private void Awake()
        {
            foreach (Presenter controller in GetComponentsInChildren<Presenter>()) _presenters.Add(controller);

            // Find all child gameObjects
        }

        public void Enter()
        {
            foreach (Presenter controller in _presenters) controller.Enter();
        }

        public void UpdateLoop()
        {
            foreach (Presenter controller in _presenters) controller.UpdateLoop();
        }

        public void Exit()
        {
            foreach (Presenter controller in _presenters) controller.Exit();
        }
    }
}