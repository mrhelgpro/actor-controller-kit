using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Inventoriable))]
    [CanEditMultipleObjects]
    public class InventoriableInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawInfoBox("INVENTORY");
        }
    }
}