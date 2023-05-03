using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public class CameraPresenter : Presenter
    {
        // Model Parametres
        public CameraParametres CameraParametres = new CameraParametres();

        // Model Components
        private Inputable _inputable;
        private Followable _followable;

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

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(CameraPresenter))]
    public class CameraPresenterEditor : ModelEditor
    {
        public override void OnInspectorGUI()
        {
            CameraPresenter thisTarget = (CameraPresenter)target;

            Actor actor = thisTarget.GetComponentInParent<Actor>();

            if (actor)
            {
                Followable followable = actor.gameObject.GetComponentInChildren<Followable>();

                if (followable)
                {
                    // Show script Link
                    ShowLink(thisTarget);

                    // Show Camera Parametres
                    SerializedProperty parametresProperty = serializedObject.FindProperty("CameraParametres");
                    EditorGUILayout.PropertyField(parametresProperty, true);
                    serializedObject.ApplyModifiedProperties();

                    if (GUI.changed)
                    {
                        if (Application.isPlaying == false)
                        {
                            followable.SetPreview(thisTarget.CameraParametres);
                            EditorUtility.SetDirty(thisTarget);
                        }
                    }

                    return;
                }

                ErrorMessage("<Followable> is not found", thisTarget.gameObject.name + " - CameraPresenter: ");
            }
            else
            {
                ErrorMessage("<Actor> is not found", thisTarget.gameObject.name + " - CameraPresenter: ");
            }
        }
    }
#endif
}