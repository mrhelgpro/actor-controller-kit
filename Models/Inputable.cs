using System;
using UnityEngine;

namespace AssemblyActorCore
{
    public class Inputable : Model
    {
        public Input Input;
        public bool FreezLook;
    }

    [Serializable]
    public class InputVector
    {
        public Vector2 Value;
        public Vector2 Delta;
    }

    [Serializable]
    public class Input
    {
                                     // KEYBOARD           X-BOX              DUALSHOCK       GAMEPAD

        public bool Menu;            // Escape
      
        public Vector2 Move;         // WASD - Movement     Left Stick        Left Stick      Left Stick
        public InputVector Look;

        public bool Option;          // Q                   Y                 Triangle        North
        public bool Cancel;          // Backspace / C       B                 Circle          East
        public bool Motion;          // Space               A                 Cross           South
        public bool Interact;        // E                   X                 Square          West

        public bool ActionRight;     // Right Mouse         Right Trigger     R2              Right Trigger
        public bool ActionLeft;      // Left Mouse          Right Bumper      R1              Right Shoulder
        public bool Control;         // Left Ctrl           Left Trigget      L2              Left Trigget
        public bool Shift;           // Left Shift          Left Bumper       L1              Left Shoulder

        public bool IsButtonPress(Input input)
        {
            if (Option == true && input.Option == false) return false;
            if (Cancel == true && input.Cancel == false) return false;
            if (Motion == true && input.Motion == false) return false;
            if (Interact == true && input.Interact == false) return false;

            if (ActionRight == true && input.ActionRight == false) return false;
            if (ActionLeft == true && input.ActionLeft == false) return false;
            if (Control == true && input.Control == false) return false;
            if (Shift == true && input.Shift == false) return false;

            return true;
        }
    }
}