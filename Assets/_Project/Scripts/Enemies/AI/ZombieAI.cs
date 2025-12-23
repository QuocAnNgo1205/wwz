using UnityEngine;
using System.Collections.Generic;
using ZombieCoopFPS.Combat;
using ZombieCoopFPS.Data; // Added for compatibility
using ZombieCoopFPS.Utilities; // Added for IPoolable
using ZombieCoopFPS.Enemy;

namespace ZombieCoopFPS.Enemy
{
    /// <summary>
    /// World War Z style zombie - NO NavMesh required!
    /// Uses direct movement and swarm behavior for massive hordes
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class ZombieAI : MonoBehaviour, IPoolable, IDamageable, IEnemyAI
    {
        [Header("Zombie Configuration")]
        [SerializeField] private ZombieType zombieType = ZombieType.Standard;
        // Added Data SO reference to keep compatibility with your setup system
        [SerializeField] private ZombieDataSO zombieData; 
        
        [SerializeField] private float health = 100f;
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackCooldown = 1.5f;
        
        [Header("Swarm Behavior")]
        [SerializeField] private float separationRadius = 1.5f;
        [SerializeField] private float cohesionRadius = 5f;
        [SerializeField] private float avoidanceRadius = 2.5f;
        
        private Transform target;
        private CharacterController characterController;
        private Animator animator;
        private float currentHealth;
        private bool isAlive;
        private float lastAttackTime;
        private Vector3 velocity;
        private Vector3 desiredVelocity;
        
        // Swarm neighbors cache
        private List<ZombieAI> nearbyZombies = new List<ZombieAI>();
        private float neighborUpdateInterval = 0.5f;
        private float lastNeighborUpdate;
        
        // State
        private enum State { Idle, Chase, Attack }
        private State currentState = State.Idle;
        
        public ZombieType Type => zombieType;
        public Transform Target => target;
        public bool IsAlive => isAlive;
        public System.Action OnReturnToPool { get; set; }
        
        public event System.Action<ZombieAI> OnDeath;
        
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }
        
        public void OnSpawnFromPool()
        {
            Initialize();
        }
        
        public void Initialize()
        {
            // Load stats from ScriptableObject if available
            if (zombieData != null)
            {
                health = zombieData.MaxHealth;
                moveSpeed = zombieData.MoveSpeed;
                attackDamage = zombieData.AttackDamage;
                attackRange = zombieData.AttackRange;
                zombieType = zombieData.Type;
            }

            currentHealth = health;
            isAlive = true;
            velocity = Vector3.zero;
            currentState = State.Idle;
            
            // Randomize update time slightly to prevent frame spikes
            lastNeighborUpdate = Time.time + Random.Range(0f, 0.5f);
            
            FindTarget();
        }
        
        public void FindTarget()
        {
            // Simple logic: Find Player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                currentState = State.Chase;
            }
        }
        
        protected virtual void Update()
        {
            if (!isAlive) return;

            if(target == null)
            {
                FindTarget();
                return;
            }
            
            // Update neighbors periodically for performance
            if (Time.time - lastNeighborUpdate > neighborUpdateInterval)
            {
                UpdateNearbyZombies();
                lastNeighborUpdate = Time.time;
            }
            
            // State machine
            switch (currentState)
            {
                case State.Chase:
                    ChaseTarget();
                    CheckAttackRange();
                    break;
                    
                case State.Attack:
                    AttackTarget();
                    break;
            }
            
            // Apply movement
            ApplyMovement();
        }
        
        private void ChaseTarget()
        {
            if (target == null) return;
            
            // Calculate direction to target
            Vector3 targetDirection = (target.position - transform.position).normalized;
            
            // Apply swarm behaviors
            Vector3 separation = CalculateSeparation();
            Vector3 cohesion = CalculateCohesion();
            Vector3 avoidance = CalculateObstacleAvoidance();
            
            // Combine forces (Weighted)
            desiredVelocity = targetDirection * moveSpeed; 
            desiredVelocity += separation * 3.5f; // High separation to prevent stacking
            desiredVelocity += cohesion * 0.2f;   // Low cohesion to prevent balling up
            desiredVelocity += avoidance * 5f;    // High avoidance for walls
            
            // Smooth velocity change
            velocity = Vector3.Lerp(velocity, desiredVelocity, Time.deltaTime * 5f);
            
            // Rotate towards movement direction
            if (velocity.magnitude > 0.1f)
            {
                // Zero out Y rotation to prevent tilting
                Vector3 lookVel = velocity;
                lookVel.y = 0;
                if(lookVel != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookVel);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
                }
            }
            
            // Animation
            if (animator != null)
            {
                animator.SetFloat("Speed", velocity.magnitude);
            }
        }
        
        private void AttackTarget()
        {
            if (target == null) { currentState = State.Chase; return; }
            
            // Face target
            Vector3 lookDir = (target.position - transform.position).normalized;
            lookDir.y = 0;
            if (lookDir.magnitude > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 10f);
            }
            
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
            
            if (Vector3.Distance(transform.position, target.position) > attackRange * 1.2f)
            {
                currentState = State.Chase;
            }
        }
        
        private void CheckAttackRange()
        {
            if (target == null) return;
            if (Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                currentState = State.Attack;
                velocity = Vector3.zero;
            }
        }
        
        // --- BOIDS ALGORITHMS ---
        
        private Vector3 CalculateSeparation()
        {
            Vector3 separationForce = Vector3.zero;
            int count = 0;
            
            foreach (var zombie in nearbyZombies)
            {
                if (zombie == null || zombie == this || !zombie.IsAlive) continue;
                
                float distance = Vector3.Distance(transform.position, zombie.transform.position);
                if (distance < separationRadius && distance > 0.1f)
                {
                    Vector3 diff = transform.position - zombie.transform.position;
                    diff.Normalize();
                    diff /= distance; 
                    separationForce += diff;
                    count++;
                }
            }
            return count > 0 ? separationForce / count : Vector3.zero;
        }
        
        private Vector3 CalculateCohesion()
        {
            Vector3 centerOfMass = Vector3.zero;
            int count = 0;
            
            foreach (var zombie in nearbyZombies)
            {
                if (zombie == null || zombie == this || !zombie.IsAlive) continue;
                centerOfMass += zombie.transform.position;
                count++;
            }
            
            if (count > 0)
            {
                centerOfMass /= count;
                return (centerOfMass - transform.position).normalized;
            }
            return Vector3.zero;
        }
        
        private Vector3 CalculateObstacleAvoidance()
        {
            Vector3 avoidanceForce = Vector3.zero;
            RaycastHit hit;
            
            // Check forward, left, and right
            Vector3[] dirs = { transform.forward, transform.forward - transform.right, transform.forward + transform.right };
            
            foreach(var dir in dirs)
            {
                if (Physics.Raycast(transform.position + Vector3.up, dir, out hit, avoidanceRadius))
                {
                    if (!hit.collider.CompareTag("Player") && !hit.collider.GetComponent<ZombieAI>())
                    {
                        avoidanceForce += hit.normal * 2.0f; // Push away from wall normal
                    }
                }
            }
            return avoidanceForce;
        }
        
        private void UpdateNearbyZombies()
        {
            nearbyZombies.Clear();
            // Optimization: Use NonAlloc if possible, but this works for now
            Collider[] colliders = Physics.OverlapSphere(transform.position, cohesionRadius);
            foreach (var col in colliders)
            {
                ZombieAI zombie = col.GetComponent<ZombieAI>();
                if (zombie != null && zombie != this) nearbyZombies.Add(zombie);
            }
        }
        
        private void ApplyMovement()
        {
            if (!isAlive || characterController == null) return;
            
            Vector3 movement = velocity * Time.deltaTime;
            
            // Simple Gravity
            if (!characterController.isGrounded) movement.y = -9.81f * Time.deltaTime;
            else movement.y = -0.1f;
            
            characterController.Move(movement);
        }
        
        // --- INTERFACES ---
        public void Attack()
        {
            if (target == null) return;
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable != null) damageable.TakeDamage(attackDamage, transform.position);
            if (animator != null) animator.SetTrigger("Attack");
        }
        
        public virtual void TakeDamage(float damage, Vector3 source)
        {
            if (!isAlive) return;
            currentHealth -= damage;
            
            // Small knockback
            velocity += (transform.position - source).normalized * 5f;
            
            if (currentHealth <= 0) Die();
        }
        
        public void Heal(float amount) => currentHealth = Mathf.Min(currentHealth + amount, health);
        public float GetCurrentHealth() => currentHealth;
        public float GetMaxHealth() => health;
        public bool IsDead() => !isAlive;
        
        private void Die()
        {
            if (!isAlive) return; // Nếu đã chết rồi thì thôi, tránh gọi nhiều lần

            isAlive = false;
            OnDeath?.Invoke(this); // Báo cho Manager biết để tính điểm/xóa khỏi list active
            
            if (animator) animator.SetTrigger("Death");
            
            // Hủy invoke cũ nếu có để tránh lỗi
            CancelInvoke(); 
            Invoke(nameof(ReturnSelf), 2f); // 2 giây sau thi thể mới biến mất
        }
        
        private void ReturnSelf() => OnReturnToPool?.Invoke();
        
        // Unused interface methods
        public void ChangeState(IEnemyState newState) { }
        public void MoveTo(Vector3 destination) { }
        public bool IsInAttackRange() => Vector3.Distance(transform.position, target.position) <= attackRange;
        public void StaggeredUpdate() { } // Manager uses distinct update logic now
    }
}