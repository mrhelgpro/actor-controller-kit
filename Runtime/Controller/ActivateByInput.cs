using UnityEngine;

namespace AssemblyActorCore
{
    public class ActivateByInput : Activator
    {
        public enum Mode { None, Repeat, Cancel}
        public Mode DeactivateMode = Mode.None;

                                     // KEYBOARD            X-BOX             DUALSHOCK       GAMEPAD
        public bool Option;          // Q                   Y                 Triangle        North
        public bool Cancel;          // Backspace / C       B                 Circle          East
        public bool Motion;          // Space               A                 Cross           South
        public bool Interact;        // E                   X                 Square          West

        public bool ActionRight;     // Right Mouse         Right Trigger     R2              Right Trigger
        public bool ActionLeft;      // Left Mouse          Right Bumper      R1              Right Shoulder
        public bool Control;         // Left Ctrl           Left Trigget      L2              Left Trigget
        public bool Shift;           // Left Shift          Left Bumper       L1              Left Shoulder

        protected Inputable inputable;

        private bool _previousButtonPress = false;

        private new void Awake()
        {
            base.Awake();

            inputable = GetComponentInParent<Inputable>();
        }

        public override void UpdateActivate()
        {
            if (isCurrentController == false)
            {
                setActive(true);
            }
            else
            {
                if (DeactivateMode == Mode.Repeat)
                {
                    setActive(false);
                }
                else if (DeactivateMode == Mode.Cancel)
                {
                    if (_isButtonPress == false)
                    {
                        Deactivate();
                    }
                }
            }
        }

        private void setActive(bool state)
        {
            if (_isButtonPress == true)
            {
                if (_previousButtonPress == false)
                {
                    if (state == true)
                    {
                        TryToActivate();
                    }
                    else
                    {
                        Deactivate();
                    }

                    _previousButtonPress = true;
                }
            }
            else
            {
                _previousButtonPress = false;
            }
        }

        private bool _isButtonPress
        {
            get
            {
                if (Option == true && inputable.Option == false) return false;
                if (Cancel == true && inputable.Cancel == false) return false;
                if (Motion == true && inputable.Motion == false) return false;
                if (Interact == true && inputable.Interact == false) return false;

                if (ActionRight == true && inputable.ActionRight == false) return false;
                if (ActionLeft == true && inputable.ActionLeft == false) return false;
                if (Control == true && inputable.Control == false) return false;
                if (Shift == true && inputable.Shift == false) return false;

                return true;
            }
        }
    }
}