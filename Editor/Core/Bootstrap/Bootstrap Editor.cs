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

                foreach (Bootstrap bootstrap in bootstraps) bootstrap.Initiate();
            }
        }
    }
}