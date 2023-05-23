using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Interactable))]
    [CanEditMultipleObjects]
    public class InteractableInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Interactable thisTarget = (Interactable)target;

            if (Application.isPlaying == false)
            {
                Inspector.DrawInfoBox("STORE INTERACTIVE TARGETS");

                return;
            }

            string info = thisTarget.Target == null ? "None" : thisTarget.Target.name;
            BoxStyle style = thisTarget.Target == null ? BoxStyle.Default : BoxStyle.Active;

            Inspector.DrawInfoBox(info, style);
        }
    }
}
