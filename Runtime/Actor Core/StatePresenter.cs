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

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [UnityEditor.CustomEditor(typeof(StatePresenter))]
    public class StatePresenterEditor : ModelEditor
    {
        public override void OnInspectorGUI()
        {
            bool error = false;

            StatePresenter thisTarget = (StatePresenter)target;

            // Check StatePresenterMachine
            StatePresenterMachine statePresenterMachine = thisTarget.gameObject.GetComponentInParent<StatePresenterMachine>();
            if (statePresenterMachine == null)
            {
                DrawModelBox("<StatePresenterMachine> - is not found", BoxStyle.Error);
                error = true;
            }

            // Check Activator
            Activator activator = thisTarget.gameObject.GetComponent<Activator>();
            if (activator == null)
            {
                DrawModelBox("<Activator> - is not found", BoxStyle.Error);
                error = true;
            }

            // Check Presenter
            Presenter presenter = thisTarget.gameObject.GetComponentInChildren<Presenter>();
            if (presenter == null)
            {
                DrawModelBox("<Presenter> - is not found", BoxStyle.Error);
                error = true;
            }

            if (error == false)
            {
                DrawDefaultInspector();

                if (Application.isPlaying)
                {
                    if (statePresenterMachine.IsCurrentStateObject(thisTarget.gameObject))
                    {
                        DrawModelBox("State active", BoxStyle.Active);
                    }
                    else
                    {
                        DrawModelBox("Waiting for state activation");
                    }
                }
                else
                {
                    DrawModelBox("Update the Presenter");
                }
            }

            UnityEditor.EditorUtility.SetDirty(thisTarget);
        }
    }
#endif
}