using UnityEngine;

namespace AssemblyActorCore
{
    [ExecuteInEditMode]
    public class TargetForCamera : MonoBehaviour
    {
        [Range(0, 90)] public int Angle = 0;
        [Range(0, 5)] public float Height = 1;
        [Range(1, 20)] public int Distance = 5;
        [Range(0, 2)] public float DampTime = 0.5f;
        [Range(0, 100)] public float AngleSpeed = 0.5f;
        [HideInInspector] public Transform Transform;
        [HideInInspector] public ActorCamera ActorCamera;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying == false)
            {
                Transform = transform;
                ActorCamera?.PreviewTheTarget(this);
            }
        }
#endif

        private void Awake() => Transform = transform;
    }
}