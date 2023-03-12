using UnityEngine;

namespace AssemblyActorCore
{
    public static class Extension
    {
        // Finds the required Model on <Actor> gets or instantiates
        public static T GetActorModel<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            gameObject = gameObject.GetComponentInParent<Actor>().gameObject;

            return gameObject.GetComponent<T>() == null ? gameObject.AddComponent<T>() : gameObject.GetComponent<T>();
        }

        // Finds the required Component on <Actor> gets or instantiates
        public static T GetActorComponent<T>(this GameObject gameObject) where T : Component
        {
            gameObject = gameObject.GetComponentInParent<Actor>().gameObject;

            return gameObject.GetComponent<T>() == null ? gameObject.AddComponent<T>() : gameObject.GetComponent<T>();
        }
        /*
        public static Movable GetMovable(this GameObject gameObject)
        {
            Movable movable = null;

            Actor actor = gameObject.GetComponentInParent<Actor>();

            switch (actor.Mode)
            {
                case EnumMode.Free:
                    movable = gameObject.GetActorModel<MovableFree>();
                    break;
                case EnumMode.ThirdPerson:
                    movable = gameObject.GetActorModel<MovableThirdPerson>();
                    break;
                case EnumMode.Platformer:
                    movable = gameObject.GetActorModel<MovablePlatformer>();
                    break;
            }

            return movable;
        }
        */
        public static Rigidbody GetRigidbodyComponent(this GameObject gameObject) 
        {
            SphereCollider sphereCollider = gameObject.GetActorComponent<SphereCollider>();
            sphereCollider.radius = 0.25f;
            sphereCollider.center = new Vector3(0, sphereCollider.radius, 0);

            Rigidbody rigidbody = gameObject.GetActorComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.freezeRotation = true;

            return rigidbody;
        }

        public static Rigidbody2D GetRigidbody2DDComponent(this GameObject gameObject)
        {
            CircleCollider2D circleCollider2D = gameObject.GetActorComponent<CircleCollider2D>();
            circleCollider2D.radius = 0.25f;
            circleCollider2D.offset = new Vector2(0, circleCollider2D.radius);

            Rigidbody2D rigidbody = gameObject.gameObject.GetActorComponent<Rigidbody2D>();
            rigidbody.freezeRotation = true;
            rigidbody.simulated = true;
            rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            return rigidbody;
        }
    }
}