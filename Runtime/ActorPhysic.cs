using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class ActorPhysic : Actor
    {
        public override void AddComponents()
        {
            gameObject.AddThisComponent<Inputable>();
            gameObject.AddThisComponent<MovablePhysic>();
            gameObject.AddThisComponent<PositionablePhysic>();

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
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(ActorPhysic))]
    public class ActorPhysicEditor : ActorEditor
    {
        public override void RemoveComponents()
        {
            gameObject.RemoveComponent<Inputable>();
            gameObject.RemoveComponent<MovablePhysic>();
            gameObject.RemoveComponent<PositionablePhysic>();
            gameObject.RemoveComponent<SphereCollider>();
            gameObject.RemoveComponent<Rigidbody>();
        }
    }
#endif
}