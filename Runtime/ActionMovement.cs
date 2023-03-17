using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionMovement : Action
    {
        public float Speed = 3;
        public float Shift = 5;

        public override void Enter() => movable.FreezRotation();

        public override void UpdateLoop()
        {

        }

        public override void FixedLoop()
        {
            Vector3 direction = new Vector3(input.MoveHorizontal, 0, input.MoveVertical);
            float speed = input.Shift ? Shift : Speed;

            animatorable.Play(Name, (direction * speed).magnitude);
            movable.MoveToDirection(direction, speed);
        }

        public override void Exit() => movable.FreezAll();
    }
}