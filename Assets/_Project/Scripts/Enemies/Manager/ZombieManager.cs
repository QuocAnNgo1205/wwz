using UnityEngine;
using System.Collections.Generic;
using ZombieCoopFPS.Utilities;

namespace ZombieCoopFPS.Enemy
{
    // Đã xóa enum ZombieType ở đây để tránh lỗi CS0101

    public class ZombieManager : MonoBehaviour
    {
        public static ZombieManager Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private int maxActiveZombies = 500;
        
        // Prefab References
        [SerializeField] private GameObject standardPrefab;
        [SerializeField] private GameObject tankPrefab;
        [SerializeField] private GameObject exploderPrefab;
        
        private ObjectPool<ZombieAI> standardPool;
        private List<ZombieAI> activeZombies = new List<ZombieAI>();
        
        // --- FIX LỖI 1: Hồi phục tên biến cũ ---
        public int ActiveCount => activeZombies.Count;
        public int ActiveZombieCount => activeZombies.Count; // Alias cho code cũ
        // ---------------------------------------

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            
            Initialize();
        }

        public void Initialize()
        {
            if (!standardPrefab) standardPrefab = Resources.Load<GameObject>("Zombies/Zombie_Standard");
            if (!tankPrefab) tankPrefab = Resources.Load<GameObject>("Zombies/Zombie_Tank");
            if (!exploderPrefab) exploderPrefab = Resources.Load<GameObject>("Zombies/Zombie_Exploder");

            if (standardPool == null && standardPrefab != null)
            {
                standardPool = new ObjectPool<ZombieAI>(standardPrefab.GetComponent<ZombieAI>(), 100, 1000);
            }
        }

        // --- FIX LỖI 2: Hồi phục hàm Start/Stop Spawning ---
        // Các hàm này sẽ gọi sang HordeWaveSystem hoặc để trống để tránh lỗi compile
        public void StartSpawning()
        {
            Debug.Log("⚠️ ZombieManager: Spawning is now handled by HordeWaveSystem.");
            // Nếu muốn kích hoạt wave ngay lập tức:
            HordeWaveSystem waveSys = FindFirstObjectByType<HordeWaveSystem>();
            if (waveSys != null && !waveSys.IsWaveActive) waveSys.StartNextWave();
        }

        public void StopSpawning()
        {
            // HordeWaveSystem tự quản lý việc dừng, hàm này để trống để GameManager không báo lỗi
        }
        // ---------------------------------------------------

        public ZombieAI SpawnZombie(Vector3? pos = null, ZombieType type = ZombieType.Standard)
        {
            if (activeZombies.Count >= maxActiveZombies) return null;

            Vector3 spawnPos = pos ?? GetRandomSpawnPos();
            ZombieAI zombie = null;

            // Logic phân loại
            if (type == ZombieType.Standard)
            {
                if(standardPool != null)
                    zombie = standardPool.Get(spawnPos, Quaternion.identity);
            }
            else
            {
                GameObject prefab = null;
                if (type == ZombieType.Tank) prefab = tankPrefab;
                else if (type == ZombieType.Exploder) prefab = exploderPrefab;
                if (type == ZombieType.Grabber) prefab = standardPrefab; 

                if (prefab != null)
                {
                    GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
                    zombie = obj.GetComponent<ZombieAI>();
                }
            }

            if (zombie != null)
            {
                activeZombies.Add(zombie);
                zombie.OnDeath += HandleDeath;
                zombie.Initialize();
            }
            return zombie;
        }

        private void HandleDeath(ZombieAI z)
        {
            z.OnDeath -= HandleDeath;
            activeZombies.Remove(z);
            
            if (z.Type == ZombieType.Standard && standardPool != null) 
            {
                standardPool.Return(z);
            }
            else 
            {
                Destroy(z.gameObject, 2f);
            }
        }
        
        public void ResetSystem() 
        {
            StopAllCoroutines();
            for (int i = activeZombies.Count - 1; i >= 0; i--)
            {
                var z = activeZombies[i];
                if (z != null)
                {
                    z.OnDeath -= HandleDeath;
                    if(z.Type == ZombieType.Standard && standardPool != null) standardPool.Return(z);
                    else Destroy(z.gameObject);
                }
            }
            activeZombies.Clear();
        }

        private Vector3 GetRandomSpawnPos()
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) return p.transform.position + (Vector3)Random.insideUnitCircle * 25f;
            return Vector3.zero;
        }
    }
}