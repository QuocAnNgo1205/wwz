using UnityEngine;

namespace ZombieCoopFPS.Combat
{
    public interface IDamageable
    {
        void TakeDamage(float damage, Vector3 damageSource);
        void Heal(float amount);
        float GetCurrentHealth();
        float GetMaxHealth();
        bool IsDead();
        
        // Cung cấp logic mặc định cho overload
        void TakeDamage(DamageInfo damageInfo)
        {
            TakeDamage(damageInfo.Damage, damageInfo.Source);
        }
    }

    public struct DamageInfo
    {
        public float Damage;
        public Vector3 Source;
        public DamageType Type;
        public GameObject Attacker;

        public DamageInfo(float damage, Vector3 source, DamageType type = DamageType.Physical, GameObject attacker = null)
        {
            Damage = damage;
            Source = source;
            Type = type;
            Attacker = attacker;
        }
    }

    public enum DamageType { Physical, Explosive, Fire, Poison, Electric }
}