using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    public static class ContextMenuExtention
    {
        /// <summary> Check if gameObject is Prefab. </summary>
        public static bool IsPrefab(this GameObject gameObject)
        {
            PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(gameObject);
            return assetType == PrefabAssetType.Regular || assetType == PrefabAssetType.Variant;
        }

        /// <summary> Hide Child Objects in Hierarchy. </summary>
        public static void HideChildObjects(this GameObject gameObject, bool state)
        {
            foreach (Transform child in gameObject.transform) child.gameObject.HideObject(state);
        }

        /// <summary> Hide This Object in Hierarchy. </summary>
        public static void HideObject(this GameObject gameObject, bool state)
        {
            if (state == true)
            {
                gameObject.AddRequiredComponent<HideInHierarchy>();
                gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

                return;
            }

            gameObject.RemoveComponent<HideInHierarchy>();
            gameObject.hideFlags = HideFlags.None;
        }

        public static bool IsHiddenChildObject(this GameObject gameObject)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                HideInHierarchy hideInHierarchy = gameObject.transform.GetChild(i).GetComponent<HideInHierarchy>();

                if (hideInHierarchy != null)
                {
                    return true;
                }
            }

            return false;
        }

        public static void CreatePrefab(string type, string name, bool hideInHierarchy = false, bool notEditable = false)
        {
            Transform parent = null;
            Vector3 position = parent == null ? Vector3.zero : parent.position;

            GameObject instantiate = GameObject.Instantiate(Resources.Load<GameObject>(type + "/" + name));
            instantiate.name = name;
            instantiate.transform.parent = parent;
            instantiate.transform.position = position;
            instantiate.transform.rotation = Quaternion.identity;
            instantiate.HideChildObjects(hideInHierarchy);
            instantiate.hideFlags = notEditable ? HideFlags.NotEditable : HideFlags.None;
        }

        public static void CreateBootstrap<T>() where T : Component
        {
            T currentBootstrap = GameObject.FindAnyObjectByType<T>();

            if (currentBootstrap == null)
            {
                Bootstrap anyBootstrap = GameObject.FindAnyObjectByType<Bootstrap>();
                Transform parentBootstrap;

                if (anyBootstrap == null)
                {
                    parentBootstrap = new GameObject().transform;
                }
                else
                {
                    parentBootstrap = anyBootstrap.transform;
                }

                currentBootstrap = parentBootstrap.gameObject.AddComponent<T>();
            }

            currentBootstrap.transform.SetAsFirstSibling();
            EditorGUIUtility.PingObject(currentBootstrap.gameObject);
        }
    }
}