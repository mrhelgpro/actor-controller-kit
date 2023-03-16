using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyActorCore
{
    public class ActivateByEnable : MonoBehaviour
    {
        public Input ActivateInput;
        private Actionable _actionable;
        private GameObject _myGameObject;

        private void Awake()
        {
            _myGameObject = gameObject;
            _actionable = GetComponentInParent<Actionable>();
        }

        private void OnEnable() => _actionable.Activate(_myGameObject);

        private void Update()
        {
            
        }
    }
}