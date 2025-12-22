using System;
using UnityEngine;
using ZombieCoopFPS.Combat; // Bắt buộc để nhìn thấy IDamageable

namespace ZombieCoopFPS.Combat
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        private float currentHealth;

        public event Action<float> OnHealthChanged;
        public event Action OnDeath;

        private void Awake() => currentHealth = maxHealth;

        public void TakeDamage(float damage, Vector3 damageSource)
        {
            if (currentHealth <= 0) return;
            
            currentHealth -= damage;
            OnHealthChanged?.Invoke(currentHealth);
            
            if (currentHealth <= 0) Die();
        }

        // Bổ sung overload để khớp với Interface nếu cần
        public void TakeDamage(DamageInfo info) => TakeDamage(info.Damage, info.Source);

        public void Heal(float amount)
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }

        public float GetCurrentHealth() => currentHealth;
        public float GetMaxHealth() => maxHealth;
        public bool IsDead() => currentHealth <= 0;

        private void Die()
        {
            OnDeath?.Invoke();
            // Có thể thêm logic tự hủy GameObject hoặc ragdoll tại đây
             Debug.Log($"{gameObject.name} đã chết!");
        }
    }
}