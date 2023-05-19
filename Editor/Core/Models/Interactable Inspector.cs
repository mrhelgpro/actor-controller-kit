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

            string info = thisTarget.Target.IsExists ? thisTarget.Target.GetName : "None";
            BoxStyle style = thisTarget.Target.IsExists ? BoxStyle.Active : BoxStyle.Default;

            Inspector.DrawInfoBox(info, style);
        }
    }
}
