using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public enum PresenterType { Controller, Interaction, Forced, Irreversible, Required };

    public sealed class StatePresenter : MonoBehaviour
    {
        public PresenterType Type;
        public string Name = "Controller";

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
        }

        public void Enter()
        {
            foreach (Presenter controller in _presenters) controller.Enter();
            foreach (GameObject childObject in _childObjects) childObject.SetActive(true);
        }

        public void UpdateLoop()
        {
            foreach (Presenter controller in _presenters) controller.UpdateLoop();
        }

        public void Exit()
        {
            foreach (Presenter controller in _presenters) controller.Exit();
            foreach (GameObject childObject in _childObjects) childObject.SetActive(false);
        }
    }
}