using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Inputable))]
    public class InputableInspector : ActorBehaviourInspector
    {
        public override void OnInspectorGUI()
        {
            Inputable thisTarget = (Inputable)target;
            Transform root = thisTarget.FindRootTransform;
            InputController inputController = root.gameObject.GetComponentInChildren<InputController>();

            if (inputController == null)
            {
                DrawModelBox("<InputController> - is not found", BoxStyle.Error);

                return;
            }

            if (Application.isPlaying)
            {
                DrawModelBox("Menu", thisTarget.MenuState == true ? BoxStyle.Active : BoxStyle.Default);

                // public Vector2 PointerScreenPosition;
                // public Vector2 MoveVector;
                // public Vector2 LookDelta;

                DrawModelBox("Option", thisTarget.OptionState == true ? BoxStyle.Active : BoxStyle.Default);
                DrawModelBox("Cancel", thisTarget.CancelState == true ? BoxStyle.Active : BoxStyle.Default);
                DrawModelBox("Motion", thisTarget.MotionState == true ? BoxStyle.Active : BoxStyle.Default);
                DrawModelBox("Interact", thisTarget.InteractState == true ? BoxStyle.Active : BoxStyle.Default);

                DrawModelBox("Action Left", thisTarget.ActionLeftState == true ? BoxStyle.Active : BoxStyle.Default);
                DrawModelBox("Action Middle", thisTarget.ActionMiddleState == true ? BoxStyle.Active : BoxStyle.Default);
                DrawModelBox("Action Right", thisTarget.ActionRightState == true ? BoxStyle.Active : BoxStyle.Default);

                DrawModelBox("Control", thisTarget.ControlState == true ? BoxStyle.Active : BoxStyle.Default);
                DrawModelBox("Shift", thisTarget.ShiftState == true ? BoxStyle.Active : BoxStyle.Default);
            }
            else
            {
                DrawModelBox("Receive input data");
            }
        }
    }
}