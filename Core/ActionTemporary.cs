using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public class ActionTemporary : ActionInteraction
    {
        public override void Exit()
        {
            movable.FreezAll();
            gameObject.SetActive(false);
        }
    }
}