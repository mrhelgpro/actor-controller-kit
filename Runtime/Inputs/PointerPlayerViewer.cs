using UnityEngine;
using UnityEngine.UI;

namespace Actormachine
{
    [AddComponentMenu("Actormachine/Input/Pointer Player Viewer")]
    public class PointerPlayerViewer : MonoBehaviour
    {
        public PointerScreen.Mode PointerScreenMode = PointerScreen.Mode.None;
        public PointerMovement.Mode PointerMovementMode = PointerMovement.Mode.None;
        public PointerScope.Mode PointerScopeMode = PointerScope.Mode.None;

        public GameObject PointerScreenPrefab;
        public GameObject PointerGroundPrefab;
        public GameObject PointerScopePrefab;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Canvas Settings
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = true;

            RectTransform rectTransform = GetComponent<RectTransform>();

            // Initiation pointers
            PointerScreen.Create(PointerScreenPrefab, rectTransform);
            PointerMovement.Create(PointerGroundPrefab);
            PointerScope.Create(PointerScopePrefab, rectTransform);

            // Set Visible
            PointerScreen.SetMode(PointerScreenMode);
            PointerMovement.SetMode(PointerMovementMode);
            PointerScope.SetMode(PointerScopeMode);
        }

        public static void Create()
        {
            GameObject instantiate = new GameObject("Input Player Viewer", typeof(RectTransform));
            instantiate.transform.position = Vector3.zero;
            instantiate.transform.rotation = Quaternion.identity;

            Canvas canvas = instantiate.AddRequiredComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = true;

#if UNITY_2022_1_OR_NEWER
            canvas.vertexColorAlwaysGammaSpace = true;
#endif

            instantiate.AddRequiredComponent<CanvasScaler>();
            instantiate.AddRequiredComponent<GraphicRaycaster>();

            instantiate.AddComponent<PointerPlayerViewer>();
            instantiate.AddComponent<InputPlayerController>();
        }
    }

    public static class PointerScreen
    {
        public enum Mode { None, Visible }
        private static Mode _mode = Mode.Visible;
        private static Vector2 _position;
        private static RectTransform _transform;

        // Set Values
        public static void Create(GameObject prefab, Transform parent)
        {
            _transform = PointerExtention.Create(prefab, parent);
        }

        public static void SetMode(Mode mode)
        {
            _mode = mode;

            if (_mode == Mode.None)
            {
                if (_transform)
                {
                    _transform.gameObject.SetActive(false);
                }
            }
            else
            {
                if (_transform)
                {
                    _transform.gameObject.SetActive(true);
                }
            }
        }

        public static void SetPosition(Vector3 position)
        {
            _position.x = Mathf.Clamp(position.x, 0f, Screen.width);
            _position.y = Mathf.Clamp(position.y, 0f, Screen.height);

            if (_mode == Mode.Visible)
            {
                if (_transform)
                {
                    _transform.anchoredPosition = new Vector2(_position.x, _position.y);
                }
            }
        }

        // Get Values
        public static Mode GetMode => _mode;

        public static Vector2 GetPosition => _position;
    }

    public static class PointerMovement
    {
        public enum Mode { None, Visible }
        private static Mode _mode = Mode.Visible;       
        private static Vector3 _position;
        private static bool _isActive = false;
        private static RectTransform _transform;
        private static Animator _animator;

        // Set Values
        public static void Create(GameObject prefab)
        {
            _transform = PointerExtention.Create(prefab, null);

            _animator = _transform.GetComponent<Animator>();
        }

        public static void SetMode(Mode mode) => _mode = mode;  

        public static void SetPosition(Vector3 position)
        {
            _isActive = true;
            _position = position;

            if (_mode == Mode.Visible)
            {
                if (_transform)
                {
                    _transform.gameObject.SetActive(true);
                    _transform.position = _position;
                }

                if (_animator) _animator.Play("Start", 0, 0);
            } 
        }

        public static void Clear()
        {
            _isActive = false;

            if (_transform) _transform.gameObject.SetActive(false);
        }

        // Get Values
        public static Mode GetMode => _mode;
        public static bool IsActive => _isActive;
        public static Vector3 GetPosition => IsActive ? _position : Vector3.zero;
        public static float GetDistance(Vector3 startPosition) => IsActive ? Vector3.Distance(startPosition, _position) : 0;
        public static Vector3 GetDirection(Vector3 startPosition) => IsActive ? (_position - startPosition).normalized : Vector3.zero;
        public static float GetDistanceHorizontal(Vector3 startPosition) => IsActive ? Vector2.Distance(new Vector2(_position.x, _position.z), new Vector2(startPosition.x, startPosition.z)) : 0;
        public static Vector2 GetDirectionHorizontal(Vector3 startPosition) => IsActive ? new Vector2(GetDirection(startPosition).x, GetDirection(startPosition).z) : Vector3.zero;
    }

    public static class PointerScope
    {
        public enum Mode { None, Visible }
        private static Mode _mode = Mode.Visible;
        private static RectTransform _transform;

        // Set Values
        public static void Create(GameObject prefab, Transform parent)
        {
            _transform = PointerExtention.Create(prefab, parent);
        }

        public static void SetMode(Mode mode)
        {
            _mode = mode;

            if (_mode == Mode.None)
            {
                if (_transform)
                {
                    _transform.gameObject.SetActive(false);
                }
            }
            else
            {
                if (_transform)
                {
                    _transform.gameObject.SetActive(true);
                    _transform.anchoredPosition = Vector2.zero;
                }
            }
        }

        // Get Values
        public static Mode GetMode => _mode;
    }

    public class PointerExtention
    {
        public static RectTransform Create(GameObject prefab, Transform parent)
        {
            GameObject instantiated = GameObject.Instantiate(prefab);
            instantiated.name = prefab.name;
            instantiated.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

            RectTransform rectTransform = instantiated.GetComponent<RectTransform>();
            rectTransform.SetParent(parent);
            rectTransform.gameObject.SetActive(false);

            return rectTransform;
        }
    }
}