using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActorExtention
{
    // Finds the required Component on <Actor> gets or instantiates
    public static T AddThisComponent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() == null ? gameObject.AddComponent<T>() : gameObject.GetComponent<T>();
    }

    public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.GetComponent<T>() != null) UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<T>());
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
}

public class BoolReferance
{
    public bool Value;
    public bool IsChanged;

    private bool _previousValue;

    public void UpdateIsChanged()
    {
        if (IsChanged)
        {
            IsChanged = false;
        }
        else if (_previousValue != Value)
        {
            IsChanged = true;
        }

        _previousValue = Value;
    }
}

public struct FlagBool
{
    private bool _previousValue;

    public bool IsChange(bool value)
    {
        bool result = _previousValue != value;
        _previousValue = value;
        return result;
    }
}

public struct FlagInt
{
    private int _previousValue;

    public bool IsChange(int value)
    {
        bool result = _previousValue != value;
        _previousValue = value;
        return result;
    }
}

public struct FlagFloat
{
    private float _previousValue;

    public bool IsChange(float value)
    {
        bool result = _previousValue != value;
        _previousValue = value;
        return result;
    }
}

public class SmoothVector
{
    private Vector3 _previousValue;

    public Vector3 GetValue(Vector3 value, float acceleration)
    {
        Vector3 currentDirection = Vector3.Lerp(_previousValue, value, Time.deltaTime * acceleration);

        _previousValue = currentDirection;

        return currentDirection;
    }
}
