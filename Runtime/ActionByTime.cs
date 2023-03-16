using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionByTime : ActionBehaviour
    {
        public float Duration = 1;
        private float _speed => 1 / Duration;
        private float _timer = 0;

        protected override void Initialization() { }

        public override void WaitLoop() { }

        public override void Enter()
        {
            movable.FreezAll();
            _timer = 0;
        }

        public override void UpdateLoop()
        {
            _timer += Time.deltaTime;

            if (_timer >= Duration) actionable.Deactivate(myGameObject);
        }

        public override void FixedLoop()
        {
            animatorable.Play(Name, _speed);
            movable.MoveToDirection(Vector3.zero, 0);
        }

        public override void Exit() => movable.FreezAll();
    }
}
