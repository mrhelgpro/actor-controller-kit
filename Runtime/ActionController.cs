using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionController : ActionBehaviour
    {
        public float Speed = 3;
        public float Shift = 5;

        protected override void Initialization() { }

        public override void WaitLoop()
        {
            if (myGameObject.activeSelf) actionable.Activate(myGameObject);
        }

        public override void Enter() => movable.FreezRotation();

        public override void UpdateLoop()
        {

        }

        public override void FixedLoop()
        {
            Vector3 direction = new Vector3(input.Direction.x, 0, input.Direction.y);
            float speed = input.Shift ? Shift : Speed;

            animatorable.Play(Name, (direction * speed).magnitude);
            movable.MoveToDirection(direction, speed);
        }

        public override void Exit() => movable.FreezAll();
    }
}