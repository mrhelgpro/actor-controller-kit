using UnityEngine;
using AssemblyActorCore;

public sealed class InputPlayerController : MonoBehaviour
{
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

            _inputActions.Player.TriggerRight.performed += context => _inputable.ActionRight = true;
            _inputActions.Player.TriggerRight.canceled += context => _inputable.ActionRight = false;

            _inputActions.Player.BumperRight.performed += context => _inputable.ActionLeft = true;
            _inputActions.Player.BumperRight.canceled += context => _inputable.ActionLeft = false;

            _inputActions.Player.TriggerLeft.performed += context => _inputable.Control = true;
            _inputActions.Player.TriggerLeft.canceled += context => _inputable.Control = false;

            _inputActions.Player.BumperLeft.performed += context => _inputable.Shift = true;
            _inputActions.Player.BumperLeft.canceled += context => _inputable.Shift = false;
        }
    }

    private void Update()
    {
        _inputable.Move = _inputActions.Player.Move.ReadValue<Vector2>();
        _inputable.Look.Delta = _inputActions.Player.Look.ReadValue<Vector2>();

        if (_inputable.Look.Freez == false)
        {
            _inputable.Look.Value += _inputable.Look.Delta;
        }
    }

    private void OnEnable() => _inputActions?.Enable();

    private void OnDisable() => _inputActions?.Disable();
}