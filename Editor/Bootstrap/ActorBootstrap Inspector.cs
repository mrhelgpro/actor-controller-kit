using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(ActorBootstrap))]
    public class ActorBootstrapInspector : ActorBehaviourInspector
    {
        private bool foldoutPlayers = true;
        //private bool foldout—ompanions = true;   
        private bool foldoutEnemies = false;            
        private bool foldoutInteractions = false;
        //private bool foldoutRandoms = false;

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

            if (ActorBootstrap.GetActors.Count > 0)
            {
                // Draw Players
                drawActorList("Player", "Players", ref foldoutPlayers, ButtonStyle.Main);

                // Draw —ompanion
                //drawActorList("—ompanion", "—ompanions", ref foldout—ompanions, ButtonStyle.Main);

                // Draw Enemies
                drawActorList("Enemy", "Enemies", ref foldoutEnemies, ButtonStyle.Active);

                // Draw Interaction
                drawActorList("Interaction", "Interactions", ref foldoutInteractions, ButtonStyle.Active);

                // Draw Random
                //drawActorList("Random", "Randoms", ref foldoutRandoms, ButtonStyle.Active);
            }

            EditorUtility.SetDirty(target);
        }

        private void drawActorList(string single, string many, ref bool foldoutState, ButtonStyle mainStyle = ButtonStyle.Active)
        {
            List<Actor> actors = ActorBootstrap.GetActors.FindAll(actor => actor.gameObject.CompareTag(single));

            if (actors.Count > 0)
            {

                string info = actors.Count == 1 ? single : many;
                string count = actors.Count == 1 ? "" : " " + actors.Count;

                if (mainStyle == ButtonStyle.Main)
                {
                    DrawHeader(info + count, 10);

                    foreach (Actor actor in actors)
                    {
                        ButtonStyle buttonStyle = actor.IsFree ? ButtonStyle.Default : mainStyle;
                        DrawLinkButton(actor.Name, actor.gameObject, buttonStyle);
                    }

                    EditorGUILayout.Space(16);

                    return;
                }

                GUILayout.BeginVertical();

                foldoutState = EditorGUILayout.Foldout(foldoutState, info + count);

                if (foldoutState)
                {
                    foreach (Actor actor in actors)
                    {
                        ButtonStyle buttonStyle = actor.IsFree ? ButtonStyle.Default : mainStyle;
                        DrawLinkButton(actor.Name, actor.gameObject, buttonStyle);
                    }
                }
                GUILayout.EndVertical();
            }
        }
    }
}
