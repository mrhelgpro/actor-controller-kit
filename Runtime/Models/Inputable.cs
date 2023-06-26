using System;
using UnityEngine;

namespace Actormachine
{
    public enum ButtonState { Up, Down }

    /// <summary> Model - to receive input data. </summary>
    [AddComponentMenu("Actormachine/Model/Inputable")]
    public class Inputable : Model
    {
                                                        // KEYBOARD            X-BOX             DUALSHOCK       GAMEPAD
        public ButtonState MenuState;                   // Escape
        public Vector2 MoveVector;                      // WASD - Movement     Left Stick        Left Stick      Left Stick
        public Vector2 LookDelta;

        public ButtonState OptionState;                 // Q                   Y                 Triangle        North
        public ButtonState CancelState;                 // Backspace / C       B                 Circle          East
        public ButtonState MotionState;                 // Space               A                 Cross           South
        public ButtonState InteractState;               // E                   X                 Square          West

        public ButtonState ActionLeftState;             // Left Mouse          Right Bumper      R1              Right Shoulder
        public ButtonState ActionMiddleState;           // Middle Mouse          
        [Range(-1, 1)] public float ActionMiddleScroll; // Scroll Mouse
        public ButtonState ActionRightState;            // Right Mouse         Right Trigger     R2              Right Trigge
        [Range(0, 1)] public float ActionRightValue;

        public ButtonState ControlState;                // Left Ctrl           Left Trigget      L2              Left Trigget
        [Range(0, 1)] public float ControlValue;

        public ButtonState ShiftState;                  // Left Shift          Left Bumper       L1              Left Shoulder

        public Target TargetInteraction;
    }

    [Serializable]
    public class InputableCompare
    {
                                                    // KEYBOARD            X-BOX             DUALSHOCK       GAMEPAD
        public ButtonState OptionState;             // Q                   Y                 Triangle        North
        public ButtonState CancelState;             // Backspace / C       B                 Circle          East
        public ButtonState MotionState;             // Space               A                 Cross           South       
        public ButtonState InteractState;           // E                   X                 Square          West

        public ButtonState ActionLeftState;         // Left Mouse          Right Bumper      R1              Right Shoulder
        public ButtonState ActionMiddleState;       // Middle Mouse
        public ButtonState ActionRightState;        // Right Mouse         Right Trigger     R2              Right Trigger

        public ButtonState ControlState;            // Left Ctrl           Left Trigget      L2              Left Trigget
        public ButtonState ShiftState;              // Left Shift          Left Bumper       L1 

        public bool IsEquals(Inputable inputable)
        {
            if (OptionState == ButtonState.Down && inputable.OptionState != ButtonState.Down) return false;
            if (CancelState == ButtonState.Down && inputable.CancelState != ButtonState.Down) return false;
            if (MotionState == ButtonState.Down && inputable.MotionState != ButtonState.Down) return false;
            if (InteractState == ButtonState.Down && inputable.InteractState != ButtonState.Down) return false;

            if (ActionLeftState == ButtonState.Down && inputable.ActionLeftState != ButtonState.Down) return false;
            if (ActionMiddleState == ButtonState.Down && inputable.ActionMiddleState != ButtonState.Down) return false;
            if (ActionRightState == ButtonState.Down && inputable.ActionRightState != ButtonState.Down) return false;

            if (ControlState == ButtonState.Down && inputable.ControlState != ButtonState.Down) return false;
            if (ShiftState == ButtonState.Down && inputable.ShiftState != ButtonState.Down) return false;

            return true;
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