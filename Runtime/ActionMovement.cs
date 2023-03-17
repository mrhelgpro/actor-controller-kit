using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionMovement : Action
    {
        public float MoveSpeed = 3;
        public float MoveShift = 5;
        public float JumpForce = 5;

        //public float Levitation = 0.5f;

        private bool _isJumpPressed = false;
        private bool _isJumpDone = false;
        public override void Enter() => movable.FreezRotation();

        public override void UpdateLoop() 
        {
            jumpInput();
            jumpReady();
        }

        public override void FixedLoop()
        {
            Vector3 direction = new Vector3(input.MoveHorizontal, 0, input.MoveVertical);
            float speed = input.Shift ? MoveShift : MoveSpeed;

            animatorable.Play(Name, (direction * speed).magnitude);
            movable.MoveToDirection(direction, speed);
            jumpHandler();
        }

        private void jumpInput() => _isJumpPressed = input.Motion;

        private void jumpHandler()
        {
            if (_isJumpDone == false)
            {
                if (_isJumpPressed == true)
                {
                    movable.Jump(JumpForce);
                    _isJumpDone = true;
                }
            }
        }

        private void jumpReady()
        {
            if (_isJumpPressed == false)
            {
               if (movable.IsGrounded)
               { 
                   _isJumpDone = false; 
               }
            }
        }

        public override void Exit() => movable.FreezAll();
    }
}