using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(BootstrapActor))]
    public class ActorBootstrapInspector : UnityEditor.Editor
    {
        private bool foldoutPlayers = true;
        //private bool foldout—ompanions = true;   
        private bool foldoutEnemies = false;            
        private bool foldoutInteractions = false;
        //private bool foldoutRandoms = false;

        public override void OnInspectorGUI()
        {
            // Draw a Warning	
            if (BootstrapExtantion.IsSingleInstanceOnScene<BootstrapActor>() == false)
            {
                Inspector.DrawInfoBox("<ACTORBOOTSTRAP> SHOULD BE A SINGLE", BoxStyle.Warning);

                return;
            }

            // Draw a Inspector
            Inspector.DrawHeader("Actor Bootstrap");

            if (BootstrapActor.GetActors.Count > 0)
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
            List<Actor> actors = BootstrapActor.GetActors.FindAll(actor => actor.gameObject.CompareTag(single));

            if (actors.Count > 0)
            {

                string info = actors.Count == 1 ? single : many;
                string count = actors.Count == 1 ? "" : " " + actors.Count;

                if (mainStyle == ButtonStyle.Main)
                {
                    Inspector.DrawHeader(info + count, 10);

                    foreach (Actor actor in actors)
                    {
                        Inspector.DrawLinkButton(actor.Name, actor.gameObject, mainStyle);
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
                        Inspector.DrawLinkButton(actor.Name, actor.gameObject, ButtonStyle.Default);
                    }
                }
                GUILayout.EndVertical();
            }
        }
    }
}
