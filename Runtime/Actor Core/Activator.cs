using UnityEngine;

namespace AssemblyActorCore
{
    public class Activator : ActorBehaviour
    {
        protected StatePresenterMachine stateMachine;

        private new void Awake()
        {
            base.Awake();

            stateMachine = GetComponentInRoot<StatePresenterMachine>();

            Initiation();
        }

        /// <summary> Called in Update to check to activate Presenter. </summary>
        public virtual void CheckLoop()
        {
            if (stateMachine.IsFree)
            {
                TryToActivate();
            }
        }

        /// <summary> Called once during Awake. Use "GetComponentInRoot". </summary>
        protected virtual void Initiation() { }
        protected void TryToActivate() => stateMachine.TryToActivate(gameObject);
        protected void Deactivate() => stateMachine.Deactivate(gameObject);
        protected bool isCurrentState => stateMachine.IsCurrentStateObject(gameObject);
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [UnityEditor.CustomEditor(typeof(Activator))]
    public class ActivatorEditor : ModelEditor
    {
        public override void OnInspectorGUI()
        {
            bool error = false;

            Activator thisTarget = (Activator)target;

            // Check StatePresenterMachine
            StatePresenterMachine statePresenterMachine = thisTarget.gameObject.GetComponentInParent<StatePresenterMachine>();
            if (statePresenterMachine == null)
            {
                DrawModelBox("<StatePresenterMachine> - is not found", BoxStyle.Error);
                error = true;
            }

            // Check StatePresenter
            StatePresenter statePresenter = thisTarget.gameObject.GetComponent<StatePresenter>();
            if (statePresenter == null)
            {
                DrawModelBox("<StatePresenter> - is not found", BoxStyle.Error);
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
                    DrawModelBox("Activated by free");
                }
            }

            UnityEditor.EditorUtility.SetDirty(thisTarget);
        }

        private void checkError()
        { 
        
        }
    }
#endif
}