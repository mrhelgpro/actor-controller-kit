using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public class HideInHierarchy : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        }
#endif
    }
}