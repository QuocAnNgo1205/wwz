using UnityEngine;
using ZombieCoopFPS.Combat;
using ZombieCoopFPS.Player; // Đảm bảo namespace này đúng với PlayerController của bạn

namespace ZombieCoopFPS.Enemy
{
    // --- TANK ZOMBIE ---
    public class TankZombie : ZombieAI
    {
        [Header("Tank Abilities")]
        [SerializeField] private float chargeSpeed = 12f; // Chạy nhanh hơn
        [SerializeField] private float chargeDamage = 50f;
        [SerializeField] private float chargeRange = 15f;
        [SerializeField] private float chargeCooldown = 8f;
        
        private float lastChargeTime;
        private bool isCharging = false;
        
        protected override void Update()
        {
            if (isCharging)
            {
                // Logic húc thẳng
                transform.position += transform.forward * chargeSpeed * Time.deltaTime;
                return; // Đang húc thì không chạy logic bầy đàn
            }

            base.Update(); // Chạy logic AI thường
            
            if (!IsAlive || Target == null) return;
            
            float dist = Vector3.Distance(transform.position, Target.position);
            if (Time.time - lastChargeTime >= chargeCooldown && dist <= chargeRange && dist > 4f)
            {
                StartCharge();
            }
        }
        
        private void StartCharge()
        {
            isCharging = true;
            lastChargeTime = Time.time;
            Debug.Log("🐂 TANK CHARGING!");
            Invoke(nameof(EndCharge), 1.5f); // Húc trong 1.5s
        }
        
        private void EndCharge() => isCharging = false;
        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (isCharging && hit.collider.CompareTag("Player"))
            {
                var dmg = hit.collider.GetComponent<IDamageable>();
                if (dmg != null) dmg.TakeDamage(chargeDamage, transform.position);
                EndCharge();
            }
        }
    }

    // --- EXPLODER ZOMBIE ---
    public class ExploderZombie : ZombieAI
    {
        [Header("Exploder Abilities")]
        [SerializeField] private float explosionRadius = 6f;
        [SerializeField] private float explosionDamage = 80f;
        [SerializeField] private GameObject explosionEffect;
        
        private bool hasExploded = false;
        
        protected override void Update()
        {
            base.Update();
            if (!IsAlive || Target == null || hasExploded) return;
            
            if (Vector3.Distance(transform.position, Target.position) <= 2.5f)
                Explode();
        }
        
        public override void TakeDamage(float damage, Vector3 source)
        {
            base.TakeDamage(damage, source);
            if (IsDead() && !hasExploded) Explode();
        }
        
        private void Explode()
        {
            if (hasExploded) return;
            hasExploded = true;
            
            // Tạo hiệu ứng nổ (Load từ Resources nếu null để tránh lỗi)
            if(explosionEffect == null) explosionEffect = Resources.Load<GameObject>("VFX/Explosion");
            if(explosionEffect) Destroy(Instantiate(explosionEffect, transform.position, Quaternion.identity), 3f);

            // Gây dam
            Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var hit in hits)
            {
                var dmg = hit.GetComponent<IDamageable>();
                if (dmg != null && !hit.GetComponent<ExploderZombie>()) // Không gây dam cho chính nó
                {
                    dmg.TakeDamage(explosionDamage, transform.position);
                }
            }
            
            // Tự hủy
            if (!IsDead()) base.TakeDamage(9999f, transform.position);
        }
    }

    // --- GRABBER ZOMBIE ---
    public class GrabberZombie : ZombieAI
    {
        // Tạm thời dùng AI thường, logic kéo người khá phức tạp với CharacterController
        // Để tránh lỗi Physics, Grabber sẽ chỉ chạy nhanh và đánh đau hơn
        protected override void Update()
        {
            base.Update();
        }
    }
}