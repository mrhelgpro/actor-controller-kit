using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class ActorCamera : MonoBehaviour
    {
        public enum UpdateMode { Fixed, Update }
        public UpdateMode Mode = UpdateMode.Fixed;

        public TargetForCamera Target;

        private Vector3 _moveVelocity = Vector3.zero;
        private Vector2 _angleVelocity = Vector2.zero;
        private Transform _mainTransform;
        private Camera _camera;

        private void Awake()
        {
            _mainTransform = transform;
            _camera = GetComponent<Camera>();

            if (Target == null) FindTarget();
        }

        private void FixedUpdate()
        {
            if (Mode == UpdateMode.Fixed) followTheTarget();
        }

        private void Update()
        {
            if (Mode == UpdateMode.Update) followTheTarget();
        }

        public void PreviewTheTarget()
        {
            if (Target)
            {
                transform.rotation = Quaternion.Euler(Target.Settings.Vertical, Target.Settings.Horizontal, 0);
                transform.position = transform.rotation * new Vector3(0, Target.Settings.Height, -Target.Settings.Distance) + Target.Transform.position;
            }
        }

        private void followTheTarget()
        {
            if (Target)
            {
                Vector2 look = Vector2.SmoothDamp(new Vector2(_mainTransform.eulerAngles.x, _mainTransform.eulerAngles.y), new Vector2 (Target.Settings.Vertical, Target.Settings.Horizontal), ref _angleVelocity, Target.Settings.RotationTime);
                Quaternion rotation = Quaternion.Euler(look.x, look.y, 0);

                Vector3 position = rotation * new Vector3(0, Target.Settings.Height, -Target.Settings.Distance) + Target.Transform.position;
                Vector3 delta = new Vector3(Target.Transform.position.x, Target.Transform.position.y + Target.Settings.Height, Target.Transform.position.z) - _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Target.Settings.Distance));
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
                Target = player.GetComponentInChildren<TargetForCamera>();
            }
            else
            {
                Target = FindObjectOfType<TargetForCamera>();

                if (Target == null)
                {
                    Debug.LogWarning("<TargetForCamera> not found");
                }
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
                    myTarget.FindTarget();
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