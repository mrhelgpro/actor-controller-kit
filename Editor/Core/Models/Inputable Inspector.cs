using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Inputable))]
    [CanEditMultipleObjects]
    public class InputableInspector : ActormachineComponentBaseInspector
    {
        Inputable thisTarget;

        public override void OnInspectorGUI()
        {
            thisTarget = (Inputable)target;
            Transform root = thisTarget.FindRootTransform;
            InputController inputController = root.gameObject.GetComponentInChildren<InputController>();

            if (inputController == null)
            {
                Inspector.DrawSubtitle("<INPUTCONTROLLER> - IS NOT FOUND", BoxStyle.Error);

                return;
            }

            Inspector.DrawSubtitle("RECEIVE INPUT DATA");
        }
    }
}