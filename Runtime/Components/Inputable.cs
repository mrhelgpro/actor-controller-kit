using System;
using UnityEngine;

namespace AssemblyActorCore
{
    public class Inputable : ModelComponent
    {
                                     // KEYBOARD            X-BOX             DUALSHOCK       GAMEPAD
        public bool Menu;            // Escape
        public Vector2 Pointer;      // Mouse Position                        Options         Start
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
 
        public Target Interaction;
    }

    [Serializable]
    public struct InputVector
    {
        public Vector2 Value;
        public Vector2 Delta;
        [Range(0, 10)] public float Sensitivity;
    }
}