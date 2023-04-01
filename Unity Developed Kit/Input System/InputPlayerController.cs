using UnityEngine;
using AssemblyActorCore;

public class InputPlayerController : MonoBehaviour
{
    private InputActions _inputActions;
    protected Inputable inputable;
    private AssemblyActorCore.Input _input => inputable.Input;

    private void Awake()
    {
        inputable = gameObject.GetComponentInParent<Inputable>();
        _inputActions = new InputActions();

        _inputActions.Player.Menu.performed += context => _input.Menu = true;
        _inputActions.Player.Menu.canceled += context => _input.Menu = false;

        _inputActions.Player.North.performed += context => _input.Option = true;
        _inputActions.Player.North.canceled += context => _input.Option = false;

        _inputActions.Player.East.performed += context => _input.Cancel = true;
        _inputActions.Player.East.canceled += context => _input.Cancel = false;

        _inputActions.Player.South.performed += context => _input.Motion = true;
        _inputActions.Player.South.canceled += context => _input.Motion = false;

        _inputActions.Player.West.performed += context => _input.Interact = true;
        _inputActions.Player.West.canceled += context => _input.Interact = false;

        _inputActions.Player.TriggerRight.performed += context => _input.ActionRight = true;
        _inputActions.Player.TriggerRight.canceled += context => _input.ActionRight = false;

        _inputActions.Player.BumperRight.performed += context => _input.ActionLeft = true;
        _inputActions.Player.BumperRight.canceled += context => _input.ActionLeft = false;

        _inputActions.Player.TriggerLeft.performed += context => _input.Control = true;
        _inputActions.Player.TriggerLeft.canceled += context => _input.Control = false;

        _inputActions.Player.BumperLeft.performed += context => _input.Shift = true;
        _inputActions.Player.BumperLeft.canceled += context => _input.Shift = false;
    }

    private void Update()
    {
        _input.Move = _inputActions.Player.Move.ReadValue<Vector2>();
        _input.Look = _inputActions.Player.Look.ReadValue<Vector2>();
    }

    private void OnEnable() => _inputActions.Enable();

    private void OnDisable() => _inputActions.Disable();
}