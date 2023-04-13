using UnityEngine;

namespace AssemblyActorCore
{
    public class MovablePreset : Movable
    {
        public virtual void StartMovement() { }
        public virtual void StopMovement() { }
        public virtual void MoveToDirection(Vector3 direction, float speed) { }
        public virtual void Jump(float force) { }
    }

    public class PositionablePreset : Positionable
    {
        public virtual void UpdateModel() { }
    }

    public class PresenterPreset : Presenter
    {
        protected Animatorable animatorable;
        protected Rotable rotable;
        protected MovablePreset movable;
        protected PositionablePreset positionable;
        protected Input input => _inputable.Input;
        private Inputable _inputable;

        protected new void Awake()
        {
            base.Awake();

            Actor actor = GetComponentInParent<Actor>();

            if (actor?.Preset == Preset.None)
            {
                gameObject.SetActive(false);

                Debug.LogWarning(gameObject.name + " - this <Presenter> does not work with Actor.Preset = None");
            }
            else
            {
                _inputable = GetComponentInParent<Inputable>();
                animatorable = GetComponentInParent<Animatorable>();
                rotable = GetComponentInParent<Rotable>();
                movable = GetComponentInParent<MovablePreset>();
                positionable = GetComponentInParent<PositionablePreset>();
            }
        }

        public override void Enter() { }
        public override void UpdateLoop() { }
        public override void FixedLoop() { }
        public override void Exit() { }
    }
}