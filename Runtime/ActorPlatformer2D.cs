using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class ActorPlatformer2D : Actor
    {
        public override void AddComponents()
        {
            gameObject.AddThisComponent<Inputable>();
            gameObject.AddThisComponent<MovablePlatformer>();
            gameObject.AddThisComponent<PositionablePlatformer>();

            CircleCollider2D collider = gameObject.AddThisComponent<CircleCollider2D>();
            collider.isTrigger = false;
            collider.radius = 0.25f;
            collider.offset = new Vector2(0, collider.radius);

            Rigidbody2D rigidbody = gameObject.AddThisComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            rigidbody.simulated = true;
            rigidbody.useAutoMass = false;
            rigidbody.mass = 1;
            rigidbody.drag = 0;
            rigidbody.angularDrag = 0.05f;
            rigidbody.gravityScale = 1;
            rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigidbody.freezeRotation = true;
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(ActorPlatformer2D))]
    public class ActorPlatformer2DEditor : ActorEditor
    {
        public override void RemoveComponents()
        {
            gameObject.RemoveComponent<Inputable>();
            gameObject.RemoveComponent<MovablePlatformer>();
            gameObject.RemoveComponent<PositionablePlatformer>();
            gameObject.RemoveComponent<CircleCollider2D>();
            gameObject.RemoveComponent<Rigidbody2D>();
        }
    }
#endif
}
