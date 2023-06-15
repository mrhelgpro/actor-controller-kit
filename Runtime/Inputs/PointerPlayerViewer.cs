using UnityEngine;

namespace Actormachine
{


    [AddComponentMenu("Actormachine/Input/Pointer Player Viewer")]
    public class PointerPlayerViewer : MonoBehaviour
    {
        public GameObject PointerScreenPrefab;
        public GameObject PointerGroundPrefab;
        public GameObject PointerScopePrefab;

        private RectTransform _pointerScreen;
        private RectTransform _pointerGround;
        private RectTransform _pointerScope;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Canvas Settings
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            RectTransform rectTransform = GetComponent<RectTransform>();

            // Pointer Screen Position
            GameObject instantiatedPointerScreen = Instantiate(PointerScreenPrefab);
            instantiatedPointerScreen.name = PointerScreenPrefab.name;
            instantiatedPointerScreen.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

            _pointerScreen = instantiatedPointerScreen.GetComponent<RectTransform>();
            _pointerScreen.SetParent(rectTransform);

            // Pointer Ground Position
            GameObject instantiatedPointerGround = Instantiate(PointerGroundPrefab);
            instantiatedPointerGround.name = PointerGroundPrefab.name;
            instantiatedPointerGround.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

            _pointerGround = instantiatedPointerGround.GetComponent<RectTransform>();
            _pointerGround.SetParent(null);

            // Pointer Scope Position
            GameObject instantiatedPointerScope = Instantiate(PointerScopePrefab);
            instantiatedPointerScope.name = PointerScopePrefab.name;
            instantiatedPointerScope.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

            _pointerScope = instantiatedPointerScope.GetComponent<RectTransform>();
            _pointerScope.SetParent(rectTransform);
            _pointerScope.anchoredPosition = Vector2.zero;
        }

        private void Update()
        {
            if (Pointer.ScreenMode == PointerScreenMode.Visible)
            {
                _pointerScreen.anchoredPosition = new Vector2(Pointer.ScreenPosition.x, Pointer.ScreenPosition.y);
            }
            else
            {
                _pointerScreen.anchoredPosition = new Vector2(-100, -100);
            }

            setPointerGround();

            if (Pointer.ScopeMode == PointerScopeMode.Visible)
            {
                _pointerScope.anchoredPosition = Vector2.zero;
            }
            else
            {
                _pointerScreen.anchoredPosition = new Vector2(-100, -100);
            }

            //Mouse.current.WarpCursorPosition(InputController.PointerScreenPosition);
        }

        private void setPointerGround()
        {
            if (Pointer.GroundMode == PointerGroundMode.Visible)
            {
                if (Pointer.GroundPosition.IsExists)
                {
                    _pointerGround.gameObject.SetActive(true);
                    _pointerGround.position = Pointer.GroundPosition.GetPosition;

                    return;
                }
            }

            _pointerGround.gameObject.SetActive(false);
        }

        public static void Create()
        {
            GameObject instantiate = new GameObject("Pointer Player Viewer", typeof(RectTransform));
            instantiate.transform.position = Vector3.zero;
            instantiate.transform.rotation = Quaternion.identity;

            instantiate.AddComponent<PointerPlayerViewer>();
            instantiate.AddComponent<InputPlayerController>();
        }
    }

    public enum PointerScreenMode { None, Visible }
    public enum PointerGroundMode { None, Visible }
    public enum PointerScopeMode { None, Visible }

    public static class Pointer
    {
        [SerializeField] public static PointerScreenMode ScreenMode = PointerScreenMode.None;
        [SerializeField] public static PointerGroundMode GroundMode = PointerGroundMode.None;
        [SerializeField] public static PointerScopeMode ScopeMode = PointerScopeMode.None;

        public static Vector2 ScreenPosition;
        public static Target GroundPosition;
    }
}