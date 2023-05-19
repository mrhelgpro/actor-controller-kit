using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(AnimatorPresenter))]
    [CanEditMultipleObjects]
    public class AnimatorPresenterInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            AnimatorPresenter thisTarget = (AnimatorPresenter)target;

            string info = thisTarget.Controller == null ? "PLAYS ANIMATION" : "SWITCHES TO - " + thisTarget.Controller.name;
            Inspector.DrawInfoBox(info);

            base.OnInspectorGUI();
        }
    }
}
