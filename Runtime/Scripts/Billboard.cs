using UnityEngine;

namespace Actormachine
{
    public class Billboard : PlayComponentBase
    {
        private Transform _cameraTransform;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        protected override void GameplayFixedUpdate()
        {
            Vector3 cameraToSprite = _cameraTransform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(-cameraToSprite);
        }
    }
}