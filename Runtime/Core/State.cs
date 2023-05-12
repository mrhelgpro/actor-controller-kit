using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public enum StateType { Controller, Interaction, Forced, Irreversible, Required };

    /// <summary> State to update Presenters. </summary>
    public sealed class State : MonoBehaviour
    {
        public string Name = "Controller";
        public StateType Type;

        private Actor _actor;
        private List<Presenter> _presenters = new List<Presenter>();
        private List<GameObject> _childObjects = new List<GameObject>();


        private void Awake()
        {
            foreach (Presenter controller in GetComponentsInChildren<Presenter>()) _presenters.Add(controller);

            // Add disabled child objects to Enable them when Enter() is called and Disable them when Exit() is called
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject childObject = transform.GetChild(i).gameObject;

                if (childObject.activeSelf == false)
                {
                    _childObjects.Add(childObject);
                }
            }

            _actor = GetComponentInParent<Actor>();
        }

        public bool IsCurrentState => _actor.IsCurrentState(this);

        public void Enter()
        {
            foreach (Presenter controller in _presenters) controller.Enter();
            foreach (GameObject childObject in _childObjects) childObject.SetActive(true);
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
            foreach (GameObject childObject in _childObjects) childObject.SetActive(false);
        }
    }
}