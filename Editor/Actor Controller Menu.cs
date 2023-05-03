using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public static class ActorControllerMenu
    {
        [MenuItem("GameObject/Actor/Player Default", false, 0)]
        public static void CreateActorDefault()
        {
            //GameObject.Instantiate(Resources.Load<GameObject>("Characters/Player Default")).name = "Player Default";

            GameObject instantiate = GameObject.Instantiate(Resources.Load<GameObject>("Characters/Player Default"));
            instantiate.name = "Player Default";
            instantiate.transform.position = Vector3.zero;
            instantiate.transform.rotation = Quaternion.identity;
        }
    }
}