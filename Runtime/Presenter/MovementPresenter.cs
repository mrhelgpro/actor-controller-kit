using UnityEngine;

namespace AssemblyActorCore
{
    public class MovementPresenter : Presenter
    {
        [Header("Movement")]
        public RotationMode RotationMode = RotationMode.DirectionOfMovement;
        [Range(1, 5)] public float MoveSpeed = 3f;
        [Range(1, 10)] public float MoveShift = 5f;
        [Range(1, 10)] public int Rate = 10;
        [Range(0, 2)] public float Gravity = 1;
        [Range(0, 5)]  public int JumpHeight = 2;
        [Range(0, 2)] public int ExtraJumps;
        [Range(0, 1)] public float Levitation = 1f;

        private int _jumpCounter;
        private bool _isJumpPressed = false;
        private bool _isJumpDone = false;
        private bool _isLevitationPressed = false;

        private FlagBool _groundFlag;

        protected Animatorable animatorable;
        protected Directable directable;
        protected Rotable rotable;
        protected Movable movable;
        protected Positionable positionable;
        protected Inputable inputable;

        protected new void Awake()
        {
            base.Awake();

            inputable = GetComponentInParent<Inputable>();
            animatorable = GetComponentInParent<Animatorable>();
            directable = GetComponentInParent<Directable>();
            rotable = GetComponentInParent<Rotable>();
            movable = GetComponentInParent<Movable>();
            positionable = GetComponentInParent<Positionable>();
        }

        public override void Enter()
        {
            animatorable.Play(Name, movable.GetVelocity());
            movable.SetMoving(true);           
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

        public override void Exit() => movable.SetMoving(false);

        protected void AnimationHandler()
        {
            animatorable.SetFloat("Speed", movable.GetVelocity());
            animatorable.SetFloat("DirectionX", directable.GetLocal.x); //animatorable.SetFloat("DirectionX", directable.GetLocal.x, 0.1f);
            animatorable.SetFloat("DirectionZ", directable.GetLocal.z); //animatorable.SetFloat("DirectionZ", directable.GetLocal.z, 0.1f);

            if (_groundFlag.IsChange(positionable.IsGrounded))
            {
                animatorable.Play(positionable.IsGrounded ? Name : "Fall");
            }
        }

        protected void MoveHandler()
        {
            directable.UpdateData(inputable.Move, Rate);
            rotable.UpdateData(RotationMode, directable.GetMove, inputable.Look.Value, Rate);
            positionable.UpdateData();
            movable.Horizontal(positionable.ProjectOntoSurface(directable.GetMove).normalized, inputable.Shift ? MoveShift : MoveSpeed, Rate, Gravity);       
        }

        protected void JumpHandler()
        {
            if (_isJumpDone == false)
            {
                if (_isJumpPressed == true)
                {
                    movable.Vertical(JumpHeight.HeightToForce(Gravity));
                    Gravity = Gravity - Levitation;

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
            _isJumpPressed = inputable.Motion;

            if (_isJumpPressed == false)
            {
                if (_isLevitationPressed == true)
                {
                    Gravity = Gravity + Levitation;
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