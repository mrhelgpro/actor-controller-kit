using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actormachine
{
    public class CursorPosition : MonoBehaviour
    {
        RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            _rectTransform.anchoredPosition = new Vector2(InputController.PointerScreenPosition.x, InputController.PointerScreenPosition.y);
        }
    }
}