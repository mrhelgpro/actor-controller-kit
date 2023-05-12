using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(ActorBootstrap))]
    public class ActorBootstrapInspector : ActorBehaviourInspector
    {
        ActorBootstrap thisTarget;

        public override void OnInspectorGUI()
        {
            // Draw a Warning	
            if (ActorExtantion.IsSingleInstanceOnScene<ActorBootstrap>() == false)
            {
                DrawModelBox("<ActorBootstrap> should be a single", BoxStyle.Warning);

                return;
            }

            // Draw a Inspector
            DrawHeader("ActorBootstrap");

            thisTarget = (ActorBootstrap)target;

            if (thisTarget.GetActors.Count > 0)
            {
                // Draw Players
                drawActorList("Player", "Players", ButtonStyle.Main);

                // Draw Enemies
                drawActorList("Enemy", "Enemies", ButtonStyle.Active);

                // Draw Interaction
                drawActorList("Interaction", "Interactions", ButtonStyle.Active);
            }

            EditorUtility.SetDirty(target);
        }

        private void drawActorList(string single, string many, ButtonStyle mainStyle = ButtonStyle.Active)
        {
            List<Actor> actors = thisTarget.GetActors.FindAll(actor => actor.gameObject.CompareTag(single));

            if (actors.Count > 0)
            {
                string info = actors.Count == 1 ? single : many;
                string count = actors.Count == 1 ? "" : " " + actors.Count;

                EditorGUILayout.Space(12);
                DrawHeader(info + count, 10);

                foreach (Actor actor in actors)
                {
                    ButtonStyle buttonStyle = actor.IsFree ? ButtonStyle.Default : mainStyle;
                    DrawLinkButton(actor.Name, actor.gameObject, buttonStyle);
                }
            }
        }
    }
}
