using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Storagable))]
    [CanEditMultipleObjects]
    public class StoragableInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawSubtitle("STORAGE ITEMS");

            DrawBaseInspector();
        }
    }
}