using UnityEngine;
using UnityEditor;
using AssemblyActorCore;

public sealed class InputPlayerController : ActorComponent
{
    public enum MoveMode { Input, Target, Both }
    public MoveMode MoveDirectionMode = MoveMode.Input;
    public enum TargetMode { LeftAction, MiddleAction, RightAction }
    public TargetMode InputTargetMode = TargetMode.LeftAction;

    public LayerMask TargetRequiredLayers;

    private Target _targetPosition;
    private Camera _camera;
    private Transform _cameraTransform;
    private InputActions _inputActions;
    private Inputable _inputable;

    private new void Awake()
    {
        base.Awake();

        _inputable = GetComponentInActor<Inputable>();

        _camera = Camera.main;
        _cameraTransform = _camera.transform;

        if (_inputable == null)
        {
            gameObject.SetActive(false);

            Debug.LogWarning(gameObject.name + " - is not found <Inputable>");
        }
        else
        {
            _inputActions = new InputActions();

            _inputActions.Player.Menu.performed += context => _inputable.Menu = true;
            _inputActions.Player.Menu.canceled += context => _inputable.Menu = false;

            _inputActions.Player.North.performed += context => _inputable.OptionState = true;
            _inputActions.Player.North.canceled += context => _inputable.OptionState = false;

            _inputActions.Player.East.performed += context => _inputable.CancelState = true;
            _inputActions.Player.East.canceled += context => _inputable.CancelState = false;

            _inputActions.Player.South.performed += context => _inputable.MotionState = true;
            _inputActions.Player.South.canceled += context => _inputable.MotionState = false;

            _inputActions.Player.West.performed += context => _inputable.InteractState = true;
            _inputActions.Player.West.canceled += context => _inputable.InteractState = false;

            _inputActions.Player.TriggerRight.performed += context => actionRight(true);
            _inputActions.Player.TriggerRight.canceled += context => actionRight(false);

            _inputActions.Player.ActionMiddle.performed += context => actionMiddle(true);
            _inputActions.Player.ActionMiddle.canceled += context => actionMiddle(false);

            _inputActions.Player.BumperRight.performed += context => actionLeft(true);
            _inputActions.Player.BumperRight.canceled += context => actionLeft(false);

            _inputActions.Player.TriggerLeft.performed += context => _inputable.ControlState = true;
            _inputActions.Player.TriggerLeft.canceled += context => _inputable.ControlState = false;

            _inputActions.Player.BumperLeft.performed += context => _inputable.ShiftState = true;
            _inputActions.Player.BumperLeft.canceled += context => _inputable.ShiftState = false;
        }
    }

    private void actionMiddle(bool state)
    {
        _inputable.ActionMiddleState = state;

        if (InputTargetMode == TargetMode.MiddleAction)
        {
            if (state) checkTargetPosition();
        }
    }

    private void actionLeft(bool state)
    {
        _inputable.ActionLeftState = state;

        if (InputTargetMode == TargetMode.LeftAction)
        {
            if (state) checkTargetPosition();
        }
    }

    private void actionRight(bool state)
    {
        _inputable.ActionRightState = state;

        if (InputTargetMode == TargetMode.RightAction)
        {
            if (state) checkTargetPosition();
        }
    }

    private void checkTargetPosition()
    {
        if (MoveDirectionMode != MoveMode.Input)
        {
            RaycastHit hit;

            if (Physics.Raycast(_camera.ScreenPointToRay(_inputable.PointerScreenPosition), out hit))
            {
                if ((TargetRequiredLayers.value & (1 << hit.collider.transform.gameObject.layer)) > 0)
                {
                    _targetPosition = new Target(transform, hit.collider.transform, hit.point);
                }
            }
        }
    }

    private void Update()
    {
        _inputable.PointerScreenPosition = _inputActions.Player.Pointer.ReadValue<Vector2>();
        _inputable.LookDelta = _inputActions.Player.Look.ReadValue<Vector2>();

        readMoveInput();
    }

    private void readMoveInput()
    {
        Vector2 inputMove = _inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 cameraDirection = _cameraTransform.forward.normalized;
        Vector3 moveDirection = (Vector3.ProjectOnPlane(cameraDirection, Vector3.up) * inputMove.y + _cameraTransform.right * inputMove.x).normalized;
        Vector2 inputMoveVector = new Vector2(moveDirection.x, moveDirection.z);

        if (MoveDirectionMode == MoveMode.Input)
        {
            _inputable.MoveVector = inputMoveVector;
        }
        else if (MoveDirectionMode == MoveMode.Target)
        {
            if (_targetPosition.IsTargetExists)
            {
                if (_targetPosition.GetHorizontalDistance > 0.1f)
                {
                    _inputable.MoveVector = _targetPosition.GetHorizontalDirection;

                    return;
                }

                _inputable.MoveVector = Vector2.zero;
                ClearTarget();
            }
        }
        else
        {
            if (_targetPosition.IsTargetExists)
            {
                if (_targetPosition.GetHorizontalDistance > 0.1f)
                {
                    _inputable.MoveVector = _targetPosition.GetHorizontalDirection;

                    return;
                }

                ClearTarget();
            }

            _inputable.MoveVector = inputMoveVector;
        }
    }

    public void ClearTarget() => _targetPosition.Clear();

    private void OnEnable() => _inputActions?.Enable();
    private void OnDisable() => _inputActions?.Disable();
}

#if UNITY_EDITOR
[ExecuteInEditMode]
[CustomEditor(typeof(InputPlayerController))]
public class InputPlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InputPlayerController _myTarget = (InputPlayerController)target;

        // Script Link
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Model", MonoScript.FromMonoBehaviour(_myTarget), typeof(MonoScript), false);
        EditorGUI.EndDisabledGroup();

        Rect scriptRect = GUILayoutUtility.GetLastRect();
        EditorGUIUtility.AddCursorRect(scriptRect, MouseCursor.Arrow);

        if (GUI.Button(scriptRect, "", GUIStyle.none))
        {
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(_myTarget)), 0);
        }

        // Show Enum Mode
        if (_myTarget.MoveDirectionMode == InputPlayerController.MoveMode.Input)
        {
            _myTarget.MoveDirectionMode = (InputPlayerController.MoveMode)EditorGUILayout.EnumPopup("Move Mode", _myTarget.MoveDirectionMode);
        }
        else
        {
            _myTarget.MoveDirectionMode = (InputPlayerController.MoveMode)EditorGUILayout.EnumPopup("Move Mode", _myTarget.MoveDirectionMode);
            _myTarget.InputTargetMode = (InputPlayerController.TargetMode)EditorGUILayout.EnumPopup("Target Mode", _myTarget.InputTargetMode);
            _myTarget.TargetRequiredLayers = EditorGUILayout.MaskField("Target Required Layers", _myTarget.TargetRequiredLayers, UnityEditorInternal.InternalEditorUtility.layers);

            if (GUI.changed)
            {
                _myTarget.ClearTarget();
            }
        }
    }
}
#endif