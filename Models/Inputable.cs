using UnityEngine;

namespace AssemblyActorCore
{
    public class Inputable : MonoBehaviour
    {
        public enum Key { None, Down, Press, Click, DoubleClick }

        public Key Menu = Key.None;

        [Header("Key-PAD")]
        public Key A = Key.None;
        public Key B = Key.None;
        public Key X = Key.None;
        public Key Y = Key.None;

        [Header("Stick")]
        public Vector3 Direction;
        public Vector3 Rotation;

        [Header("Trigger")]
        public Key LT = Key.None;
        public Key RT = Key.None;

        [Header("Bumper")]
        public Key LB = Key.None;
        public Key RB = Key.None;

        [Header("D-PAD")]
        public Key L = Key.None;
        public Key R = Key.None;
        public Key U = Key.None;
        public Key D = Key.None;
    }
}
