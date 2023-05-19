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
        public override void Initiate()
        {
            // Get components using "GetComponentInRoot" to create them on <Actor>
            _interactable = AddComponentInRoot<Interactable>();
        }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }
    }
}