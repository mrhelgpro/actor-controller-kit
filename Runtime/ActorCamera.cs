using UnityEngine;

namespace AssemblyActorCore
{
    public class ActorCamera : MonoBehaviour
    {
        public enum FollowMode { None, ThirdPerson, Platformer }
        public FollowMode Mode = FollowMode.Platformer;

        public float shoulder = 0;
        public float height = 1;
        public float distance = 5;  
        public float _dampTime = 0.5f;

        private Vector3 _velocity = Vector3.zero;
        private Transform _targetPlayer;
        private Transform _mainTransform;
        private Camera _camera;

        private Inputable _inputable;
        private Vector2 _mouse;

        private void OnValidate()
        {
            gameObject.name = "Actor Camera";

            try
            {
                _targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
            }
            catch
            {
                Debug.LogWarning("Player is not find");
            }
        }

        private void Awake()
        {
            _targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
            _inputable = _targetPlayer.GetComponent<Inputable>();
            _mainTransform = transform;
            _camera = GetComponent<Camera>();
        }

        void FixedUpdate()
        {
            switch (Mode)
            {
                case FollowMode.ThirdPerson:
                    followTheThirdPerson(_targetPlayer);
                    break;
                case FollowMode.Platformer:
                    followTheTarget(_targetPlayer);
                    break;
            }
        }

        public float rotationSpeed = 5.0f; // Ўвидк≥сть обертанн€ камери

        private void followTheThirdPerson(Transform target)
        {
            _mouse.x += _inputable.Input.Look.x * rotationSpeed;
            _mouse.y += _inputable.Input.Look.y * rotationSpeed;

            _mouse.y = Mathf.Clamp(_mouse.y, -30, 75);

            _mainTransform.rotation = Quaternion.Euler(_mouse.y, _mouse.x, 0);
            _mainTransform.position = _mainTransform.rotation * new Vector3(shoulder, height, -distance) + target.position;
        }

        private void followTheTarget(Transform target)
        {
            Vector3 delta = new Vector3(target.position.x, target.position.y + height, target.position.z) - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
            Vector3 destination = _mainTransform.position + delta;

            _mainTransform.position = Vector3.SmoothDamp(_mainTransform.position, destination, ref _velocity, _dampTime);
        }
    }
}