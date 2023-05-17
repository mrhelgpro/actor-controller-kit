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

            if (thisTarget.Controller == null)
            {
                Inspector.DrawModelBox("You need to add an AnimatorController", BoxStyle.Error);
            }

            base.OnInspectorGUI();
        }
    }
}
