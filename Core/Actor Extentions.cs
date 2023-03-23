using UnityEngine;

public static class ActorExtentionComponents
{
    // Finds the required Component on <Actor> gets or instantiates
    public static T AddThisComponent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() == null ? gameObject.AddComponent<T>() : gameObject.GetComponent<T>();
    }

    public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.GetComponent<T>() != null) Object.DestroyImmediate(gameObject.GetComponent<T>());
    }
}

public static class ActorExtentionMovements
{
    public static float HeightToForce(this int height, float gravityScale = 1)
    {
        float force;

        switch (height)
        {
            case 1:
                force = 4.532f;
                break;
            case 2:
                force = 6.375f;
                break;
            case 3:
                force = 7.777f;
                break;
            case 4:
                force = 8.965f;
                break;
            case 5:
                force = 10.01f;
                break;
            default:
                force = height * 2;
                Debug.Log("Force not calculated for height " + height);
                break;
        }

        float gravity = 0.425f * gravityScale + 0.575f;

        return force * gravity;
    }
}