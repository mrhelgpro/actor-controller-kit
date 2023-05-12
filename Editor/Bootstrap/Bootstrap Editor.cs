using UnityEngine;
using UnityEditor;

namespace Actormachine.Editor
{
    [InitializeOnLoad]
    public class BootstrapEditor : UnityEditor.Editor
    {
        static BootstrapEditor()
        {
            EditorApplication.update += UpdateInEditMode;
        }

        private static void UpdateInEditMode()
        {
            if (Application.isPlaying == false)
            {
                Bootstrap[] bootstraps = GameObject.FindObjectsOfType<Bootstrap>();

                // Check Bootstrap
                if (bootstraps.Length > 0)
                {
                    bootstraps[0].transform.name = "Bootstrap";
                    bootstraps[0].transform.parent = null;
                    bootstraps[0].transform.localScale = Vector3.one;
                    bootstraps[0].transform.position = Vector3.zero;
                    bootstraps[0].transform.rotation = Quaternion.identity;
                }

                // Update Bootstrap
                foreach (Bootstrap bootstrap in bootstraps) bootstrap.Initiation();
            }
        }
    }
}