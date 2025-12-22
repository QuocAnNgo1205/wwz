using System.Collections.Generic;
using UnityEngine;
using ZombieCoopFPS.Utilities; // Quan trọng: Gọi thư viện Pooling ở trên

namespace ZombieCoopFPS.Enemy
{
    public class ZombiePool
    {
        private Dictionary<ZombieType, ObjectPool<ZombieAI>> pools;
        private Dictionary<ZombieType, ZombieAI> prefabs;
        private int initialSize;
        private int maxSize;
        private Transform parentTransform;
        
        public ZombiePool(int initialSize, int maxSize, Transform parent)
        {
            this.initialSize = initialSize;
            this.maxSize = maxSize;
            this.parentTransform = parent;
            pools = new Dictionary<ZombieType, ObjectPool<ZombieAI>>();
            prefabs = new Dictionary<ZombieType, ZombieAI>();
            
            LoadZombiePrefabs();
            InitializePools();
        }
        
        private void LoadZombiePrefabs()
        {
            // Lưu ý: Bạn cần tạo folder Resources/Zombies và bỏ Prefab vào đó
            prefabs[ZombieType.Standard] = Resources.Load<ZombieAI>("Zombies/StandardZombie");
            // prefabs[ZombieType.Tank] = Resources.Load<ZombieAI>("Zombies/TankZombie");
        }
        
        private void InitializePools()
        {
            foreach (var kvp in prefabs)
            {
                if (kvp.Value != null)
                {
                    int size = kvp.Key == ZombieType.Standard ? initialSize : initialSize / 4;
                    pools[kvp.Key] = new ObjectPool<ZombieAI>(kvp.Value, size, maxSize, parentTransform);
                }
                else
                {
                    Debug.LogWarning($"Không tìm thấy Prefab cho zombie loại: {kvp.Key}. Hãy kiểm tra thư mục Resources.");
                }
            }
        }
        
        public ZombieAI Get(Vector3 position, ZombieType type = ZombieType.Standard)
        {
            if (pools.ContainsKey(type))
            {
                return pools[type].Get(position, Quaternion.identity);
            }
            return null;
        }
        
        public void Return(ZombieAI zombie)
        {
            if (pools.ContainsKey(zombie.Type))
            {
                pools[zombie.Type].Return(zombie);
            }
        }
    }
}