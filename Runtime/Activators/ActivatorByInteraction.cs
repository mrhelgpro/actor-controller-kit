using UnityEngine;

namespace Actormachine
{
    public sealed class ActivatorByInteraction : Activator
    {
        public string TargetTag = "Any";

        // Model Components
        private Interactable _interactable;

#if UNITY_EDITOR
        private void OnValidate()
        {
            TargetTag = TargetTag == "" ? "Any" : TargetTag;
        }
#endif

        public override void Initiate()
        {
            // Using "AddComponentInRoot" to add or get comppnent on the Root
            _interactable = AddComponentInRoot<Interactable>();
        }

        public override void UpdateLoop()
        {
            if (_interactable.Target.IsExists)
            {
                if (TargetTag == "Any" ? true : _interactable.Target.IsTag(TargetTag))
                {
                    SetAvailable(true);

                    return;
                }
            }

            SetAvailable(false);
        }
    }
}