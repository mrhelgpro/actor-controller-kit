using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    public static class Engine
    {
        public static void SetLayer(this GameObject gameObject, string name)
        {
            if (IsLayerExist(name) == false)
            {
                // Check free layer
                int freeLayer = GetFreeLayer();

                if (freeLayer == -1)
                {
                    Debug.Log("It is not necessary to add layer <" + name + "> No free layer found. All layers are used.");

                    return;
                }

                // Change the name of the layer
                SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
                SerializedProperty layers = tagManager.FindProperty("layers");
                if (layers.GetArrayElementAtIndex(freeLayer).stringValue.Length == 0)
                {
                    layers.GetArrayElementAtIndex(freeLayer).stringValue = name;
                    tagManager.ApplyModifiedProperties();
                }
            }

            gameObject.layer = LayerMask.NameToLayer(name);
        }

        public static bool IsLayerExist(string name) => LayerMask.NameToLayer(name) == -1 ? false : true;

        public static int GetFreeLayer()
        {
            int maxLayers = 32;
            int freeLayer = -1;

            for (int i = 0; i < maxLayers; i++)
            {
                string layerName = LayerMask.LayerToName(i);

                if (string.IsNullOrEmpty(layerName))
                {
                    freeLayer = i;
                    break;
                }
            }

            if (freeLayer != -1)
            {
                return freeLayer;
            }

            return -1;
        }
    }
}