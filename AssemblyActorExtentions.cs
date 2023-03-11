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

        public static Rigidbody GetRigidbidyComponent(this GameObject gameObject) 
        {
            SphereCollider sphereCollider = gameObject.GetActorComponent<SphereCollider>();
            sphereCollider.radius = 0.25f;
            sphereCollider.center = new Vector3(0, sphereCollider.radius, 0);

            Rigidbody rigidbody = gameObject.GetActorComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.freezeRotation = true;

            return rigidbody;
        }

        public static Rigidbody2D GetRigidbidy2DComponent(this GameObject gameObject)
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