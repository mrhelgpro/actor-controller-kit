using UnityEngine;
using UnityEditor;
using AssemblyActorCore;

public sealed class InputPlayerController : MonoBehaviour
{
    public enum MoveMode { Input, Target, Both }
    public MoveMode MoveDirectionMode = MoveMode.Input;
    public enum TargetMode { LeftAction, RightAction }
    public TargetMode InputTargetMode = TargetMode.LeftAction;

    public LayerMask TargetRequiredLayers;

    private Target _targetPosition;
    private InputActions _inputActions;
    private Inputable _inputable;

    private void Awake()
    {
        _inputable = gameObject.GetComponentInParent<Inputable>();

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

            _inputActions.Player.North.performed += context => _inputable.Option = true;
            _inputActions.Player.North.canceled += context => _inputable.Option = false;

            _inputActions.Player.East.performed += context => _inputable.Cancel = true;
            _inputActions.Player.East.canceled += context => _inputable.Cancel = false;

            _inputActions.Player.South.performed += context => _inputable.Motion = true;
            _inputActions.Player.South.canceled += context => _inputable.Motion = false;

            _inputActions.Player.West.performed += context => _inputable.Interact = true;
            _inputActions.Player.West.canceled += context => _inputable.Interact = false;

            _inputActions.Player.TriggerRight.performed += context => actionRight(true);
            _inputActions.Player.TriggerRight.canceled += context => actionRight(false);

            _inputActions.Player.BumperRight.performed += context => actionLeft(true);
            _inputActions.Player.BumperRight.canceled += context => actionLeft(false);

            _inputActions.Player.TriggerLeft.performed += context => _inputable.Control = true;
            _inputActions.Player.TriggerLeft.canceled += context => _inputable.Control = false;

            _inputActions.Player.BumperLeft.performed += context => _inputable.Shift = true;
            _inputActions.Player.BumperLeft.canceled += context => _inputable.Shift = false;
        }
    }

    private void actionLeft(bool state)
    {
        _inputable.ActionLeft = state;

        if (InputTargetMode == TargetMode.LeftAction)
        {
            if (state) checkTargetPosition();
        }
    }

    private void actionRight(bool state)
    {
        _inputable.ActionRight = state;

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

            if (Physics.Raycast(Camera.main.ScreenPointToRay(_inputable.Pointer), out hit))
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
        _inputable.Pointer = _inputActions.Player.Pointer.ReadValue<Vector2>();

        readMoveInput();
        readLookInput();
    }

    private void readMoveInput()
    {
        if (MoveDirectionMode == MoveMode.Input)
        {
            _inputable.Move = _inputActions.Player.Move.ReadValue<Vector2>();
        }
        else if (MoveDirectionMode == MoveMode.Target)
        {
            if (_targetPosition.IsTargetExists)
            {
                if (_targetPosition.GetHorizontalDistance > 0.1f)
                {
                    _inputable.Move = _targetPosition.GetHorizontalDirection;

                    return;
                }

                _inputable.Move = Vector2.zero;
                ClearTarget();
            }
        }
        else
        {
            if (_targetPosition.IsTargetExists)
            {
                if (_targetPosition.GetHorizontalDistance > 0.1f)
                {
                    _inputable.Move = _targetPosition.GetHorizontalDirection;

                    return;
                }

                ClearTarget();
            }

            _inputable.Move = _inputActions.Player.Move.ReadValue<Vector2>();
        }
    }

    private void readLookInput()
    {
        _inputable.Look.Delta = _inputActions.Player.Look.ReadValue<Vector2>();
        _inputable.Look.Value += _inputable.Look.Delta * _inputable.Look.Sensitivity;
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