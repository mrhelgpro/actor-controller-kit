using UnityEngine.AI;
using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public enum Preset { Free, Physic, Platformer, Navigation, None }

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
        private Preset _previousPreset;

        public override void OnInspectorGUI()
        {
            myTarget = (Actor)target;
            gameObject = myTarget.gameObject;

            if (Application.isPlaying)
            {
                // Show Fields as text so that it is impossible to change the value
                EditorGUILayout.LabelField("Name", myTarget.Name);
                EditorGUILayout.LabelField("Preset", myTarget.Preset.ToString());
            }
            else
            {
                // Show Fields in Editor
                myTarget.Name = EditorGUILayout.TextField("Name", myTarget.Name);
                myTarget.Preset = (Preset)EditorGUILayout.EnumPopup("Preset", myTarget.Preset);

                if (myTarget.Preset != _previousPreset)
                {
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
                        case Preset.None:
                            ClearAll();
                            break;
                    }
                }

                _previousPreset = myTarget.Preset;
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
            gameObject.AddThisComponent<Inputable>();
            gameObject.AddThisComponent<Actionable>();
            gameObject.AddThisComponent<Animatorable>();
            gameObject.AddThisComponent<Rotable>();
        }

        private void PresetFree()
        {
            ClearThirdPerson();
            ClearPlatformer();
            ClearNavigation();

            gameObject.AddThisComponent<MovableFree>();
            gameObject.AddThisComponent<PositionableFree>();
        }

        private void PresetPhysic()
        {
            ClearFree();
            ClearPlatformer();
            ClearNavigation();

            gameObject.AddThisComponent<MovablePhysic>();
            gameObject.AddThisComponent<PositionablePhysic>();

            SphereCollider collider = gameObject.AddThisComponent<SphereCollider>();
            collider.radius = 0.25f;
            collider.center = new Vector3(0, collider.radius, 0);

            Rigidbody rigidbody = gameObject.AddThisComponent<Rigidbody>();
            rigidbody.mass = 1;
            rigidbody.drag = 0;
            rigidbody.angularDrag = 0.05f;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbody.freezeRotation = true;
        }

        private void PresetPlatformer()
        {
            ClearFree();
            ClearThirdPerson();
            ClearNavigation();

            gameObject.AddThisComponent<MovablePlatformer>();
            gameObject.AddThisComponent<PositionablePlatformer>();

            CircleCollider2D collider = gameObject.AddThisComponent<CircleCollider2D>();
            collider.isTrigger = false;
            collider.radius = 0.25f;
            collider.offset = new Vector2(0, collider.radius);

            Rigidbody2D rigidbody = gameObject.AddThisComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            rigidbody.simulated = true;
            rigidbody.useAutoMass = false;
            rigidbody.mass = 1;
            rigidbody.drag = 0;
            rigidbody.angularDrag = 0.05f;
            rigidbody.gravityScale = 1;
            rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigidbody.freezeRotation = true;
        }

        private void PresetNavigation()
        {
            ClearFree();
            ClearThirdPerson();
            ClearPlatformer();

            gameObject.AddThisComponent<MovableNavigation>();
            gameObject.AddThisComponent<PositionableFree>();

            NavMeshAgent navMeshAgent = gameObject.AddThisComponent<NavMeshAgent>();
            navMeshAgent.agentTypeID = 0;
            navMeshAgent.baseOffset = 0;
            navMeshAgent.speed = 2f;
            navMeshAgent.angularSpeed = 10000;
            navMeshAgent.acceleration = 100;
            navMeshAgent.stoppingDistance = 0;
            navMeshAgent.autoBraking = true;
            navMeshAgent.radius = 0.25f;
            navMeshAgent.height = navMeshAgent.radius * 2;
            navMeshAgent.avoidancePriority = 50;
            navMeshAgent.autoTraverseOffMeshLink = true;
            navMeshAgent.autoRepath = true;
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
            gameObject.RemoveComponent<PositionableFree>();
            gameObject.RemoveComponent<NavMeshAgent>();
        }

        private void ClearAll()
        {
            gameObject.RemoveComponent<Inputable>();
            gameObject.RemoveComponent<Actionable>();
            gameObject.RemoveComponent<Animatorable>();
            gameObject.RemoveComponent<Rotable>();

            ClearFree();
            ClearThirdPerson();
            ClearPlatformer();
            ClearNavigation();
        }
    }
#endif
}

namespace AssemblyActorCore
{

}