using System;
using UnityEngine;

namespace Actormachine
{
    /// <summary> Model - to receive input data. </summary>
    [AddComponentMenu("Actormachine/Model/Inputable")]
    public class Inputable : Model
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
        [Range(0, 1)] public float ActionRightValue;

        public bool ControlState;                       // Left Ctrl           Left Trigget      L2              Left Trigget
        [Range(0, 1)] public float ControlValue;

        public bool ShiftState;                         // Left Shift          Left Bumper       L1              Left Shoulder

        public Target TargetInteraction;
    }

    [Serializable]
    public class InputableCompare
    {
                                     // KEYBOARD            X-BOX             DUALSHOCK       GAMEPAD
        public bool Option;          // Q                   Y                 Triangle        North
        public bool Cancel;          // Backspace / C       B                 Circle          East
        public bool Motion;          // Space               A                 Cross           South
        public bool Interact;        // E                   X                 Square          West

        public bool ActionLeft;      // Left Mouse          Right Bumper      R1              Right Shoulder
        public bool ActionMiddle;    // Middle Mouse
        public bool ActionRight;     // Right Mouse         Right Trigger     R2              Right Trigger

        public bool Control;         // Left Ctrl           Left Trigget      L2              Left Trigget
        public bool Shift;           // Left Shift          Left Bumper       L1 

        public bool IsEquals(Inputable inputable)
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

    /// <summary> Parent class from which all "Input Controllers: should inherit. </summary>
    public class InputController : ActormachineComponentBase
    {
        public static Vector2 PointerScreenPosition;

        protected Inputable inputable;

        protected new void Awake()
        {
            base.Awake();

            inputable = AddComponentInRoot<Inputable>();
        }
    }

    public interface IInputConteroller
    { 
        // TODO
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