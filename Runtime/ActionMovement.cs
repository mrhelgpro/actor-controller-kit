using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionMovement : Action
    {
        [Range(1, 10)] public float MoveSpeed = 3;
        [Range(1, 20)] public float MoveShift = 5;
        [Range(1, 5)]  public int JumpHeight = 2;
        [Range (0, 3)] public int AmountOfJumps = 1;
        [Range(0, 1)] public float Levitation = 0.5f;
       
        private int _jumpCounter;
        private bool _isJumpPressed = false;
        private bool _isJumpDone = false;

        public float h = 0;

        public override void Enter() => movable.FreezRotation();

        public override void UpdateLoop() 
        {
            JumpInput();

            if (transform.position.y > h) { h = transform.position.y; }

            Debug.Log(h);
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

            animatorable.Play(Name, (direction * speed).magnitude);
            movable.MoveToDirection(direction, speed);
        }

        protected void JumpHandler()
        {
            if (_isJumpDone == false)
            {
                if (_isJumpPressed == true)
                {
                    movable.Jump(movable.HeightToForce(JumpHeight));
                    _jumpCounter--;

                    _isJumpDone = true;
                }
            }
        }

        protected void JumpInput()
        {
            _isJumpPressed = input.Motion;

            movable.Gravity = _isJumpPressed ? Levitation : 1;

            if (_isJumpPressed == false)
            {
                if (positionable)
                {
                    if (positionable.IsGrounded)
                    {
                        _isJumpDone = false;
                        _jumpCounter = AmountOfJumps;

                        h = 0; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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