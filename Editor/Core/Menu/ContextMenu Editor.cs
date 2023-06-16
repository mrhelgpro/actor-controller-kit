using UnityEditor;

namespace Actormachine.Editor
{
    public static class ContextMenuEditor
    {
        // Create a Input Player Viewer
        [MenuItem("GameObject/Actormachine/Input/Input Player Viewer", false, 0)]
        public static void CreateInputPlayerViewer() => PointerPlayerViewer.Create();

        // Create a prefab from the resources folder  - Character
        [MenuItem("GameObject/Actormachine/Player/Player (Physic)", false, 0)]
        public static void CreatePlayerPhysic() => ContextMenuExtention.CreatePrefab("Players", "Player (Physic)");

        [MenuItem("GameObject/Actormachine/Player/Player (Navigation)", false, 0)]
        public static void CreatePlayerNavigation() => ContextMenuExtention.CreatePrefab("Players", "Player (Navigation)");

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