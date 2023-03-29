using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class ActorCamera : MonoBehaviour
    {
        public TargetForCamera Target;

        private Vector3 _moveVelocity = Vector3.zero;
        private float _angleVelocity = 0;
        private Transform _mainTransform;
        private Camera _camera;

        private void Awake()
        {
            _mainTransform = transform;
            _camera = GetComponent<Camera>();
        }

        private void FixedUpdate()
        {
            if (Target)
            {
                followTheTarget();
            }
        }

        public void PreviewTheTarget()
        {
            if (Target)
            {
                transform.rotation = Quaternion.Euler(Target.Settings.Angle, 0, 0);
                transform.position = transform.rotation * new Vector3(0, Target.Settings.Height, -Target.Settings.Distance) + Target.Transform.position;
            }
        }

        private void followTheTarget()
        {
            if (Target)
            {
                float angle = Mathf.SmoothDamp(_mainTransform.eulerAngles.x, Target.Settings.Angle, ref _angleVelocity, Target.Settings.RotationTime);
                Quaternion rotation = Quaternion.Euler(angle, 0, 0);

                Vector3 position = rotation * new Vector3(0, Target.Settings.Height, -Target.Settings.Distance) + Target.Transform.position;
                Vector3 delta = new Vector3(Target.Transform.position.x, Target.Transform.position.y + Target.Settings.Height, Target.Transform.position.z) - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Target.Settings.Distance));
                Vector3 destination = position + delta;

                _mainTransform.rotation = rotation;
                _mainTransform.position = Vector3.SmoothDamp(_mainTransform.position, destination, ref _moveVelocity, Target.Settings.MoveTime);
            }
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(ActorCamera))]
    public class ActorCameraEditor : Editor
    {
        private ActorCamera myTarget;

        public override void OnInspectorGUI()
        {
            myTarget = (ActorCamera)target;

            DrawDefaultInspector();

            if (myTarget.Target)
            {
                SerializedObject serializedObject = new SerializedObject(myTarget.Target);
                SerializedProperty settingsProperty = serializedObject.FindProperty("Settings");
                EditorGUILayout.PropertyField(settingsProperty);
                serializedObject.ApplyModifiedProperties();

                if (Application.isPlaying == false)
                {
                    myTarget.PreviewTheTarget();
                }

                if (GUILayout.Button("Clear"))
                {
                    myTarget.Target = null;
                }
            }
            else
            {
                if (GUILayout.Button("Find"))
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");

                    if (player)
                    {
                        myTarget.Target = player.GetComponentInChildren<TargetForCamera>();
                    }
                    else
                    {
                        myTarget.Target = FindObjectOfType<TargetForCamera>();

                        if (myTarget.Target == null)
                        {
                            Debug.LogWarning("<TargetForCamera> not found");
                        }
                    }
                }
            }
        }
    }
#endif
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