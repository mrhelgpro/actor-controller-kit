using UnityEngine;

namespace AssemblyActorCore
{
    public class CameraController : Presenter
    {
        // Model Parametres
        public CameraParametres CameraParametres = new CameraParametres();

        // Model Components
        protected Inputable inputable;
        protected Followable followable;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying == false)
            {
                GetComponentInParent<Followable>()?.SetPreview(CameraParametres);
            }
        }
#endif

        private new void Awake()
        {
            base.Awake();

            followable = RequireComponent<Followable>();
            inputable = RequireComponent<Inputable>();
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
                isRotable = inputable.ActionLeftState;
            }
            else if (CameraParametres.InputOrbitMode == InputOrbitMode.MiddleHold)
            {
                isRotable = inputable.ActionMiddleState;
            }
            else if (CameraParametres.InputOrbitMode == InputOrbitMode.RightHold)
            {
                isRotable = inputable.ActionRightState;
            }

            if (isRotable)
            {
                followable.Parametres.Orbit.Horizontal += inputable.LookDelta.x * CameraParametres.Orbit.SensitivityX;
                followable.Parametres.Orbit.Vertical += inputable.LookDelta.y * CameraParametres.Orbit.SensitivityY;
                followable.Parametres.Orbit.Vertical = Mathf.Clamp(followable.Parametres.Orbit.Vertical, -30, 80);
            }

            followable.Parametres.Offset = CameraParametres.Offset;
            followable.Parametres.DampTime = CameraParametres.DampTime;
            followable.Parametres.FieldOfView = CameraParametres.FieldOfView;
        }
    }
}