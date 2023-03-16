using System;
using UnityEngine;

namespace AssemblyActorCore
{
    //public enum KeyState { None, Down, Press, Up, Click, DoubleClick }
    public class Inputable : MonoBehaviour
    {
        public Input Input;
        /*

        public bool IsEqual(Inputable input)
        {
            bool value = false;

            if (Option) value = Option == input.Option;
            if (Cancel) value = Cancel == input.Cancel;
            if (Motion) value = Motion == input.Motion;
            if (Interact) value = Interact == input.Interact;

            return value;
        }
        */
    }

    [Serializable]
    public class Input
    {
                                  // KEYBOARD           X-BOX              DUALSHOCK       GAMEPAD
        public bool Menu;         // Escape

        public Vector2 Direction; // WASD - Movement     Left Stick        Left Stick      Left Stick
        public Vector2 Rotation;  // Mouse - Look        Right Stick       Right Stick     Right Stick

        public bool Option;       // Q                   Y                 Triangle        North
        public bool Cancel;       // Backspace / C       B                 Circle          East
        public bool Motion;       // Space               A                 Cross           South
        public bool Interact;     // E                   X                 Square          West

        public bool ActionRight;  // Right Mouse         Right Trigger     R2              Right Trigger
        public bool ActionLeft;   // Left Mouse          Right Bumper      R1              Right Shoulder
        public bool Control;      // Left Ctrl           Left Trigget      L2              Left Trigget
        public bool Shift;        // Left Shift          Left Bumper       L1              Left Shoulder

        public bool IsButtonPress(Input input)
        {
            if (Option != input.Option) return false;
            if (Cancel != input.Cancel) return false;
            if (Motion != input.Motion) return false;
            if (Interact != input.Interact) return false;

            if (ActionRight != input.ActionRight) return false;
            if (ActionLeft != input.ActionLeft) return false;
            if (Control != input.Control) return false;
            if (Shift != input.Shift) return false;

            return true;
        }
    }
}