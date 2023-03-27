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

        public override void Enter() => movable.FreezRotation();

        public override void UpdateLoop() 
        {
            JumpInput();
        }

        public override void FixedLoop()
        {
            MoveHandler();
            JumpHandler();
        }

        public override void Exit() => movable.FreezAll();

        protected void MoveHandler()
        {
            Vector3 direction = new Vector3(input.MoveHorizontal, 0, input.MoveVertical);
            float speed = input.Shift ? MoveShift : MoveSpeed;

            animatorable.Play(Name, movable.GetVelocity);
            movable.MoveToDirection(direction, speed);
        }

        protected void JumpHandler()
        {
            if (_isJumpDone == false)
            {
                if (_isJumpPressed == true)
                {
                    movable.Jump(JumpHeight.HeightToForce(movable.Gravity));

                    if (positionable.IsGrounded == false)
                    {
                        _jumpCounter--;
                    }

                    _isJumpDone = true;
                }
            }
        }

        protected void JumpInput()
        {
            _isJumpPressed = input.Motion;

            //movable.Gravity = _isJumpPressed ? Levitation : 1;

            if (_isJumpPressed == false)
            {
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