using UnityEngine;

namespace Actormachine
{
    public enum HoldType { WhileStateActive, ToCallAgain }

    public class GrabberPresenter : Presenter
    {
        public HoldType HoldType = HoldType.WhileStateActive;
    }
}