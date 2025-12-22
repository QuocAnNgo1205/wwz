using UnityEngine;
using System.Collections.Generic;
using ZombieCoopFPS.Utilities;

namespace ZombieCoopFPS.Enemy
{
    public class ZombieManager : MonoBehaviour
    {
        [Header("Spawning Configuration")]
        [SerializeField] private int maxActiveZombies = 200;
        [SerializeField] private float baseSpawnRate = 1f;
        [SerializeField] private int zombiesPerWave = 5;
        [SerializeField] private Transform[] spawnPoints;
        
        [Header("Pool Configuration")]
        [SerializeField] private GameObject swarmZombiePrefab; 
        
        private ObjectPool<ZombieAI> zombiePool;
        private List<ZombieAI> activeZombies = new List<ZombieAI>();
        private bool isSpawning = false;
        private Transform playerTransform;
        
        public int ActiveZombieCount => activeZombies.Count;
        
        public void Initialize()
        {
            // 1. Tự động tìm Prefab nếu chưa gán trong Inspector
            if (swarmZombiePrefab == null)
            {
                // Tool setup đặt tên là "Zombie_Standard", không phải "StandardZombie"
                swarmZombiePrefab = Resources.Load<GameObject>("Zombies/Zombie_Standard");
            }

            // 2. Kiểm tra lại lần nữa xem có Prefab chưa
            if (swarmZombiePrefab == null)
            {
                Debug.LogError("❌ LỖI NGHIÊM TRỌNG: Không tìm thấy Prefab Zombie! Hãy gán vào ô 'Swarm Zombie Prefab' trong Inspector của GameObject ZombieManager.");
                return;
            }

            // 3. Kiểm tra xem Prefab có gắn script ZombieAI chưa
            if (swarmZombiePrefab.GetComponent<ZombieAI>() == null)
            {
                Debug.LogError("❌ LỖI: Prefab Zombie có tồn tại nhưng thiếu script 'ZombieAI'. Hãy chạy lại Setup Window -> Rebuild Scene.");
                return;
            }

            // 4. Tạo Pool
            if (zombiePool == null)
            {
                zombiePool = new ObjectPool<ZombieAI>(
                    swarmZombiePrefab.GetComponent<ZombieAI>(),
                    50, 300 
                );
                Debug.Log("✓ Zombie Pool initialized successfully.");
            }
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
        }
        
        private void Start() => Initialize();

        public void StartSpawning()
        {
            isSpawning = true;
            InvokeRepeating(nameof(SpawnWave), 0f, baseSpawnRate);
        }
        
        public void StopSpawning()
        {
            isSpawning = false;
            CancelInvoke(nameof(SpawnWave));
        }
        
        private void SpawnWave()
        {
            if (activeZombies.Count >= maxActiveZombies) return;
            
            int count = Mathf.Min(zombiesPerWave, maxActiveZombies - activeZombies.Count);
            for (int i = 0; i < count; i++) SpawnZombie();
        }
        
        public ZombieAI SpawnZombie(Vector3? position = null, ZombieType type = ZombieType.Standard)
        {
            // Khởi tạo lại nếu chưa có (Phòng hờ)
            if (zombiePool == null) Initialize();

            // --- FIX LỖI NULL REFERENCE TẠI ĐÂY ---
            // Nếu Initialize thất bại (vẫn null), thì dừng lại ngay, đừng chạy tiếp lệnh Get()
            if (zombiePool == null) 
            {
                Debug.LogWarning("⚠️ Không thể Spawn Zombie vì Pool chưa khởi tạo (Thiếu Prefab).");
                return null;
            }
            // -------------------------------------

            Vector3 spawnPos = position ?? GetRandomSpawnPoint();
            ZombieAI zombie = zombiePool.Get(spawnPos, Quaternion.identity);
            
            if (zombie != null)
            {
                activeZombies.Add(zombie);
                zombie.OnDeath += HandleZombieDeath;
                zombie.Initialize(); 
            }
            return zombie;
        }
        
        private void HandleZombieDeath(ZombieAI zombie)
        {
            zombie.OnDeath -= HandleZombieDeath;
            activeZombies.Remove(zombie);
            zombiePool.Return(zombie);
        }

        public void ResetSystem()
        {
            StopSpawning();
            for (int i = activeZombies.Count - 1; i >= 0; i--)
            {
                var zombie = activeZombies[i];
                if (zombie != null)
                {
                    zombie.OnDeath -= HandleZombieDeath;
                    zombiePool.Return(zombie);
                }
            }
            activeZombies.Clear();
        }
        
        private Vector3 GetRandomSpawnPoint()
        {
            if (spawnPoints != null && spawnPoints.Length > 0)
                return spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                
            if (playerTransform) {
                Vector2 r = Random.insideUnitCircle * 20f;
                return playerTransform.position + new Vector3(r.x, 0, r.y);
            }
            return Vector3.zero;
        }
    }
}