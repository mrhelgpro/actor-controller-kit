using UnityEngine.AI;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public enum Preset { Free, Physic, Platformer, Navigation }

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
                    case Preset.Free:
                        PresetFree();
                        break;
                    case Preset.Physic:
                        PresetPhysic();
                        break;
                    case Preset.Platformer:
                        PresetPlatformer();
                        break;
                    case Preset.Navigation:
                        PresetNavigation();
                        break;
                }
            }
        }

        // Examples of showing fields
        private bool foldoutInput = false;
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
            ClearNavigation();

            gameObject.AddThisComponent<MovableFree>();
        }

        private void PresetPhysic()
        {
            ClearFree();
            ClearPlatformer();
            ClearNavigation();

            gameObject.AddThisComponent<MovablePhysic>();
            gameObject.AddThisComponent<PositionablePhysic>();

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
            ClearNavigation();

            gameObject.AddThisComponent<MovablePlatformer>();
            gameObject.AddThisComponent<PositionablePlatformer>();

            CircleCollider2D circleCollider2D = gameObject.AddThisComponent<CircleCollider2D>();
            circleCollider2D.radius = 0.25f;
            circleCollider2D.offset = new Vector2(0, circleCollider2D.radius);

            Rigidbody2D rigidbody = gameObject.AddThisComponent<Rigidbody2D>();
            rigidbody.freezeRotation = true;
            rigidbody.simulated = true;
            rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        private void PresetNavigation()
        {
            ClearFree();
            ClearThirdPerson();
            ClearPlatformer();

            gameObject.AddThisComponent<MovableNavigation>();

            NavMeshAgent navMeshAgent = gameObject.AddThisComponent<NavMeshAgent>();
            navMeshAgent.agentTypeID = 0;
            navMeshAgent.radius = 0.25f;
            navMeshAgent.height = 1.25f;
            navMeshAgent.acceleration = 100;
            navMeshAgent.angularSpeed = 10000;
        }

        private void ClearFree()
        {
            gameObject.RemoveComponent<MovableFree>();
        }

        private void ClearThirdPerson()
        {
            gameObject.RemoveComponent<MovablePhysic>();
            gameObject.RemoveComponent<PositionablePhysic>();
            gameObject.RemoveComponent<SphereCollider>();
            gameObject.RemoveComponent<Rigidbody>();
        }

        private void ClearPlatformer()
        {
            gameObject.RemoveComponent<MovablePlatformer>();
            gameObject.RemoveComponent<PositionablePlatformer>();
            gameObject.RemoveComponent<CircleCollider2D>();
            gameObject.RemoveComponent<Rigidbody2D>();
        }

        private void ClearNavigation()
        {
            gameObject.RemoveComponent<MovableNavigation>();
            gameObject.RemoveComponent<NavMeshAgent>();
        }

        private void ClearAll()
        {
            gameObject.RemoveComponent<Actionable>();
            gameObject.RemoveComponent<Animatorable>();
            gameObject.RemoveComponent<Inputable>();

            ClearFree();
            ClearThirdPerson();
            ClearPlatformer();
            ClearNavigation();
        }
    }
#endif
}