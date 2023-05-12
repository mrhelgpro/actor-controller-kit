using UnityEngine;

namespace Actormachine
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