using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Deactivator))]
    [CanEditMultipleObjects]
    public class DeactivatorInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawInfoBox("DEACTIVATES THE PRESENTER");

            base.OnInspectorGUI();
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(DeactivatorByInput))]
    [CanEditMultipleObjects]
    public class DeactivatorByInputInspector : DeactivatorInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}