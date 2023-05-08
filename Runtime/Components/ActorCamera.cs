using UnityEngine;

namespace AssemblyActorCore
{
    public class ActorCamera : MonoBehaviour
    {
        public enum UpdateMode { Fixed, Update }
        public UpdateMode Mode = UpdateMode.Fixed;

        public Followable Target;

        private Vector3 _moveVelocity = Vector3.zero;
        private Vector2 _angleVelocity = Vector2.zero;
        private Transform _mainTransform;
        private Camera _camera;

        private void Awake()
        {
            _mainTransform = transform;
            _camera = GetComponent<Camera>();

            if (Target == null)
            {
                FindTarget();
            }
            else
            {
                if (Target.gameObject.activeSelf == false)
                {
                    Target = null;
                }
            }
        }

        private void FixedUpdate()
        {
            if (Mode == UpdateMode.Fixed) followTheTarget();
        }

        private void Update()
        {
            if (Mode == UpdateMode.Update) followTheTarget();
        }

        public void PreviewTheTarget(Followable target)
        {
            /*
            GetComponent<Camera>().fieldOfView = target.Parameters.FieldOfView;

            transform.rotation = Quaternion.Euler(target.Parameters.Orbit.Vertical, target.Parameters.Orbit.Horizontal, 0);
            transform.position = transform.rotation * new Vector3(target.Parameters.Offset.Horizontal, target.Parameters.Offset.Vertical, -target.Parameters.Offset.Distance) + target.transform.position;
        */
        }

        private void followTheTarget()
        {
            /*
            if (Target)
            {
                _camera.fieldOfView = Target.Parameters.FieldOfView;

                float horizontal = Mathf.SmoothDampAngle(_mainTransform.eulerAngles.x, Target.Parameters.Orbit.Vertical, ref _angleVelocity.y, Target.Parameters.DampTime.Rotation + 0.025f);
                float vertical = Mathf.SmoothDampAngle(_mainTransform.eulerAngles.y, Target.Parameters.Orbit.Horizontal, ref _angleVelocity.x, Target.Parameters.DampTime.Rotation + 0.025f);
                Quaternion rotation = Quaternion.Euler(horizontal, vertical, 0);

                Vector3 position = rotation * new Vector3(Target.Parameters.Offset.Horizontal * 2, Target.Parameters.Offset.Vertical, -Target.Parameters.Offset.Distance) + Target.transform.position;
                Vector3 delta = new Vector3(Target.transform.position.x, Target.transform.position.y + Target.Parameters.Offset.Vertical, Target.transform.position.z) - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Target.Parameters.Offset.Distance));
                Vector3 destination = position + delta;

                _mainTransform.rotation = rotation;
                _mainTransform.position = Vector3.SmoothDamp(_mainTransform.position, destination, ref _moveVelocity, Target.Parameters.DampTime.Move + 0.01f);
            }
            */
        }

        private void FindTarget()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player)
            {
                Target = player.GetComponentInChildren<Followable>();
            }
            else
            {
                Target = FindObjectOfType<Followable>();

                if (Target == null)
                {
                    Debug.LogWarning("<TargetForCamera> not found");
                }
            }
        }
    }
}