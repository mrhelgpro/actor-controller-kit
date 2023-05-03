using UnityEngine;

namespace AssemblyActorCore
{
    public class CameraPresenter : Presenter
    {
        // Model Parametres
        public CameraParametres CameraParametres = new CameraParametres();

        // Model Components
        private Inputable _inputable;
        private Followable _followable;

#if UNITY_EDITOR
        private void OnValidate()
        {
            try
            {
                if (Application.isPlaying == false)
                {
                    Actor actor = GetComponentInParent<Actor>();

                    actor.gameObject.GetComponentInChildren<Followable>()?.SetPreview(CameraParametres);
                }
            }
            catch
            {
                // NullReferenceException: Called during Script Reload
            }
        }
#endif

        protected override void Initiation()
        {
            // Get components using "GetComponentInActor" to create them on <Actor>
            _followable = GetComponentInActor<Followable>();
            _inputable = GetComponentInActor<Inputable>();
        }

        public override void UpdateLoop()
        {
            bool isRotable = false;

            if (CameraParametres.InputOrbitMode == InputOrbitMode.Free)
            {
                isRotable = true;
            }
            else if (CameraParametres.InputOrbitMode == InputOrbitMode.LeftHold)
            {
                isRotable = _inputable.ActionLeftState;
            }
            else if (CameraParametres.InputOrbitMode == InputOrbitMode.MiddleHold)
            {
                isRotable = _inputable.ActionMiddleState;
            }
            else if (CameraParametres.InputOrbitMode == InputOrbitMode.RightHold)
            {
                isRotable = _inputable.ActionRightState;
            }

            if (isRotable)
            {
                _followable.Parametres.Orbit.Horizontal += _inputable.LookDelta.x * CameraParametres.Orbit.SensitivityX;
                _followable.Parametres.Orbit.Vertical += _inputable.LookDelta.y * CameraParametres.Orbit.SensitivityY;
                _followable.Parametres.Orbit.Vertical = Mathf.Clamp(_followable.Parametres.Orbit.Vertical, -30, 80);
            }

            _followable.Parametres.Offset = CameraParametres.Offset;
            _followable.Parametres.DampTime = CameraParametres.DampTime;
            _followable.Parametres.FieldOfView = CameraParametres.FieldOfView;
        }
    }
}