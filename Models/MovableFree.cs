using UnityEngine;

namespace AssemblyActorCore
{
    public class MovableFree : Movable
    {
        public override void FreezAll() { }

        public override void FreezRotation() { }

        public override void MoveToDirection(Vector3 direction, float speed)
        {
            mainTransform.position += direction * speed * GetSpeedScale;
        }

        public override void Jump(float force)
        {

        }
    }
}