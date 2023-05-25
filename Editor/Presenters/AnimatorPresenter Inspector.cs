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

            string info = thisTarget.Controller == null ? "PLAY ANIMATION" : "SWITCHES TO - " + thisTarget.Controller.name;
            Inspector.DrawInfoBox(info);

            thisTarget.PlayName = EditorGUILayout.TextField("Time", thisTarget.PlayName);
            thisTarget.Controller = (RuntimeAnimatorController)EditorGUILayout.ObjectField("Controller", thisTarget.Controller, typeof(RuntimeAnimatorController), true);
            thisTarget.PlayMode = (PlayMode)EditorGUILayout.EnumPopup("Play Mode", thisTarget.PlayMode);

            if (thisTarget.PlayMode == PlayMode.ByTime)
            {
                thisTarget.Duration = EditorGUILayout.FloatField("Duration", thisTarget.Duration);
            }
        }
    }
}
