using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieCoopFPS.Utilities
{
    /// <summary>
    /// Hệ thống Object Pooling đa năng
    /// </summary>
    public class ObjectPool<T> where T : Component, IPoolable
    {
        private Queue<T> availableObjects = new Queue<T>();
        private List<T> activeObjects = new List<T>();
        private T prefab;
        private Transform poolParent;
        private int maxSize;
        
        public int AvailableCount => availableObjects.Count;
        public int ActiveCount => activeObjects.Count;
        
        public ObjectPool(T prefab, int initialSize, int maxSize, Transform parent = null)
        {
            this.prefab = prefab;
            this.maxSize = maxSize;
            this.poolParent = parent;
            
            for (int i = 0; i < initialSize; i++)
            {
                CreateNewObject();
            }
        }
        
        private T CreateNewObject()
        {
            T newObj = UnityEngine.Object.Instantiate(prefab, poolParent);
            newObj.gameObject.SetActive(false);
            newObj.OnReturnToPool = () => Return(newObj);
            availableObjects.Enqueue(newObj);
            return newObj;
        }
        
        public T Get(Vector3 position, Quaternion rotation)
        {
            T obj;
            
            if (availableObjects.Count > 0)
            {
                obj = availableObjects.Dequeue();
            }
            else if (activeObjects.Count < maxSize)
            {
                obj = CreateNewObject();
                availableObjects.Dequeue();
            }
            else
            {
                // Nếu full pool thì không tạo thêm để tránh crash game
                // Debug.LogWarning($"Pool reached max size: {maxSize}");
                return null;
            }
            
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.gameObject.SetActive(true);
            obj.OnSpawnFromPool();
            
            activeObjects.Add(obj);
            return obj;
        }
        
        public void Return(T obj)
        {
            if (!activeObjects.Contains(obj)) 
            {
                if(obj.gameObject.activeSelf) obj.gameObject.SetActive(false);
                return;
            }

            obj.gameObject.SetActive(false);
            
            activeObjects.Remove(obj);
            availableObjects.Enqueue(obj);
        }
        
        public void Clear()
        {
            foreach (var obj in activeObjects) if (obj) UnityEngine.Object.Destroy(obj.gameObject);
            foreach (var obj in availableObjects) if (obj) UnityEngine.Object.Destroy(obj.gameObject);
            
            activeObjects.Clear();
            availableObjects.Clear();
        }
    }
    
    public interface IPoolable
    {
        Action OnReturnToPool { get; set; }
        void OnSpawnFromPool();
    }
}

