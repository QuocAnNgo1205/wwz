using System;
using UnityEngine;
using ZombieCoopFPS.Combat; // Để dùng DamageInfo nếu cần
using ZombieCoopFPS.Enemy;  // Để dùng ZombieType

namespace ZombieCoopFPS.Core
{
    public static class GameEvents
    {
        // Player events
        public static event Action<int> OnPlayerJoined;
        public static event Action<int> OnPlayerLeft;
        public static event Action<int, float> OnPlayerHealthChanged;
        public static event Action<int> OnPlayerDied;
        public static event Action<int> OnPlayerRespawned;
        
        // Combat events
        public static event Action<GameObject, GameObject, float> OnDamageDealt;
        public static event Action<Vector3, float> OnExplosion;
        
        // Economy events
        public static event Action<int, int> OnCurrencyEarned;
        public static event Action<int, int> OnCurrencySpent;
        public static event Action<int, string> OnItemPurchased;
        
        // Enemy events
        public static event Action<ZombieType, Vector3> OnZombieSpawned;
        public static event Action<ZombieAI, GameObject> OnZombieKilled;
        public static event Action OnHordeWaveStarted;
        public static event Action OnHordeWaveCompleted;
        
        // ... (Bạn giữ nguyên phần còn lại của class GameEvents trong code bạn gửi) ...
        
        // Trigger Methods (Copy nốt phần Trigger phía dưới vào đây)
        public static void TriggerZombieKilled(ZombieAI zombie, GameObject killer) => OnZombieKilled?.Invoke(zombie, killer);
        public static void TriggerCurrencyEarned(int playerId, int amount) => OnCurrencyEarned?.Invoke(playerId, amount);
        // ... v.v ...
        
        public static void ClearAllEvents()
        {
            OnPlayerJoined = null;
            OnPlayerLeft = null;
            OnZombieKilled = null;
            // ... Clear hết các event để tránh lỗi memory leak khi đổi scene
        }
    }
}