using System;
using UnityEngine;

namespace AssemblyActorCore
{
    public enum KeyState { None, Down, Press, Up, Click, DoubleClick }


    public class Inputable : MonoBehaviour
    {
        public bool Menu;

        //[Header("Stick Left / Right")]
        public Vector2 Direction;      // WASD - Movement
        public Vector2 Rotation;       // Mouse - Look

        //[Header("Button")]
        public bool Option;              // Q - Option
        public bool Cancel;               // Backspace / C - Back / Cancel
        public bool Motion;              // Space - Jump / Evade
        public bool Interact;               // E - Interact

        //[Header("Trigger / Bumper")]
        public bool ActionRight;       // Right Mouse - Action B
        public bool ActionLeft;        // Left Mouse - Action A
        public bool Control;        // Left Ctrl - Aim
        public bool Shift;         // Left Shift - Sprint / Block / Evade

        public bool IsEqual(Inputable input)
        {
            bool value = false;

            if (Option) value = Option == input.Option;
            if (Cancel) value = Cancel == input.Cancel;
            if (Motion) value = Motion == input.Motion;
            if (Interact) value = Interact == input.Interact;

            return value;
        }
    }
}