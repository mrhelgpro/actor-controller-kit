using UnityEditor;

namespace Actormachine.Editor
{
    public static class ContextMenuEditor
    {
        // Create Bootstrap
        [MenuItem("GameObject/Actormachine/Bootstrap/Actor", false, 0)]
        public static void CreateBootstrapActor() => ContextMenuExtention.CreateBootstrap<ActorBootstrap>();

        [MenuItem("GameObject/Actormachine/Bootstrap/Camera", false, 0)]
        public static void CreateBootstrapCamera() => ContextMenuExtention.CreateBootstrap<CameraBootstrap>();

        // Create a prefab from the resources folder  - Character
        [MenuItem("GameObject/Actormachine/Player/Player Third Person", false, 0)]
        public static void CreatePlayerThirdPerson() => ContextMenuExtention.CreatePrefab("Players", "Player Third Person");

        [MenuItem("GameObject/Actormachine/Player/Player Top Down", false, 0)]
        public static void CreatePlayerTopDown() => ContextMenuExtention.CreatePrefab("Players", "Player Top Down");

        [MenuItem("GameObject/Actormachine/Player/Player Platformer 2D", false, 0)]
        public static void CreatePlayerPlatformer2D() => ContextMenuExtention.CreatePrefab("Players", "Player Platformer 2D");

        // Create a prefab from the resources folder - Camera
        [MenuItem("GameObject/Actormachine/Camera/Point Of Interest", false, 0)]
        public static void CreatePointOfInterest() => ContextMenuExtention.CreatePrefab("Camera", "Point Of Interest", hideInHierarchy: true);

        /*
        // If there is a selected object in the hierarchy and there are no hidden objects in it
        // If the selected object has no hidden objects, show "Hide Child Objects"
        // If the selected object contains hidden objects, show "Show Child Objects"

        // Hide Child Objects.
        [MenuItem("GameObject/Actormachine/Hide Child Objects", true)]
        private static bool IsHideChildObjects() => Selection.activeGameObject == null ? false : Selection.activeGameObject.IsHiddenChildObject() == false;

        [MenuItem("GameObject/Actormachine/Hide Child Objects")]
        public static void HideChildObjects() => Selection.activeGameObject.HideChildObjects(true);

        // Show Child Objects
        [MenuItem("GameObject/Actormachine/Show Child Objects", true)]
        private static bool IsShowChildObjects() => Selection.activeGameObject == null ? false : Selection.activeGameObject.IsHiddenChildObject() == true;

        [MenuItem("GameObject/Actormachine/Show Child Objects")]
        public static void ShowChildObjects() => Selection.activeGameObject.HideChildObjects(false);
        */
    }
}