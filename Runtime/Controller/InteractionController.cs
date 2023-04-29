using UnityEngine;

namespace AssemblyActorCore
{
    public class InteractionController : Controller
    {
        [Header("Interaction")]
        public float Duration = 1;
        
        public CameraSettings CameraSettings = new CameraSettings();

        protected Vector3 interactionPosition;

        // Models
        [SerializeField] protected Rotable rotable = new Rotable();
        [SerializeField] protected Directable directable = new Directable();
        [SerializeField] protected Animatorable animatorable = new Animatorable();

        // Model Components
        protected Inputable inputable;
        protected Followable followable;

        private float _speed => 1 / Duration;
        private float _timer = 0;

        protected new void Awake()
        {
            base.Awake();

            inputable = GetComponentInParent<Inputable>();

            followable = GetComponentInParent<Followable>();

            animatorable.Initialization(transform);
            directable.Initialization(transform);
            rotable.Initialization(transform);
        }

        public override void Enter()
        {
            interactionPosition = RootTransform.position;
            animatorable.Play(Name, _speed);
            _timer = 0;
        }

        public override void UpdateLoop()
        {
            _timer += Time.deltaTime;

            if (_timer >= Duration)
            {
                controllerMachine.Deactivate(gameObject);
            }

            RootTransform.position = interactionPosition;
        }

        public override void FixedLoop() { }

        public override void Exit() { }
    }
}