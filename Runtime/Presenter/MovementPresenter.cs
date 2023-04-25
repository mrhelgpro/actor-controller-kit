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
        public ActorCameraSettings CameraSettings;

        private int _jumpCounter;
        private bool _isJumpPressed = false;
        private bool _isJumpDone = false;
        private bool _isLevitationPressed = false;
        private FlagBool _groundFlag;

        protected Inputable inputable;
        protected Animatorable animatorable;
        protected Directable directable;
        protected Rotable rotable;
        protected Movable movable;
        protected Positionable positionable;
        
        protected Followable followable;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying == false)
            {
                followable = GetComponentInParent<Followable>();

                if (followable)
                {
                    if (followable.Settings.Equals(CameraSettings) == false)
                    {
                        followable.SetPreview(CameraSettings);
                    }
                }
            }
        }
#endif

        protected new void Awake()
        {
            base.Awake();

            inputable = GetComponentInParent<Inputable>();
            animatorable = GetComponentInParent<Animatorable>();
            directable = GetComponentInParent<Directable>();
            rotable = GetComponentInParent<Rotable>();
            movable = GetComponentInParent<Movable>();
            positionable = GetComponentInParent<Positionable>();
            followable = GetComponentInParent<Followable>();
        }

        public override void Enter()
        {
            animatorable.Play(Name, movable.Velocity.magnitude);
            movable.Enable(true);           
        }

        public override void UpdateLoop() 
        {
            JumpInput();
            FollowableHandler();
        }

        public override void FixedLoop()
        {
            MoveHandler();
            AnimationHandler();
            JumpHandler();
        }

        public override void Exit() => movable.Enable(false);

        protected void FollowableHandler()
        {
            if (followable)
            {
                if (inputable.Look.Freez == false)
                {
                    CameraSettings.Horizontal = inputable.Look.Value.x;
                    inputable.Look.Value.y = Mathf.Clamp(inputable.Look.Value.y, -30, 85);
                    CameraSettings.Vertical = inputable.Look.Value.y;
                }

                followable.SetParametres(CameraSettings);
            }
        }
        protected void AnimationHandler()
        {
            animatorable.SetFloat("Speed", movable.Velocity.magnitude);
            animatorable.SetFloat("DirectionX", directable.Local.x); //animatorable.SetFloat("DirectionX", directable.GetLocal.x, 0.1f);
            animatorable.SetFloat("DirectionZ", directable.Local.z); //animatorable.SetFloat("DirectionZ", directable.GetLocal.z, 0.1f);

            if (_groundFlag.IsChange(positionable.IsGrounded))
            {
                animatorable.Play(positionable.IsGrounded ? Name : "Fall");
            }
        }
        protected void MoveHandler()
        {
            Vector2 inputMove = inputable.Move;
            Vector2 inputLook = inputable.Look.Value;
            float speed = inputable.Shift ? MoveShift : MoveSpeed;
            Vector3 moveDirection = directable.Move;
            Vector3 projectOntoSurface = positionable.ProjectOntoSurface(moveDirection).normalized;

            positionable.UpdateParametres();
            directable.SetParameters(inputMove, Rate);
            rotable.SetParameters(RotationMode, moveDirection, inputLook, Rate);
            movable.SetParametersy(projectOntoSurface, speed, Gravity, Rate);
        }
        protected void JumpHandler()
        {
            if (_isJumpDone == false)
            {
                if (_isJumpPressed == true)
                {
                    movable.SetForce(Vector3.up * JumpHeight.HeightToForce(Gravity));

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