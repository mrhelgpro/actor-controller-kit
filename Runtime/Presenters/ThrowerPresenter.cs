using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionTime { Enter, Exit }

namespace Actormachine
{
    public class ThrowerPresenter : Presenter
    {
        public ActionTime ActionTime = ActionTime.Enter;

        // Model Components
        private Interactable _interactable;

        // Presenter Methods
        public override void Enter()
        {
            // Using "AddComponentInRoot" to add or get comppnent on the Root    
            _interactable = AddComponentInRoot<Interactable>();
        }

        public override void Exit()
        {

        }
    }
}