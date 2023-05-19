using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(MovementPhysicPresenter))]
    [CanEditMultipleObjects]
    public class MovementPhysicPresenterInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(MovementNavigationPresenter))]
    [CanEditMultipleObjects]
    public class MovementNavigationPresenterInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }

    [ExecuteInEditMode]
    [CustomEditor(typeof(Movement2DPresenter))]
    [CanEditMultipleObjects]
    public class Movement2DPresenterInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}