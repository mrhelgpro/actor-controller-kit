using System;
using UnityEngine;

namespace AssemblyActorCore
{
    public enum KeyState { None, Down, Press, Up, Click, DoubleClick }

    [Serializable]
    public class Key
    {
        public KeyState State = KeyState.None;
        public bool IsDown => State == KeyState.Down;
        public bool IsPress => State == KeyState.Press;
        public bool IsUp => State == KeyState.Up;
        public bool IsClick => State == KeyState.Click;
        public bool IsDoubleClick => State == KeyState.DoubleClick;

        private KeyState _previousState = KeyState.None;
        private float _lastTime = 0;
        private float _duration => Time.time - _lastTime;

        public void SetState(bool state)
        {
            _previousState = State;

            if (state == true)
            {
                if (_duration <= 0.25f)
                {
                    State = KeyState.DoubleClick;
                }
                else
                {
                    State = KeyState.Down;
                }
            }
            else
            {
                State = KeyState.None;
            }

            if (State != _previousState && State != KeyState.None) Debug.Log(State + " - " + _duration);

            if (State != _previousState) _lastTime = Time.time;
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

        public void SetState(Key key, bool value)
        {
            key.SetState(value);

            //key.State = value == true ? KeyState.Down : KeyState.None;
        }
    }
}