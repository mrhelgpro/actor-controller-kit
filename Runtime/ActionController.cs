using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionController : ActionBehaviour
    {
        public float Speed = 3;
        public float Shift = 5;

        private Vector3 _direction;
        private float _speed;

        protected override void Initialization() => type = ActionType.Controller;

        public override void WaitLoop()
        {
            if (myGameObject.activeSelf) actionable.Activate(myGameObject);
        }

        public override void Enter() => movable.FreezRotation();

        public override void UpdateLoop()
        {
            _direction = inputable.Direction;
            _speed = inputable.A == Inputable.Key.Press ? Shift : Speed;
        }

        public override void FixedLoop()
        {
            animatorable.Play(Name, (_direction * _speed).magnitude);
            movable.MoveToDirection(_direction, _speed);
        }

        public override void Exit() => movable.FreezAll();
    }
}