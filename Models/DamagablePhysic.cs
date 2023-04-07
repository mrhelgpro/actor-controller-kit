using UnityEngine;

namespace AssemblyActorCore
{
    public class DamagablePhysic : Damagable
    {
        private Collider _bodyCollider;

        private void Start()
        {
            _bodyCollider = GetComponent<Collider>();

            if (_bodyCollider == null)
            {
                gameObject.SetActive(false);
                Debug.LogWarning(gameObject.name + " - Damagable: <Collider> is not found");
            }
            else
            {
                _bodyCollider.isTrigger = true;

                gameObject.tag = mainTransform.tag;
                gameObject.layer = LayerMask.NameToLayer("Damagable");
            }
        }
    }
}