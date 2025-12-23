using UnityEngine;
using System;

namespace ZombieCoopFPS
{
    public static class GameEvents
    {
        // --- COMBAT EVENTS ---
        // Sự kiện nổ (Vị trí, Bán kính)
        public static event Action<Vector3, float> OnExplosion;
        public static void TriggerExplosion(Vector3 position, float radius) 
        {
            OnExplosion?.Invoke(position, radius);
        }

        // --- HORDE WAVE EVENTS ---
        // Sự kiện bắt đầu đợt quái
        public static event Action OnHordeWaveStarted;
        public static void TriggerHordeWaveStarted() => OnHordeWaveStarted?.Invoke();

        // Sự kiện kết thúc đợt quái
        public static event Action OnHordeWaveCompleted;
        public static void TriggerHordeWaveCompleted() => OnHordeWaveCompleted?.Invoke();
    }
}