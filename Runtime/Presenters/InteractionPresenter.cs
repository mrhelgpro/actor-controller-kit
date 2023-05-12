using UnityEngine;

namespace Actormachine
{
    public class InteractionPresenter : Presenter
    {
        [Header("Interaction")]
        public float Duration = 1;
        
        protected Vector3 interactionPosition;

        // Model Components
        protected Inputable inputable;
        protected Animatorable animatorable;

        private float _speed => 1 / Duration;
        private float _timer = 0;

        public override void Initiation()
        {
            // Get components using "GetComponentInRoot" to create them on <Actor>
            inputable = GetComponentInRoot<Inputable>();
            animatorable = GetComponentInRoot<Animatorable>();
        }

        public override void Enter()
        {
            interactionPosition = RootTransform.position;
            animatorable.Play(Name);
            animatorable.SetFloat("Speed", _speed);
            _timer = 0;
        }

        public override void UpdateLoop()
        {
            _timer += Time.deltaTime;

            if (_timer >= Duration)
            {
                Deactivate();
            }

            // REMOVE THIS TO FIXED UPDATE
            //interactionPosition += Vector3.up * Time.fixedDeltaTime * 2; // REMOTE THIS!!!!!!!!!!!!!!!!!!!!!!!
            //interactionPosition += Vector3.up * Time.deltaTime * 2; // REMOTE THIS!!!!!!!!!!!!!!!!!!!!!!!
            RootTransform.position = interactionPosition;
        }

        public override void Exit() { }
    }
}