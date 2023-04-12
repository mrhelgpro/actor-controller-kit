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
        public const float MaxSlope = 46;
        private float _timerSliding = 0;

        public virtual void UpdateModel() { }

        protected void slidingCheck()
        {
            float startTime = 0.1f;
            float endTime = 0.2f;

            if (IsGrounded == false) _timerSliding = startTime;

            if (IsSliding)
            {
                _timerSliding = SurfaceSlope < MaxSlope ? _timerSliding - Time.fixedDeltaTime : endTime;
                IsSliding = _timerSliding <= 0.0f ? false : true;
                _timerSliding = IsSliding == false ? 0.0f : _timerSliding;
            }
            else
            {
                _timerSliding = SurfaceSlope > MaxSlope && SurfaceSlope < 75 ? _timerSliding + Time.fixedDeltaTime : 0.0f;
                IsSliding = _timerSliding > startTime ? true : false;
                _timerSliding = IsSliding ? endTime : _timerSliding;
            }
        }

        public Vector3 Project(Vector3 direction)
        {
            Vector3 projection = direction - Vector3.Dot(direction, surfaceNormal) * surfaceNormal;
            Vector3 normal = projection == Vector3.zero || IsGrounded == false || SurfaceSlope > 75 ? direction : projection;
            Vector3 slope = Vector3.down - Vector3.Dot(Vector3.down, surfaceNormal) * surfaceNormal;

            return IsSliding ? SurfaceSlope > 75 ? direction : slope : normal;
        }
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