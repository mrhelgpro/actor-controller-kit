using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{   
    [InitializeOnLoad]
    public sealed class BootstrapEditor : UnityEditor.Editor
    {
        static BootstrapEditor()
        {
            EditorApplication.update += InitInEditMode;
        }

        private static void InitInEditMode()
        {
            if (Application.isPlaying == false)
            {
                Bootstrap[] bootstraps = FindObjectsOfType<Bootstrap>();

                foreach (Bootstrap bootstrap in bootstraps) bootstrap.Initiation();

                ActorBehaviour[] actorBehaviours = FindObjectsOfType<ActorBehaviour>();

                foreach (ActorBehaviour actorBehaviour in actorBehaviours) actorBehaviour.Initiation();
            }
        }

        /*
        /// <summary> Finds all IInitInEditMode and calls InitInEditMode(). </summary>
        private static void InitInEditMode()
        {
            if (Application.isPlaying == false)
            {
                IInitInEditMode[] initiations = FindObjectsOfType<MonoBehaviour>()
                    .Where(obj => obj is IInitInEditMode)
                    .Select(obj => obj as IInitInEditMode)
                    .ToArray();

                foreach (IInitInEditMode initiation in initiations) initiation.InitInEditMode();
            }
        }
        */
    }
}