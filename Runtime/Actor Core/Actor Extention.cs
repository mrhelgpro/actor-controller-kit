using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ActorExtention
{
    // Finds the required Component on <Actor> gets or instantiates
    public static T AddThisComponent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() == null ? gameObject.AddComponent<T>() : gameObject.GetComponent<T>();
    }

    public static T AddThisComponent<T>(this Transform transform) where T : Component
    {
        return transform.GetComponent<T>() == null ? transform.gameObject.AddComponent<T>() : transform.GetComponent<T>();
    }

    public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.GetComponent<T>() != null) UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<T>());
    }

    public static void RemoveComponent<T>(this Transform transform) where T : Component
    {
        if (transform.GetComponent<T>() != null) UnityEngine.Object.DestroyImmediate(transform.gameObject.GetComponent<T>());
    }

    public static Vector3 GetVector3Horizontal(this Vector2 vector) => new Vector3(vector.x, 0, vector.y);
    public static Vector3 GetVector3Vertical(this Vector2 vector) => new Vector3(vector.x, vector.y, 0);
    public static Vector2 GetVector2Horizontal(this Vector3 vector) => new Vector2(vector.x, vector.z);
    public static Vector2 GetVector2Vertical(this Vector3 vector) => new Vector2(vector.x, vector.y);

    public static float HeightToForce(this int height, float gravityScale = 1)
    {
        float force;

        switch (height)
        {
            case 0:
                force = 0.0f;
                break;
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

    public static void MouseVisibility(bool state)
    {
        Cursor.lockState = state == true ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }

    public static bool FieldsAreEqual(object obj1, object obj2)
    {
        Type type1 = obj1.GetType();
        Type type2 = obj2.GetType();

        if (type1 != type2)
        {
            return false;
        }

        FieldInfo[] fields = type1.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            object value1 = field.GetValue(obj1);
            object value2 = field.GetValue(obj2);

            if (!value1.Equals(value2))
            {
                return false;
            }
        }

        return true;
    }
}
