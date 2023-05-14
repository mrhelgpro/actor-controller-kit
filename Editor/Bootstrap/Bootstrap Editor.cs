using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Actormachine.Editor
{
    [InitializeOnLoad]
    public class BootstrapEditor : UnityEditor.Editor
    {
        static BootstrapEditor()
        {
            EditorApplication.update += updateInEditMode;
        }

        private void Awake()
        {
            Debug.Log("BootstrapEditor");
        }

        private static void updateInEditMode()
        {
            updateBootstraps();
        }

        /*
        private static void checkRequireComponentInRoot<T>() where T : ActorBehaviour
        {
            IRequireComponentInRoot<T>[] requireComponents = (IRequireComponentInRoot<T>[])GameObject.FindObjectsOfType<ActorBehaviour>().Where(c => c is IRequireComponentInRoot<T>).ToArray();

            //foreach()

            GameObject gameObject = (requireComponents as ActorBehaviour).gameObject;
        }
        */

        private static void updateBootstraps()
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