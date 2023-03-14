using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public enum Preset { None, Free, ThirdPerson, Platformer }

    public class Actor : MonoBehaviour
    {
        public Preset Preset = Preset.Free;
        public string Name = "Actor";
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    [CustomEditor(typeof(Actor))]
    public class ActorEditor : Editor
    {
        private Actor myTarget;
        private GameObject gameObject;

        private bool foldoutInput = false;

        public override void OnInspectorGUI()
        {
            myTarget = (Actor)target;
            gameObject = myTarget.gameObject;

            if (Application.isPlaying)
            {
                // Show Fields as text so that it is impossible to change the value
                EditorGUILayout.LabelField("Name", myTarget.Name);
                EditorGUILayout.LabelField("Preset", myTarget.Preset.ToString());

                //Example();
            }
            else
            {
                // Show Fields in Editor
                myTarget.Name = EditorGUILayout.TextField("Name", myTarget.Name);
                myTarget.Preset = (Preset)EditorGUILayout.EnumPopup("Preset", myTarget.Preset);

                PresetDefault();

                // Create the required Components
                switch (myTarget.Preset)
                {
                    case Preset.None:
                        ClearAll();
                        break;
                    case Preset.Free:
                        PresetFree();
                        break;
                    case Preset.ThirdPerson:
                        PresetThirdPerson();
                        break;
                    case Preset.Platformer:
                        PresetPlatformer();
                        break;
                }
            }
        }

        // Examples of showing fields
        private void Example()
        {
            // EXAMPLE: Example of how to hide some fields
            GUILayout.BeginVertical();
            foldoutInput = EditorGUILayout.Foldout(foldoutInput, "Foldout");
            if (foldoutInput)
            {
                EditorGUILayout.LabelField("Your ad could be here");
            }
            GUILayout.EndVertical();

            // EXAMPLE: To show a property - EditorGUILayout.PropertyField(new SerializedObject(target).FindProperty("Input")); 
        }

        private void PresetDefault()
        {
            gameObject.AddThisComponent<Actionable>();
            gameObject.AddThisComponent<Animatorable>();
            gameObject.AddThisComponent<Inputable>();
        }

        private void PresetFree()
        {
            ClearThirdPerson();
            ClearPlatformer();

            gameObject.AddThisComponent<MovableFree>();
        }

        private void PresetThirdPerson()
        {
            ClearFree();
            ClearPlatformer();

            gameObject.AddThisComponent<MovableThirdPerson>();

            SphereCollider sphereCollider = gameObject.AddThisComponent<SphereCollider>();
            sphereCollider.radius = 0.25f;
            sphereCollider.center = new Vector3(0, sphereCollider.radius, 0);

            Rigidbody rigidbody = gameObject.AddThisComponent<Rigidbody>();
            rigidbody.freezeRotation = true;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        private void PresetPlatformer()
        {
            ClearFree();
            ClearThirdPerson();

            gameObject.AddThisComponent<MovablePlatformer>();

            CircleCollider2D circleCollider2D = gameObject.AddThisComponent<CircleCollider2D>();
            circleCollider2D.radius = 0.25f;
            circleCollider2D.offset = new Vector2(0, circleCollider2D.radius);

            Rigidbody2D rigidbody = gameObject.AddThisComponent<Rigidbody2D>();
            rigidbody.freezeRotation = true;
            rigidbody.simulated = true;
            rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        private void ClearFree()
        {
            gameObject.RemoveComponent<MovableFree>();
        }

        private void ClearThirdPerson()
        {
            gameObject.RemoveComponent<MovableThirdPerson>();
            gameObject.RemoveComponent<SphereCollider>();
            gameObject.RemoveComponent<Rigidbody>();
        }

        private void ClearPlatformer()
        {
            gameObject.RemoveComponent<MovablePlatformer>();
            gameObject.RemoveComponent<CircleCollider2D>();
            gameObject.RemoveComponent<Rigidbody2D>();
        }

        private void ClearAll()
        {
            gameObject.RemoveComponent<Actionable>();
            gameObject.RemoveComponent<Animatorable>();
            gameObject.RemoveComponent<Inputable>();

            ClearFree();
            ClearThirdPerson();
            ClearPlatformer();
        }
    }
#endif
}