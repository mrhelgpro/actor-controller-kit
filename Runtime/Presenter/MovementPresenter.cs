using UnityEngine;

namespace AssemblyActorCore
{
    public class MovementPresenter : PresenterPreset
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
            animatorable.Play(Name, movable.GetVelocity);
            movable.StartMovement();
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

        public override void Exit() => movable.StopMovement();

        protected void AnimationHandler()
        {
            animatorable.SetSpeed(movable.GetVelocity);
            animatorable.SetJump(movable.IsJump);
            animatorable.SetFall(movable.IsFall);
            animatorable.SetGrounded(positionable.IsGrounded);
        }

        protected void MoveHandler()
        {
            Vector3 inputDirection = new Vector3(input.Move.x, 0, input.Move.y);
            float speed = input.Shift ? MoveShift : MoveSpeed;

            Vector3 lookDirection = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
            Vector3 projectedLookDirection = Vector3.ProjectOnPlane(lookDirection, Vector3.up);
            Vector3 movementDirection = projectedLookDirection * inputDirection.z + Camera.main.transform.right * inputDirection.x;
            
            movable.MoveToDirection(movementDirection, speed);
            rotable.UpdateModel(movementDirection, input.Look.Value.x);
            positionable.UpdateModel();
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
        }
    }
}