using UnityEngine;

namespace Actormachine
{
    public sealed class ActivatorByInteraction : Activator
    {
        public string TargetTag = "Player";

        // Model Components
        private Interactable _interactable;

#if UNITY_EDITOR
        private void OnValidate()
        {
            TargetTag = TargetTag == "" ? "Player" : TargetTag;
        }
#endif

        public override void Enable()
        {
            // Using "AddComponentInRoot" to add or get comppnent on the Root
            _interactable = AddComponentInRoot<Interactable>();
        }

        public override void UpdateLoop()
        {

        }
    }
}