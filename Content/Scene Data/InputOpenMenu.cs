using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;

namespace Actormachine
{
    public class InputOpenMenu : MonoBehaviour
    {
        public GameObject Menu;
        public Text Device;
        public bool MouseVisable = false;

        private bool _isGamepad = false;
        private EventSystem _eventSystem;
        private PlayerInput _playerInput;

        private void Awake()
        {
            Time.timeScale = 1;

            InputSystem.CursorVisible(MouseVisable);

            _playerInput = GetComponent<PlayerInput>();
            _eventSystem = FindObjectOfType<EventSystem>();
        }

        public void OnControlsChanged()
        {
            if (_playerInput)
            {
                _isGamepad = _playerInput.currentControlScheme == "Gamepad" ? true : false;
                Device.text = _isGamepad == true ? "Option (Gamepad)" : "Esc (Keyboard)";

                InputSystem.CursorVisible(_isGamepad ? false : MouseVisable);
            }
        }

        public void OnMenu()
        {
            bool state = Menu.activeSelf ? false : true;

            Time.timeScale = state == true ? 0 : 1;
            Menu.SetActive(state);
            _eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);

            InputSystem.CursorVisible(_isGamepad ? false : state);
        }

        public void LoadSceneByName(string name) => SceneManager.LoadScene(name);

        public void Quit()
        {
            Debug.Log("Quitting game...");

            Application.Quit();
        }
    }
}