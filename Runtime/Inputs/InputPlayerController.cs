using UnityEngine;

namespace Actormachine
{
    public sealed class InputPlayerController : InputController
    {
        public enum MoveMode { Input, Target, Both }
        public enum TargetMode { LeftAction, MiddleAction, RightAction }

        [Range(0, 1)] public float PointerSensitivityX = 0.5f;
        [Range(0, 1)] public float PointerSensitivityY = 0.5f;

        public MoveMode MoveDirectionMode = MoveMode.Input;
        public TargetMode InputTargetMode = TargetMode.LeftAction;

        public LayerMask LayerMask;

        private Target _targetPosition;
        private Camera _camera;
        private Transform _cameraTransform;
        private InputActions _inputActions;

        private new void Awake()
        {
            base.Awake();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            PointerScreenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

            _camera = Camera.main;
            _cameraTransform = _camera.transform;

            _inputActions = new InputActions();

            _inputActions.Player.Menu.performed += context => inputable.MenuState = true;
            _inputActions.Player.Menu.canceled += context => inputable.MenuState = false;

            _inputActions.Player.North.performed += context => inputable.OptionState = true;
            _inputActions.Player.North.canceled += context => inputable.OptionState = false;

            _inputActions.Player.East.performed += context => inputable.CancelState = true;
            _inputActions.Player.East.canceled += context => inputable.CancelState = false;

            _inputActions.Player.South.performed += context => inputable.MotionState = true;
            _inputActions.Player.South.canceled += context => inputable.MotionState = false;

            _inputActions.Player.West.performed += context => inputable.InteractState = true;
            _inputActions.Player.West.canceled += context => inputable.InteractState = false;

            _inputActions.Player.TriggerRight.performed += context => actionRight(true);
            _inputActions.Player.TriggerRight.canceled += context => actionRight(false);

            _inputActions.Player.ActionMiddle.performed += context => actionMiddle(true);
            _inputActions.Player.ActionMiddle.canceled += context => actionMiddle(false);

            _inputActions.Player.BumperRight.performed += context => actionLeft(true);
            _inputActions.Player.BumperRight.canceled += context => actionLeft(false);

            _inputActions.Player.TriggerLeft.performed += context => inputable.ControlState = true;
            _inputActions.Player.TriggerLeft.canceled += context => inputable.ControlState = false;

            _inputActions.Player.BumperLeft.performed += context => inputable.ShiftState = true;
            _inputActions.Player.BumperLeft.canceled += context => inputable.ShiftState = false;
        }

        private void actionMiddle(bool state)
        {
            inputable.ActionMiddleState = state;

            if (InputTargetMode == TargetMode.MiddleAction)
            {
                if (state) checkTargetPosition();
            }
        }

        private void actionLeft(bool state)
        {
            inputable.ActionLeftState = state;

            if (InputTargetMode == TargetMode.LeftAction)
            {
                if (state) checkTargetPosition();
            }
        }

        private void actionRight(bool state)
        {
            inputable.ActionRightState = state;

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

                if (Physics.Raycast(_camera.ScreenPointToRay(inputable.PointerScreenPosition), out hit, Mathf.Infinity, LayerMask))
                {
                    _targetPosition = new Target(transform, hit.collider.transform, hit.point);
                }
            }
        }

        private void Update()
        {
            // Get Look Delta
            float x = _inputActions.Player.Look.ReadValue<Vector2>().x * PointerSensitivityX;
            float y = _inputActions.Player.Look.ReadValue<Vector2>().y * PointerSensitivityY;

            inputable.LookDelta = new Vector2(x, y);

            // Get Pointer Screen Position, for Mouse and Gamepad 
            PointerScreenPosition += Vector2.Scale(inputable.LookDelta, new Vector2(1f, -1f));
            PointerScreenPosition = new Vector2(Mathf.Clamp(PointerScreenPosition.x, 0f, Screen.width), Mathf.Clamp(PointerScreenPosition.y, 0f, Screen.height));

            inputable.PointerScreenPosition = PointerScreenPosition;

            // Get Pointer Screen Position, for Mouse
            //inputable.PointerScreenPosition = _inputActions.Player.Pointer.ReadValue<Vector2>();

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
                inputable.MoveVector = inputMoveVector;
            }
            else if (MoveDirectionMode == MoveMode.Target)
            {
                if (_targetPosition.IsExists)
                {
                    if (_targetPosition.GetDistanceHorizontal > 0.1f)
                    {
                        inputable.MoveVector = _targetPosition.GetDirectionHorizontal;

                        return;
                    }

                    inputable.MoveVector = Vector2.zero;
                    ClearTarget();
                }
            }
            else if (MoveDirectionMode == MoveMode.Both)
            {
                if (_targetPosition.IsExists)
                {
                    if (_targetPosition.GetDistanceHorizontal > 0.1f)
                    {
                        inputable.MoveVector = _targetPosition.GetDirectionHorizontal;

                        return;
                    }

                    ClearTarget();
                }

                inputable.MoveVector = inputMoveVector;
            }
        }

        public void ClearTarget() => _targetPosition.Clear();

        private void OnEnable() => _inputActions?.Enable();
        private void OnDisable() => _inputActions?.Disable();
    }
}