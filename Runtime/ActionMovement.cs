using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionMovement : Action
    {
        [Range(1, 10)] public float MoveSpeed = 3f;
        [Range(1, 20)] public float MoveShift = 5f;
        [Range(1, 5)]  public int JumpHeight = 2;
        [Range (0, 3)] public int AmountOfJumps = 1;
        [Range(0, 1)] public float Levitation = 1f;
       
        private int _jumpCounter;
        private bool _isJumpPressed = false;
        private bool _isJumpDone = false;

        #region LOGGER
        public bool LogHeight = false;
        private float _heightOfTheJump = 0; // To display the jump height
        private void heightLog()
        {
            if (LogHeight)
            {
                if (positionable.IsGrounded) _heightOfTheJump = 0;

                if (mainTransform.position.y > _heightOfTheJump)
                {
                    _heightOfTheJump = mainTransform.position.y;
                    Debug.Log("Height of the jump = " + _heightOfTheJump + " (" + gameObject.name + ")");
                }
            }
        }

        public bool LogSpeed = false;
        private Vector3 _lastPositionForSpeed = Vector3.zero;
        private void speedLog()
        {
            if (LogSpeed)
            {
                Vector3 velocity = (mainTransform.position - _lastPositionForSpeed) / Time.deltaTime;
                float speed = velocity.magnitude;

                _lastPositionForSpeed = mainTransform.position;

                Debug.Log("Movement speed = " + speed + " (" + gameObject.name + ")");
            }
        }
        # endregion

        public override void Enter() => movable.FreezRotation();

        public override void UpdateLoop() 
        {
            JumpInput();
        }

        public override void FixedLoop()
        {
            MoveHandler();
            JumpHandler();

            heightLog();
            speedLog();
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
                    movable.Jump(JumpHeight.HeightToForce());
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