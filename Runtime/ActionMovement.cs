using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionMovement : Action
    {
        [Range(1, 5)] public float MoveSpeed = 3f;
        [Range(1, 10)] public float MoveShift = 5f;
        [Range(0, 5)]  public int JumpHeight = 2;
        [Range(0, 2)] public int ExtraJumps;
        [Range(0, 1)] public float Levitation = 1f;
      
        private int _jumpCounter;
        private bool _isJumpPressed = false;
        private bool _isJumpDone = false;
        private bool _isLevitationPressed = false;

        public override void Enter()
        {
            movable.FreezRotation();
            animatorable.Play(Name, movable.GetVelocity);
        }

        public override void UpdateLoop() 
        {
            JumpInput();
        }

        public override void FixedLoop()
        {
            MoveHandler();
            AnimationHandler();
            JumpHandler();
        }

        public override void Exit() => movable.FreezAll();

        protected void AnimationHandler()
        {
            animatorable.SetSpeed(movable.GetVelocity);
            animatorable.SetJump(movable.IsJump);
            animatorable.SetFall(movable.IsFall);
            if (positionable)  animatorable.SetGrounded(positionable.IsGrounded);
        }

        protected void MoveHandler()
        {
            Vector3 direction = new Vector3(input.Move.x, 0, input.Move.y);
            float speed = input.Shift ? MoveShift : MoveSpeed;

            movable.MoveToDirection(direction, speed);
        }

        protected void JumpHandler()
        {
            if (_isJumpDone == false)
            {
                if (_isJumpPressed == true)
                {
                    movable.Jump(JumpHeight.HeightToForce(movable.Gravity));
                    movable.Gravity = movable.Gravity - Levitation;

                    if (positionable)
                    {
                        if (positionable.IsGrounded == false)
                        {
                            _jumpCounter--;
                        }
                    }

                    _isJumpDone = true;
                    _isLevitationPressed = true;
                }
            }
        }

        protected void JumpInput()
        {
            _isJumpPressed = input.Motion;

            if (_isJumpPressed == false)
            {
                if (_isLevitationPressed == true)
                {
                    movable.Gravity = movable.Gravity + Levitation;
                    _isLevitationPressed = false;
                }

                if (positionable)
                {
                    if (positionable.IsGrounded)
                    {
                        _isJumpDone = false;
                        _jumpCounter = ExtraJumps;
                    }
                    else
                    {
                        if (_jumpCounter > 0)
                        {
                            _isJumpDone = false;
                        }
                    }
                }
                else
                {
                    _isJumpDone = false;
                }
            }
        }
    }
}