using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(GrabberPresenter))]
    [CanEditMultipleObjects]
    public class GrabberPresenterInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawInfoBox("GRAB THE TARGET");

            base.OnInspectorGUI();
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(ThrowerPresenter))]
    [CanEditMultipleObjects]
    public class ThrowerPresenterInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawInfoBox("THROWING A TARGET");

            base.OnInspectorGUI();
        }
    }
}

