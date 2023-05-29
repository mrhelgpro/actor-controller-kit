using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Storagable))]
    [CanEditMultipleObjects]
    public class StoragableInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            //Inspector.DrawInfoBox("STORAGE");

            base.OnInspectorGUI();
        }
    }
}