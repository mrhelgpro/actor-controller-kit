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
        public virtual void UpdateData() { }
    }

    public class PresenterPreset : Presenter
    {
        protected Animatorable animatorable;
        protected Directable directable;
        protected MovablePreset movable;
        protected PositionablePreset positionable;
        protected Input input;

        protected new void Awake()
        {
            base.Awake();

            Actor actor = GetComponentInParent<Actor>();

            if (actor?.Preset == Preset.Clear)
            {
                gameObject.SetActive(false);

                Debug.LogWarning(gameObject.name + " - this <Presenter> does not work with Actor.Preset = None");
            }
            else
            {
                input = GetComponentInParent<Inputable>().Input;
                animatorable = GetComponentInParent<Animatorable>();
                directable = GetComponentInParent<Directable>();
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