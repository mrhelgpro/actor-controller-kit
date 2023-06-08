using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Actormachine
{
    public class CursorPosition : MonoBehaviour
    {
        RectTransform _rectTransform;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _rectTransform.anchoredPosition = new Vector2(InputController.PointerScreenPosition.x, InputController.PointerScreenPosition.y);

            //Mouse.current.WarpCursorPosition(InputController.PointerScreenPosition);
        }
    }
}