using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Inputable))]
    [CanEditMultipleObjects]
    public class InputableInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Inputable thisTarget = (Inputable)target;
            Transform root = thisTarget.FindRootTransform;
            InputController inputController = root.gameObject.GetComponentInChildren<InputController>();

            if (inputController == null)
            {
                Inspector.DrawInfoBox("<INPUTCONTROLLER> - IS NOT FOUND", BoxStyle.Error);

                return;
            }

            if (Application.isPlaying)
            {
                Inspector.DrawInfoBox("Menu", thisTarget.MenuState == true ? BoxStyle.Active : BoxStyle.Default);

                // public Vector2 PointerScreenPosition;
                // public Vector2 MoveVector;
                // public Vector2 LookDelta;

                Inspector.DrawInfoBox("Option", thisTarget.OptionState == true ? BoxStyle.Active : BoxStyle.Default);
                Inspector.DrawInfoBox("Cancel", thisTarget.CancelState == true ? BoxStyle.Active : BoxStyle.Default);
                Inspector.DrawInfoBox("Motion", thisTarget.MotionState == true ? BoxStyle.Active : BoxStyle.Default);
                Inspector.DrawInfoBox("Interact", thisTarget.InteractState == true ? BoxStyle.Active : BoxStyle.Default);

                Inspector.DrawInfoBox("Action Left", thisTarget.ActionLeftState == true ? BoxStyle.Active : BoxStyle.Default);
                Inspector.DrawInfoBox("Action Middle", thisTarget.ActionMiddleState == true ? BoxStyle.Active : BoxStyle.Default);
                Inspector.DrawInfoBox("Action Right", thisTarget.ActionRightState == true ? BoxStyle.Active : BoxStyle.Default);

                Inspector.DrawInfoBox("Control", thisTarget.ControlState == true ? BoxStyle.Active : BoxStyle.Default);
                Inspector.DrawInfoBox("Shift", thisTarget.ShiftState == true ? BoxStyle.Active : BoxStyle.Default);
            }
            else
            {
                Inspector.DrawInfoBox("RECEIVE INPUT DATA");
            }
        }
    }
}