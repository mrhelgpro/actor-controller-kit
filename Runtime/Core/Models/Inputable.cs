using System;
using UnityEngine;

namespace Actormachine
{
    /// <summary> Model - to receive input data. </summary>
    public class Inputable : ActorBehaviour
    {
                                                        // KEYBOARD            X-BOX             DUALSHOCK       GAMEPAD
        public bool MenuState;                          // Escape
        public Vector2 PointerScreenPosition;           // Mouse Position                        Options         Start
        public Vector2 MoveVector;                      // WASD - Movement     Left Stick        Left Stick      Left Stick
        public Vector2 LookDelta;

        public bool OptionState;                        // Q                   Y                 Triangle        North
        public bool CancelState;                        // Backspace / C       B                 Circle          East
        public bool MotionState;                        // Space               A                 Cross           South
        public bool InteractState;                      // E                   X                 Square          West

        public bool ActionLeftState;                    // Left Mouse          Right Bumper      R1              Right Shoulder
        public bool ActionMiddleState;                  // Middle Mouse          
        [Range(-1, 1)] public float ActionMiddleScroll; // Scroll Mouse
        public bool ActionRightState;                   // Right Mouse         Right Trigger     R2              Right Trigge
        [Range (0, 1)] public float ActionRightValue;

        public bool ControlState;                       // Left Ctrl           Left Trigget      L2              Left Trigget
        [Range(0, 1)] public float ControlValue;

        public bool ShiftState;                         // Left Shift          Left Bumper       L1              Left Shoulder

        public Target TargetInteraction;
    }

    /// <summary> Parent class from which all "Input Controllers: should inherit. </summary>
    public class InputController : ActorBehaviour
    {
        protected Inputable inputable;

        protected new void Awake()
        {
            base.Awake();

            inputable = GetComponentInRoot<Inputable>();
        }
    }

    /// <summary> Extensions for the input system. </summary>
    public static class InputSystem
    {
        public static void CursorVisible(bool state)
        {
            Cursor.lockState = state == true ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = state;
        }
    }
}