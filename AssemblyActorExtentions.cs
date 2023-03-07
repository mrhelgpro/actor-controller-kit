using UnityEngine;

namespace AssemblyActorCore
{
    public static class Extension
    {
        // Finds the required Component on <Actor> gets or instantiates
        public static T GetModel<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            gameObject = gameObject.GetComponentInParent<Actor>().gameObject;

            return gameObject.GetComponent<T>() == null ? gameObject.AddComponent<T>() : gameObject.GetComponent<T>();
        }

        public static void Move(this Transform transform, Vector3 velocity, float speed)
        {
            transform.position += velocity * speed;
        }

        public static void Move(this Rigidbody rigidbody, Vector3 velocity, float speed, float gravity = 1.0f)
        {
            rigidbody.isKinematic = gravity == 0;
            rigidbody.useGravity = false;
            rigidbody.MovePosition(rigidbody.position + velocity * speed);
            rigidbody.AddForce(Physics.gravity * gravity, ForceMode.Acceleration);
        }

        public static void Move(this Rigidbody2D rigidbody, Vector3 velocity, float speed, float gravity = 1.0f)
        {
            float horizontal = velocity.x * speed * 20;
            float vertical = gravity == 0 ? 0 : rigidbody.velocity.y;
            rigidbody.bodyType = gravity == 0 ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
            rigidbody.gravityScale = gravity;
            rigidbody.velocity = new Vector2(horizontal, vertical);
        }
    }
}