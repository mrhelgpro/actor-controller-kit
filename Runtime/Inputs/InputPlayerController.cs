using UnityEngine;

namespace Actormachine
{
    public sealed class InputPlayerController : InputController
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

        public override void Initiation()
        {
            Bootstrap.Create<BootstrapCamera>();

            gameObject.tag = "Player";
        }

        private new void Awake()
        {
            base.Awake();

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

                if (Physics.Raycast(_camera.ScreenPointToRay(inputable.PointerScreenPosition), out hit))
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
            inputable.PointerScreenPosition = _inputActions.Player.Pointer.ReadValue<Vector2>();
            inputable.LookDelta = _inputActions.Player.Look.ReadValue<Vector2>();

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
                if (_targetPosition.IsTargetExists)
                {
                    if (_targetPosition.GetHorizontalDistance > 0.1f)
                    {
                        inputable.MoveVector = _targetPosition.GetHorizontalDirection;

                        return;
                    }

                    inputable.MoveVector = Vector2.zero;
                    ClearTarget();
                }
            }
            else
            {
                if (_targetPosition.IsTargetExists)
                {
                    if (_targetPosition.GetHorizontalDistance > 0.1f)
                    {
                        inputable.MoveVector = _targetPosition.GetHorizontalDirection;

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