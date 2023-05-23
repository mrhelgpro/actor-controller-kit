using UnityEngine;

namespace Actormachine
{
    /// <summary> To deactivate the Presenters. </summary>
    public class DeactivatorByInput : Deactivator
    {
        public enum Mode { Hold, Press }

        [Tooltip("The event when the controller will be deactivated")]
        public Mode DeactivateMode = Mode.Hold;
        public InputableCompare InputableCompare;

        private Inputable _inputable;

        public override void Enable()
        {
            // Using "AddComponentInRoot" to add or get comppnent on the Root
            _inputable = AddComponentInRoot<Inputable>();
        }

        private bool _isRepeatedPressing = false;

        public override void UpdateLoop()
        {
            if (DeactivateMode == Mode.Press)
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
            else if (DeactivateMode == Mode.Hold)
            {
                if (InputableCompare.IsEquals(_inputable) == false)
                {
                    Deactivate();
                }
            }
        }
    }
}