using UnityEngine;

namespace AssemblyActorCore
{
    public class MovementController : Controller
    {
        [Header("Movement")]
        public MovementParametres MovementParametres;
        [Range(1, 5)] public float MoveSpeed = 3f;
        [Range(1, 10)] public float MoveShift = 5f;
        //[Range(1, 10)] public int Rate = 10;
        //[Range(0, 2)] public float Gravity = 1;
        [Range(0, 5)]  public int JumpHeight = 2;
        [Range(0, 2)] public int ExtraJumps = 0;
        [Range(0, 1)] public float Levitation = 0f;


        public CameraParametres CameraParametres = new CameraParametres();

        // Models
        [SerializeField] protected Rotable rotable = new Rotable();
        [SerializeField] protected Directable directable = new Directable();
        [SerializeField] protected Animatorable animatorable = new Animatorable();

        // Model Components
        protected Inputable inputable;
        protected Movable movable;
        protected Positionable positionable;
        protected Followable followable;

        // Jump Fields
        private int _jumpCounter;
        private bool _isJumpPressed = false;
        private bool _isJumpDone = false;
        private bool _isLevitationPressed = false;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying == false)
            {
                GetComponentInParent<Followable>()?.SetPreview(CameraParametres);
            }
        }
#endif

        protected new void Awake()
        {
            base.Awake();

            inputable = GetComponentInParent<Inputable>();
            movable = GetComponentInParent<Movable>();
            positionable = GetComponentInParent<Positionable>();

            followable = GetComponentInParent<Followable>();

            directable.Initialization(transform);
            rotable.Initialization(transform);
        }

        public override void Enter()
        {
            animatorable.Enter(transform, positionable.IsGrounded ? Name : "Fall");
            followable.Parametres = CameraParametres;
            movable.MovementParametres = MovementParametres;
        }

        public override void UpdateLoop()
        {
            JumpInput();

            directable.Update(inputable.MoveVector, inputable.LookDelta, MovementParametres.Rate);
            rotable.Update(inputable.MoveVector, directable.Look, MovementParametres.Rate);

            animatorable.Play(positionable.IsGrounded ? Name : "Fall");
            animatorable.SetFloat("Speed", movable.Velocity.magnitude);
            animatorable.SetFloat("DirectionX", directable.Local.x); //animatorable.SetFloat("DirectionX", directable.GetLocal.x, 0.1f);
            animatorable.SetFloat("DirectionZ", directable.Local.z); //animatorable.SetFloat("DirectionZ", directable.GetLocal.z, 0.1f);

            FollowableUpdate();
        }

        public override void FixedLoop()
        {
            MoveUpdate();
            JumpUpdate();
        }

        public override void Exit() 
        {
            movable.Exit();
        }

        protected void MoveUpdate()
        {
            Vector3 projectOntoSurface = positionable.ProjectOntoSurface(inputable.MoveVector).normalized;

            MovementParametres.Direction = projectOntoSurface;
            MovementParametres.Speed = inputable.ShiftState ? MoveShift : MoveSpeed;
        }
        protected void JumpUpdate()
        {
            if (_isJumpDone == false)
            {
                if (_isJumpPressed == true)
                {
                    movable.SetForce(Vector3.up * JumpHeight.HeightToForce(MovementParametres.Gravity));

                    MovementParametres.Gravity = MovementParametres.Gravity - Levitation;

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
            _isJumpPressed = inputable.MotionState;

            if (_isJumpPressed == false)
            {
                if (_isLevitationPressed == true)
                {
                    MovementParametres.Gravity = MovementParametres.Gravity + Levitation;
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

        protected void FollowableUpdate()
        {
            bool isRotable = false;

            if (CameraParametres.InputCameraMode == InputCameraMode.Free)
            {
                isRotable = true;
            }
            else if (CameraParametres.InputCameraMode == InputCameraMode.LeftHold)
            {
                isRotable = inputable.ActionLeftState;
            }
            else if (CameraParametres.InputCameraMode == InputCameraMode.MiddleHold)
            {
                isRotable = inputable.ActionMiddleState;
            }
            else if (CameraParametres.InputCameraMode == InputCameraMode.RightHold)
            {
                isRotable = inputable.ActionRightState;
            }

            if (isRotable)
            {
                CameraParametres.Orbit.Horizontal += inputable.LookDelta.x * CameraParametres.Orbit.SensitivityX;
                CameraParametres.Orbit.Vertical += inputable.LookDelta.y * CameraParametres.Orbit.SensitivityY;
                CameraParametres.Orbit.Vertical = Mathf.Clamp(CameraParametres.Orbit.Vertical, -30, 80);
            }
        }
    }
}