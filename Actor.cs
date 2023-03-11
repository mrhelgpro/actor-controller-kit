using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class Actor : MonoBehaviour
    {
        public string Name = "Actor";
        public Inputable Input = new Inputable();
        public Actionable Actionable = new Actionable();

        private void Update()
        {
            Actionable.WaitList();
            Actionable.UpdateLoop();
        }

        private void FixedUpdate()
        {
            Actionable.FixedLoop();
        }
    }
}

namespace AssemblyActorCore
{

}