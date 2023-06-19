using UnityEngine;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Input/Input Player Controller")]
    public sealed class InputPlayerController : MonoBehaviour
    {
        public enum MoveMode { Input, Target, InputAndTarget, Input2D }
        public enum TargetMode { LeftAction, MiddleAction, RightAction }

        public GameObject Player;
        [Range(0, 1)] public float PointerSensitivityX = 0.5f;
        [Range(0, 1)] public float PointerSensitivityY = 0.5f;

        public MoveMode MoveDirectionMode = MoveMode.Input;
        public TargetMode InputTargetMode = TargetMode.LeftAction;

        public LayerMask LayerMask;

        private Inputable _inputable;
        private Camera _camera;
        private Transform _cameraTransform;
        private InputActions _inputActions;

        private void Awake()
        {
            if (Player == null) Player = GameObject.FindGameObjectWithTag("Player");

            if (Player == null)
            {
                gameObject.SetActive(false);

                return;
            }

            _inputable = Player.GetComponentInChildren<Inputable>();

            PointerScreen.SetPosition(new Vector2(Screen.width / 2, Screen.height / 2));

            _camera = Camera.main;
            _cameraTransform = _camera.transform;

            _inputActions = new InputActions();

            _inputActions.Player.Menu.performed += context => _inputable.MenuState = true;
            _inputActions.Player.Menu.canceled += context => _inputable.MenuState = false;

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

                if (Physics.Raycast(_camera.ScreenPointToRay(PointerScreen.GetPosition), out hit, Mathf.Infinity, LayerMask))
                {
                    PointerMovement.SetPosition(hit.point);
                }
            }
        }

        private void Update()
        {
            // Get Look Delta
            float x = _inputActions.Player.Look.ReadValue<Vector2>().x * PointerSensitivityX;
            float y = _inputActions.Player.Look.ReadValue<Vector2>().y * PointerSensitivityY;

            _inputable.LookDelta = new Vector2(x, y);

            // Get Pointer Screen Position, for Mouse and Gamepad 
            Vector2 pointerScreen = PointerScreen.GetPosition + Vector2.Scale(_inputable.LookDelta, new Vector2(1f, -1f));

            PointerScreen.SetPosition(pointerScreen);

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
                if (PointerMovement.IsActive)
                {
                    if (PointerMovement.GetDistanceHorizontal(Player.transform.position) > 0.1f)
                    {
                        _inputable.MoveVector = PointerMovement.GetDirectionHorizontal(Player.transform.position);

                        return;
                    }

                    _inputable.MoveVector = Vector2.zero;
                    ClearTarget();
                }
            }
            else if (MoveDirectionMode == MoveMode.InputAndTarget)
            {
                if (PointerMovement.IsActive)
                {
                    if (inputMoveVector.magnitude > 0)
                    {
                        ClearTarget();

                        return;
                    }

                    if (PointerMovement.GetDistanceHorizontal(Player.transform.position) > 0.1f)
                    {
                        _inputable.MoveVector = PointerMovement.GetDirectionHorizontal(Player.transform.position);

                        return;
                    }

                    ClearTarget();
                }

                _inputable.MoveVector = inputMoveVector;
            }
            else if (MoveDirectionMode == MoveMode.Input2D)
            {
                _inputable.MoveVector.x = inputMove.x;
            }
        }

        public void ClearTarget() => PointerMovement.Clear();

        private void OnEnable() => _inputActions?.Enable();
        private void OnDisable() => _inputActions?.Disable();
    }
}