using System;
using UnityEngine;

namespace AssemblyActorCore
{
    public class Inputable : MonoBehaviour
    {
        public Input Input;
    }

    [Serializable]
    public class Input
    {
                                     // KEYBOARD           X-BOX              DUALSHOCK       GAMEPAD

        public bool Menu;            // Escape

        public float MoveHorizontal; // AD - Movement     Left Stick        Left Stick      Left Stick
        public float MoveVertical;   // WS - Movement

        public float LookHorizontal; // Mouse - Look        Right Stick       Right Stick     Right Stick
        public float LookVertical;

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
            if (MoveHorizontal != 0 && input.MoveHorizontal != MoveHorizontal) return false;
            if (MoveVertical != 0 && input.MoveVertical != MoveVertical) return false;

            if (LookHorizontal != 0 && input.LookHorizontal != LookHorizontal) return false;
            if (LookVertical != 0 && input.LookVertical != LookVertical) return false;

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