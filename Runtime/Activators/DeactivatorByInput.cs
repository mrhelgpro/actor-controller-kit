using UnityEngine;

namespace Actormachine
{
    /// <summary> To deactivate the Propertys. </summary>
    public class DeactivatorByInput : Deactivator
    {
        public enum Mode { Hold, Down }

        [Tooltip("The event when the controller will be deactivated")]
        public Mode DeactivateMode = Mode.Hold;
        public InputableCompare InputableCompare;

        private bool _previousInput = false;
        private Inputable _inputable;

        public override void OnEnableState()
        {
            // Add or Get comppnent in the Root
            _inputable = AddComponentInRoot<Inputable>();
        }

        public override void OnActiveState()
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

        public override void OnExitState()
        {
            _previousInput = false;
        }
    }
}