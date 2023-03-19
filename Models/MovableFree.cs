using UnityEngine;

namespace AssemblyActorCore
{
    public class MovableFree : Movable
    {
        public override void FreezAll() { }

        public override void FreezRotation() { }

        public override void MoveToDirection(Vector3 direction, float speed) => mainTransform.position += direction * speed * getSpeedScale;

        public override void Jump(float force) => mainTransform.position += Vector3.up * force;
    }
}