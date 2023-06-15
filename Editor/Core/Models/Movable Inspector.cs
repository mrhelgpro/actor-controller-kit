using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(Movable))]
    [CanEditMultipleObjects]
    public class MovableInspector : ActormachineComponentBaseInspector
    {
        public override void OnInspectorGUI()
        {
            Inspector.DrawSubtitle("CONTROLS SPEED");

            // Draw a Inspector
            Movable thisTarget = (Movable)target;

            thisTarget.WalkSpeed = EditorGUILayout.Slider("Walk Speed", thisTarget.WalkSpeed, 1f, 5f);
            thisTarget.RunSpeed = EditorGUILayout.Slider("Run Speed", thisTarget.RunSpeed, thisTarget.WalkSpeed, 10);
            thisTarget.Gravity = EditorGUILayout.Slider("Gravity", thisTarget.Gravity, 0f, 2f);
            thisTarget.JumpHeight = EditorGUILayout.IntSlider("Jump Height", thisTarget.JumpHeight, 0, 5);

            if (thisTarget.JumpHeight > 0)
            {
                thisTarget.ExtraJumps = EditorGUILayout.IntSlider("Extra Jumps", thisTarget.ExtraJumps, 0, 3);
                thisTarget.Levitation = EditorGUILayout.Slider("Levitation", thisTarget.Levitation, 0f, 1f);
            }

            //DrawBaseInspector();
        }
    }
}
