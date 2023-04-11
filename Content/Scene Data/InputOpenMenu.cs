using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using AssemblyActorCore;
using System.Collections.Generic;

public class InputOpenMenu : MonoBehaviour
{
    public GameObject Menu;
    public Text Device;

    private EventSystem _eventSystem;
    private InputActions _inputActions;

    private void Awake()
    {
        _inputActions = new InputActions();

        _inputActions.Player.Menu.performed += context => checkMenu(true);
        _inputActions.Player.Menu.canceled += context => checkMenu(false);
        _inputActions.Player.Any.performed += context => checkDevice();

        _eventSystem = FindObjectOfType<EventSystem>();
    }

    private void checkDevice()
    {
        Debug.Log("ANY BUTTON");
    }

    private void checkMenu(bool state)
    {
        Time.timeScale = state == true ? 0 : 1;
        Menu.SetActive(state);
        _eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);
    }

    private void OnEnable() => _inputActions.Enable();

    private void OnDisable() => _inputActions.Disable();
}
