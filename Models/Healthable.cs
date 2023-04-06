using UnityEngine;

namespace AssemblyActorCore
{
    public class Healthable : MonoBehaviour
    {
        public float GetHealth => _currentHealth;
        public bool IsLastDamage(float value) => _currentHealth - Mathf.Abs(value) <= 0;

        [Range (1, 10)] public int Health = 1; // Temporary field to replace the loading function
        private float _currentHealth;
        private float _maxHealth;

        private void Awake()
        {
            loadHealthData();
        }

        private void loadHealthData()
        {
            _currentHealth = Health; // Make loading current Health
            _maxHealth = Health;     // Make loading max Health
        }

        public void TakeDamage(float value)
        {
            _currentHealth -= Mathf.Abs(value);
        }

        public void TakeHealing(float value)
        {
            float health = _currentHealth + Mathf.Abs(value);
            _currentHealth = health > _maxHealth ? _maxHealth : health;
        }
    }
}