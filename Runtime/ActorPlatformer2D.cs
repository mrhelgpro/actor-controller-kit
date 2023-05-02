using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class ActorPlatformer2D : Actor
    {
        public void AddComponents()
        {
            gameObject.AddThisComponent<Inputable>();
            gameObject.AddThisComponent<Animatorable>();
            gameObject.AddThisComponent<Movable>();
            gameObject.AddThisComponent<Positionable2D>();

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

        public void RemoveComponents()
        {
            gameObject.RemoveComponent<Inputable>();
            gameObject.RemoveComponent<Animatorable>();
            gameObject.RemoveComponent<Movable>();
            gameObject.RemoveComponent<Positionable2D>();
            gameObject.RemoveComponent<CircleCollider2D>();
            gameObject.RemoveComponent<Rigidbody2D>();
        }
    }
}
