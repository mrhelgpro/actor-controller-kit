using UnityEngine;

namespace AssemblyActorCore
{
    public class InteractionPresenter : Presenter
    {
        public float Duration = 1;
        private float _speed => 1 / Duration;
        private float _timer = 0;

        protected Animatorable animatorable;
        protected Directable directable;
        protected Rotable rotable;
        protected Movable movable;
        protected Positionable positionable;
        protected Inputable inputable;

        protected new void Awake()
        {
            base.Awake();

            inputable = GetComponentInParent<Inputable>();
            animatorable = GetComponentInParent<Animatorable>();
            directable = GetComponentInParent<Directable>();
            rotable = GetComponentInParent<Rotable>();
            movable = GetComponentInParent<Movable>();
            positionable = GetComponentInParent<Positionable>();
        }

        public override void Enter()
        {
            movable.SetMoving(false);
            animatorable.Play(Name, _speed);
            _timer = 0;
        }

        public override void UpdateLoop()
        {
            _timer += Time.deltaTime;

            if (_timer >= Duration)
            {
                presenterMachine.Deactivate(gameObject);
            }
        }

        public override void FixedLoop() { }

        public override void Exit() { }
    }
}
