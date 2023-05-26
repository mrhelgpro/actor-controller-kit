using UnityEngine;

namespace Actormachine
{
    /// <summary> To deactivate the Presenters. </summary>
    public class DeactivatorByInput : Deactivator, IEnableState, IActiveState, IExitState
    {
        public enum Mode { Hold, Down }

        [Tooltip("The event when the controller will be deactivated")]
        public Mode DeactivateMode = Mode.Hold;
        public InputableCompare InputableCompare;

        private bool _previousInput = false;
        private Inputable _inputable;

        public void OnEnableState()
        {
            // Add or Get comppnent in the Root
            _inputable = AddComponentInRoot<Inputable>();
        }

        public void OnActiveState()
        {
            if (DeactivateMode == Mode.Down)
            {
                if (InputableCompare.IsEquals(_inputable) == false)
                {
                    _previousInput = true;
                }

                if (_previousInput == true)
                {
                    if (InputableCompare.IsEquals(_inputable) == true)
                    {
                        Deactivate();

                        _previousInput = false;
                    }
                }
            }
            else if (DeactivateMode == Mode.Hold)
            {
                if (InputableCompare.IsEquals(_inputable) == false)
                {
                    Deactivate();
                }
            }
        }

        public void OnExitState()
        {
            _previousInput = false;
        }
    }
}