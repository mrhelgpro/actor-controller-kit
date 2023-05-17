using UnityEngine;

namespace Actormachine
{
    /// <summary> To deactivate the Presenters. </summary>
    public class DeactivatorByInput : Deactivator
    {
        public enum Mode { None, RepeatedPressing, ActiveWhileHolding }

        [Tooltip("The event when the controller will be deactivated")]
        public Mode DeactivateMode = Mode.None;

        // KEYBOARD            X-BOX             DUALSHOCK       GAMEPAD
        public bool Option;          // Q                   Y                 Triangle        North
        public bool Cancel;          // Backspace / C       B                 Circle          East
        public bool Motion;          // Space               A                 Cross           South
        public bool Interact;        // E                   X                 Square          West

        public bool ActionLeft;      // Left Mouse          Right Bumper      R1              Right Shoulder
        public bool ActionMiddle;    // Middle Mouse
        public bool ActionRight;     // Right Mouse         Right Trigger     R2              Right Trigger

        public bool Control;         // Left Ctrl           Left Trigget      L2              Left Trigget
        public bool Shift;           // Left Shift          Left Bumper       L1              Left Shoulder

        protected Inputable inputable;

        public override void Initiation()
        {
            // Get components using "GetComponentInRoot" to create them on <Actor>
            inputable = GetComponentInRoot<Inputable>();
        }

        private bool _isRepeatedPressing = false;

        public override void UpdateLoop()
        {
            if (DeactivateMode == Mode.RepeatedPressing)
            {
                if (_isButtonPress == false)
                {
                    _isRepeatedPressing = true;
                }

                if (_isRepeatedPressing == true)
                {
                    if (_isButtonPress == true)
                    {
                        Deactivate();

                        _isRepeatedPressing = false;
                    }
                }
            }
            else if (DeactivateMode == Mode.ActiveWhileHolding)
            {
                if (_isButtonPress == false)
                {
                    Deactivate();
                }
            }
        }

        private bool _isButtonPress
        {
            get
            {
                if (Option == true && inputable.OptionState == false) return false;
                if (Cancel == true && inputable.CancelState == false) return false;
                if (Motion == true && inputable.MotionState == false) return false;
                if (Interact == true && inputable.InteractState == false) return false;

                if (ActionLeft == true && inputable.ActionLeftState == false) return false;
                if (ActionMiddle == true && inputable.ActionMiddleState == false) return false;
                if (ActionRight == true && inputable.ActionRightState == false) return false;

                if (Control == true && inputable.ControlState == false) return false;
                if (Shift == true && inputable.ShiftState == false) return false;

                return true;
            }
        }
    }
}