using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(DirectionPresenter))]
    [CanEditMultipleObjects]
    public class DirectionPresenterInspectorInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
