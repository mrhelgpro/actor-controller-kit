using UnityEngine;

namespace AssemblyActorCore
{
    public class Healthable : MonoBehaviour
    {
        public float GetHealth => _currentHealth;
        public bool IsDead => _currentHealth <= 0;

        [Range (1, 10)] public int Health = 1; // Temporary field to replace the loading function
        
        private float _currentHealth;
        private float _maxHealth;

        private void Awake()
        {
            _currentHealth = Health; // Make loading current Health
            _maxHealth = Health;     // Make loading max Health
        }

        public void TakeDamage(float value)
        {
            float health = _currentHealth - Mathf.Abs(value);
            _currentHealth = health < 0 ? 0 : health;
        }

        public void TakeHealing(float value)
        {
            float health = _currentHealth + Mathf.Abs(value);
            _currentHealth = health > _maxHealth ? _maxHealth : health;
        }

        public void TakeKill()
        {
            _currentHealth = 0;
        }
    }
}