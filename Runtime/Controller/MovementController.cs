using UnityEngine;

namespace AssemblyActorCore
{
    public class MovementController : Controller
    {
        [Header("Movement")]
        [Range(1, 5)] public float MoveSpeed = 3f;
        [Range(1, 10)] public float MoveShift = 5f;
        [Range(1, 10)] public int Rate = 10;
        [Range(0, 2)] public float Gravity = 1;
        [Range(0, 5)]  public int JumpHeight = 2;
        [Range(0, 2)] public int ExtraJumps = 0;
        [Range(0, 1)] public float Levitation = 0f;

        public ActorCameraSettings CameraSettings = new ActorCameraSettings();

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
                GetComponentInParent<Followable>()?.SetPreview(CameraSettings);
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

            animatorable.Initialization(transform);
            directable.Initialization(transform);
            rotable.Initialization(transform);
        }

        public override void Enter()
        {
            animatorable.Play(positionable.IsGrounded ? Name : "Fall");          
        }

        public override void UpdateLoop() 
        {
            JumpInput();
            
            directable.Update(inputable.Move, inputable.Look.Delta, Rate);
            rotable.Update(directable.Move, directable.Look, Rate);

            animatorable.Play(positionable.IsGrounded ? Name : "Fall");
            animatorable.SetFloat("Speed", movable.Velocity.magnitude);
            animatorable.SetFloat("DirectionX", directable.Local.x); //animatorable.SetFloat("DirectionX", directable.GetLocal.x, 0.1f);
            animatorable.SetFloat("DirectionZ", directable.Local.z); //animatorable.SetFloat("DirectionZ", directable.GetLocal.z, 0.1f);
            
            followable?.SetParametres(CameraSettings, ref inputable.Look);
        }

        public override void FixedLoop()
        {
            MoveHandler();       
            JumpHandler();
        }

        public override void Exit() { }

        protected void MoveHandler()
        {
            float speed = inputable.Shift ? MoveShift : MoveSpeed;
            Vector3 projectOntoSurface = positionable.ProjectOntoSurface(directable.Move).normalized;

            positionable.UpdateParametres();
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