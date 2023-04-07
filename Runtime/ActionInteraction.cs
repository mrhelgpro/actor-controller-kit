using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionInteraction : Action
    {
        public float Duration = 1;
        private float _speed => 1 / Duration;
        private float _timer = 0;

        public override void Enter()
        {
            movable.FreezAll();
            animatorable.Play(Name, _speed);
            _timer = 0;
        }

        public override void UpdateLoop()
        {
            _timer += Time.deltaTime;

            if (_timer >= Duration)
            {
                actionable.Deactivate(myTransform);
            }
        }

        public override void FixedLoop()
        {       
            movable.MoveToDirection(Vector3.zero, 0);
        }

        public override void Exit()
        {
            movable.FreezAll();
        }
    }
}
