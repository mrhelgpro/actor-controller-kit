using UnityEngine;

namespace AssemblyActorCore
{

    public class Damagable : Model
    {
        private Healthable _healthable;

        protected new void Awake()
        {
            base.Awake();

            _healthable = mainTransform.GetComponent<Healthable>();

            if (_healthable == null)
            {
                gameObject.SetActive(false);
                Debug.LogWarning(gameObject.name + " - Damagable: <Healthable> is not found");
            }
        }

        public virtual void TakeDamage(float value) => _healthable.TakeDamage(value);
    }
}