using System;
using UnityEngine;

namespace AssemblyActorCore
{
    public enum KeyState { None, Down, Press, Up, Click, DoubleClick }

    [Serializable]
    public class Key
    {
        public bool performed = false;

        public bool IsNone => performed == false && IsClick == false;
        public bool IsDown => performed == true && _lastDuration >= 0.25f;
        public bool IsHold => performed == true && _duration >= 0.25f;
        public bool IsClick => performed == false && _lastDuration <= 0.25f;
        public bool IsDoubleClick => performed == true && _lastDuration <= 0.25f;

        private float _preTime = 0;
        private float _lastTime = 0;
        private float _duration => Time.time - _lastTime;
        private float _lastDuration => _lastTime - _preTime;

        public void SetPerformed(bool value)
        {
            performed = value;
            _preTime = _lastTime;
            _lastTime = Time.time;
        }
    }

    public class Inputable : MonoBehaviour
    {
        public Key Menu;

        [Header("Key-PAD")]
        public Key KeyA;
        public Key KeyB;
        public Key KeyX;
        public Key KeyY;

        [Header("Stick")]
        public Vector3 Direction;
        public Vector3 Rotation;

        [Header("Trigger")]
        public Key KeyLT;
        public Key KeyRT;

        [Header("Bumper")]
        public Key KeyLB;
        public Key KeyRB;

        public delegate void EventInputable();
        public EventInputable Input;

        public void InvokeInput(Key key, bool value)
        {
            key.SetPerformed(value);
            Input?.Invoke();
        }
    }
}