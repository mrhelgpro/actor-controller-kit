using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(InteractionPresenter))]
    [CanEditMultipleObjects]
    public class InteractionPresenterInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawInfoBox("GRAB THE TARGET");

            base.OnInspectorGUI();
        }
    }
}

