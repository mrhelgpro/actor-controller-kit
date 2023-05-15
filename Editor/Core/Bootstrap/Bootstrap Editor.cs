using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Actormachine.Editor
{   
    [InitializeOnLoad]
    public sealed class BootstrapEditor : UnityEditor.Editor
    {
        static BootstrapEditor()
        {
            EditorApplication.update += InitInEditMode;
        }

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
    }
}