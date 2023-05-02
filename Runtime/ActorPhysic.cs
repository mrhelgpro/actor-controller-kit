using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class ActorPhysic : Actor
    {
        public void AddComponents()
        {
            gameObject.AddThisComponent<Inputable>();
            gameObject.AddThisComponent<Animatorable>();
            gameObject.AddThisComponent<Movable>();
            gameObject.AddThisComponent<Positionable>();

            SphereCollider collider = gameObject.AddThisComponent<SphereCollider>();
            collider.radius = 0.25f;
            collider.center = new Vector3(0, collider.radius, 0);

            Rigidbody rigidbody = gameObject.AddThisComponent<Rigidbody>();
            rigidbody.mass = 1;
            rigidbody.drag = 0;
            rigidbody.angularDrag = 0.05f;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.freezeRotation = true;
        }

        public void RemoveComponents()
        {
            gameObject.RemoveComponent<Inputable>();
            gameObject.RemoveComponent<Animatorable>();
            gameObject.RemoveComponent<Movable>();
            gameObject.RemoveComponent<Positionable>();
            gameObject.RemoveComponent<SphereCollider>();
            gameObject.RemoveComponent<Rigidbody>();
        }
    }
}