using UnityEngine;
using UnityEditor;

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
            GetComponent<Camera>().fieldOfView = target.Settings.FieldOfView;

            transform.rotation = Quaternion.Euler(target.Settings.VerticalDirection, target.Settings.HorizontalDirection, 0);
            transform.position = transform.rotation * new Vector3(target.Settings.Shoulder, target.Settings.Height, -target.Settings.Distance) + target.transform.position;
        }

        private void followTheTarget()
        {
            if (Target)
            {
                _camera.fieldOfView = Target.Settings.FieldOfView;

                float horizontal = Mathf.SmoothDampAngle(_mainTransform.eulerAngles.x, Target.Settings.VerticalDirection, ref _angleVelocity.y, Target.Settings.RotationTime);
                float vertical = Mathf.SmoothDampAngle(_mainTransform.eulerAngles.y, Target.Settings.HorizontalDirection, ref _angleVelocity.x, Target.Settings.RotationTime);
                Quaternion rotation = Quaternion.Euler(horizontal, vertical, 0);

                Vector3 position = rotation * new Vector3(Target.Settings.Shoulder * 2, Target.Settings.Height, -Target.Settings.Distance) + Target.ThisTransform.position;
                Vector3 delta = new Vector3(Target.ThisTransform.position.x, Target.ThisTransform.position.y + Target.Settings.Height, Target.ThisTransform.position.z) - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Target.Settings.Distance));
                Vector3 destination = position + delta;

                _mainTransform.rotation = rotation;
                _mainTransform.position = Vector3.SmoothDamp(_mainTransform.position, destination, ref _moveVelocity, Target.Settings.MoveTime);
            }
        }

        public void FindTarget()
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