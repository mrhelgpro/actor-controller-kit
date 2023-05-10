using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    [InitializeOnLoad]
    public class ActorBootstrapEditor : Editor
    {
        static ActorBootstrapEditor()
        {
            EditorApplication.update += CheckRequiredComponents;
        }

        private static void CheckRequiredComponents()
        {
            if (Application.isPlaying == false)
            {
                // Add Required Components
            }
        }
    }
}