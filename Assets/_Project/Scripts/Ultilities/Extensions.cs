using UnityEngine;
using System.Collections.Generic;

namespace ZombieCoopFPS.Utilities
{
    public static class Extensions
    {
        public static Vector3 WithX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);
        public static Vector3 WithY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
        // ... Copy hết các hàm extension trong đoạn code bạn gửi vào đây
        
        public static T GetRandom<T>(this List<T> list)
        {
            if (list.Count == 0) return default(T);
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }

    // Bạn có thể để class Singleton ở đây hoặc tách ra file riêng Assets/Scripts/Utilities/Singleton.cs
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        // ... Copy đoạn logic Singleton vào đây ...
        protected virtual void Awake()
        {
            if (instance == null) { instance = this as T; DontDestroyOnLoad(gameObject); }
            else if (instance != this) { Destroy(gameObject); }
        }
    }
}