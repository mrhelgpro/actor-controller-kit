using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public static class ActorExtention
{
    /// <summary> Check if gameObject is Prefab. </summary>
    public static bool IsPrefab(this GameObject gameObject)
    {
        PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(gameObject);
        return assetType == PrefabAssetType.Regular || assetType == PrefabAssetType.Variant;
    }

    /// <summary> Hide Child Objects in Hierarchy. </summary>
    public static void HideChildObjects(this Transform transform, bool state)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.hideFlags = state == true ? HideFlags.HideInHierarchy | HideFlags.HideInInspector : HideFlags.None;
        }
    }

    /// <summary> For calculating the exact height of the jump, based on gravity. </summary>
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
                Debug.LogWarning("Force not calculated for height " + height);
                break;
        }

        float gravity = 0.425f * gravityScale + 0.575f;

        return force * gravity;
    }
}
