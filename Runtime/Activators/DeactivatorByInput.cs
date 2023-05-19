using UnityEngine;

namespace Actormachine
{
    /// <summary> To deactivate the Presenters. </summary>
    public class DeactivatorByInput : Deactivator
    {
        public enum Mode { None, RepeatedPressing, ActiveWhileHolding }

        [Tooltip("The event when the controller will be deactivated")]
        public Mode DeactivateMode = Mode.None;
        public InputableCompare InputableCompare;

        private Inputable _inputable;

        public override void Initiate()
        {
            _inputable = AddComponentInRoot<Inputable>();
        }

        private bool _isRepeatedPressing = false;

        public override void UpdateLoop()
        {
            if (DeactivateMode == Mode.RepeatedPressing)
            {
                if (InputableCompare.IsEquals(_inputable) == false)
                {
                    _isRepeatedPressing = true;
                }

                if (_isRepeatedPressing == true)
                {
                    if (InputableCompare.IsEquals(_inputable) == true)
                    {
                        Deactivate();

                        _isRepeatedPressing = false;
                    }
                }
            }
            else if (DeactivateMode == Mode.ActiveWhileHolding)
            {
                if (InputableCompare.IsEquals(_inputable) == false)
                {
                    Deactivate();
                }
            }
        }
    }
}