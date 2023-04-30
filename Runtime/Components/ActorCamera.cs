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
            GetComponent<Camera>().fieldOfView = target.Parametres.FieldOfView;

            transform.rotation = Quaternion.Euler(target.Parametres.Orbit.Vertical, target.Parametres.Orbit.Horizontal, 0);
            transform.position = transform.rotation * new Vector3(target.Parametres.Offset.Horizontal, target.Parametres.Offset.Vertical, -target.Parametres.Offset.Distance) + target.transform.position;
        }

        private void followTheTarget()
        {
            if (Target)
            {
                _camera.fieldOfView = Target.Parametres.FieldOfView;

                float horizontal = Mathf.SmoothDampAngle(_mainTransform.eulerAngles.x, Target.Parametres.Orbit.Vertical, ref _angleVelocity.y, Target.Parametres.DampTime.Rotation + 0.025f);
                float vertical = Mathf.SmoothDampAngle(_mainTransform.eulerAngles.y, Target.Parametres.Orbit.Horizontal, ref _angleVelocity.x, Target.Parametres.DampTime.Rotation + 0.025f);
                Quaternion rotation = Quaternion.Euler(horizontal, vertical, 0);

                Vector3 position = rotation * new Vector3(Target.Parametres.Offset.Horizontal * 2, Target.Parametres.Offset.Vertical, -Target.Parametres.Offset.Distance) + Target.ThisTransform.position;
                Vector3 delta = new Vector3(Target.ThisTransform.position.x, Target.ThisTransform.position.y + Target.Parametres.Offset.Vertical, Target.ThisTransform.position.z) - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Target.Parametres.Offset.Distance));
                Vector3 destination = position + delta;

                _mainTransform.rotation = rotation;
                _mainTransform.position = Vector3.SmoothDamp(_mainTransform.position, destination, ref _moveVelocity, Target.Parametres.DampTime.Move + 0.01f);
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