using UnityEngine;
using UnityEditor;

namespace AssemblyActorCore
{
    public enum EnumMode { Free, ThirdPerson, Platformer }

    public class Actor : MonoBehaviour
    {
        public EnumMode Mode = EnumMode.Free;
        public string Name = "Actor";
        public Inputable Input = new Inputable();
        public Actionable Actionable = new Actionable();
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
                EditorGUILayout.LabelField("Mode", myTarget.Mode.ToString());

                //if (myTarget.GetAction != null) EditorGUILayout.LabelField("Action", myTarget.GetAction.gameObject.name);
                if (myTarget.Actionable.IsAction(null) == false) EditorGUILayout.LabelField("Action", myTarget.Actionable.GetName);

                //Example();
            }
            else
            {
                // Show Fields in Editor
                myTarget.Name = EditorGUILayout.TextField("Name", myTarget.Name);
                myTarget.Mode = (EnumMode)EditorGUILayout.EnumPopup("Mode", myTarget.Mode);

                // Create the required Components
                switch (myTarget.Mode)
                {
                    case EnumMode.Free:
                        PresetFree();
                        break;
                    case EnumMode.ThirdPerson:
                        PresetThirdPerson();
                        break;
                    case EnumMode.Platformer:
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

        private void PresetFree()
        {
            ClearThirdPerson();
            ClearPlatformer();
        }

        private void PresetThirdPerson()
        {
            ClearPlatformer();

            SphereCollider sphereCollider = gameObject.GetComponent<SphereCollider>() == null ? gameObject.AddComponent<SphereCollider>() : gameObject.GetComponent<SphereCollider>();
            sphereCollider.radius = 0.25f;
            sphereCollider.center = new Vector3(0, sphereCollider.radius, 0);

            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>() == null ? gameObject.AddComponent<Rigidbody>() : gameObject.GetComponent<Rigidbody>();
            rigidbody.freezeRotation = true;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        private void PresetPlatformer()
        {
            ClearThirdPerson();

            CircleCollider2D circleCollider2D = gameObject.GetComponent<CircleCollider2D>() == null ? gameObject.AddComponent<CircleCollider2D>() : gameObject.GetComponent<CircleCollider2D>();
            circleCollider2D.radius = 0.25f;
            circleCollider2D.offset = new Vector2(0, circleCollider2D.radius);

            Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>() == null ? gameObject.AddComponent<Rigidbody2D>() : gameObject.GetComponent<Rigidbody2D>();
            rigidbody.freezeRotation = true;
            rigidbody.simulated = true;
            rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        private void ClearThirdPerson()
        {
            if (gameObject.GetComponent<SphereCollider>() != null) DestroyImmediate(gameObject.GetComponent<SphereCollider>());
            if (gameObject.GetComponent<Rigidbody>() != null) DestroyImmediate(gameObject.GetComponent<Rigidbody>());
        }

        private void ClearPlatformer()
        {
            if (gameObject.GetComponent<CircleCollider2D>() != null) DestroyImmediate(gameObject.GetComponent<CircleCollider2D>());
            if (gameObject.GetComponent<Rigidbody2D>() != null) DestroyImmediate(gameObject.GetComponent<Rigidbody2D>());
        }
    }
#endif
}

namespace AssemblyActorCore
{

}