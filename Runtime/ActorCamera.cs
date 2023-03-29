using UnityEngine;

namespace AssemblyActorCore
{
    public class ActorCamera : MonoBehaviour
    {
        public TargetForCamera CurrentTarget;
        private TargetForCamera _previousTarget;

        private Vector3 _moveVelocity = Vector3.zero;
        //private float _angleVelocity = 0;
        private Transform _mainTransform;
        private Camera _camera;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying == false)
            {
                gameObject.name = "Actor Camera";
                _mainTransform = transform;

                if (CurrentTarget)
                {
                    CurrentTarget.ActorCamera = this;

                    if (_previousTarget != null)
                    {
                        if (CurrentTarget != _previousTarget)
                        {
                            _previousTarget.ActorCamera = null;
                            PreviewTheTarget(CurrentTarget);
                        }
                    }

                    _previousTarget = CurrentTarget;
                }
            }
        }
#endif

        private void Awake()
        {
            _mainTransform = transform;
            _camera = GetComponent<Camera>();
        }

        private void FixedUpdate()
        {
            if (CurrentTarget)
            {
                followTheTarget(CurrentTarget);
            }
        }

        public void PreviewTheTarget(TargetForCamera target)
        {
            try
            {
                _mainTransform.rotation = Quaternion.Euler(target.Angle, 0, 0);
                _mainTransform.position = _mainTransform.rotation * new Vector3(0, target.Height, -target.Distance) + target.Transform.position;
            }
            catch { }
        }


        private void followTheTarget(TargetForCamera target)
        {
            //float angle = Mathf.SmoothDamp(_mainTransform.eulerAngles.x, target.Angle, ref _angleVelocity, target.DampTime);
            //Quaternion rotation = Quaternion.Euler(angle, 0, 0);

            Quaternion rotation = Quaternion.Euler(target.Angle, 0, 0);

            Vector3 position = rotation * new Vector3(0, target.Height, -target.Distance) + target.Transform.position;
            Vector3 delta = new Vector3(target.Transform.position.x, target.Transform.position.y + target.Height, target.Transform.position.z) - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, target.Distance));
            Vector3 destination = position + delta;

            //_mainTransform.rotation = rotation;
            _mainTransform.rotation = Quaternion.Lerp(_mainTransform.rotation, rotation, target.AngleSpeed * Time.fixedDeltaTime);
            _mainTransform.position = Vector3.SmoothDamp(_mainTransform.position, destination, ref _moveVelocity, target.DampTime);
        }
    }
}

/*
public float shoulder = 0;
public float rotationSpeed = 5.0f;
private Inputable _inputable;
private Vector2 _mouse;
_inputable = _targetPlayer.GetComponentInParent<Inputable>();

private void followTheThirdPerson(Transform target)
{
    _mouse.x += _inputable.Input.Look.x * rotationSpeed;
    _mouse.y += _inputable.Input.Look.y * rotationSpeed;
    _mouse.y = Mathf.Clamp(_mouse.y, -30, 75);
    _mainTransform.rotation = Quaternion.Euler(_mouse.y, _mouse.x, 0);
    _mainTransform.position = _mainTransform.rotation * new Vector3(shoulder, height, -distance) + target.position;
}
*/