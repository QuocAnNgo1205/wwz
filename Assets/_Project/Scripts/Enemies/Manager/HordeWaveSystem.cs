using UnityEngine;
using System.Collections;

using ZombieCoopFPS.Utilities;
using ZombieCoopFPS.Core;

namespace ZombieCoopFPS.Enemy
{
    public class HordeWaveSystem : MonoBehaviour
    {
        [Header("Wave Config")]
        public int currentWave = 0;
        public float timeBetweenWaves = 10f;
        public int baseZombies = 20;
        public float multiplier = 1.2f; // Mỗi wave tăng 20% quái

        // --- FIX LỖI 1: Thêm biến công khai để ZombieManager đọc ---
        public bool IsWaveActive { get; private set; } = false;

        private void Start()
        {
            // Tự động bắt đầu wave 1 sau 3 giây
            Invoke(nameof(StartNextWave), 3f);
        }

        // --- FIX LỖI 2: Thêm hàm công khai để ZombieManager gọi ---
        public void StartNextWave()
        {
            if (IsWaveActive) return; // Nếu đang đánh nhau thì không start đè lên
            
            StartCoroutine(WaveRoutine());
        }

        private IEnumerator WaveRoutine()
        {
            IsWaveActive = true;
            currentWave++;
            
            // Tính số lượng quái: Wave 1 = 20, Wave 2 = 24, Wave 3 = 29...
            int zombiesToSpawn = Mathf.RoundToInt(baseZombies * Mathf.Pow(multiplier, currentWave - 1));
            
            Debug.Log($"🌊 WAVE {currentWave} START! Spawning {zombiesToSpawn} zombies.");

            // --- GIAI ĐOẠN 1: SINH QUÁI ---
            for (int i = 0; i < zombiesToSpawn; i++)
            {
                // Tỉ lệ: 5% Tank, 10% Nổ, 85% Thường
                float r = Random.value;
                ZombieType type = ZombieType.Standard;
                
                if (r < 0.05f) type = ZombieType.Tank;
                else if (r < 0.15f) type = ZombieType.Exploder;

                // Gọi ZombieManager để sinh ra
                if (ZombieManager.Instance)
                    ZombieManager.Instance.SpawnZombie(null, type);
                
                // Delay nhẹ để không spawn 1 cục 500 con (giảm giật lag)
                // Spawn nhanh hơn ở wave cao
                float delay = Mathf.Max(0.05f, 0.5f - (currentWave * 0.02f));
                yield return new WaitForSeconds(delay);
            }

            // --- GIAI ĐOẠN 2: CHỜ NGƯỜI CHƠI DIỆT HẾT ---
            // Chờ cho đến khi số lượng zombie active về 0
            yield return new WaitUntil(() => ZombieManager.Instance.ActiveCount == 0);
            
            Debug.Log($"✅ WAVE {currentWave} CLEARED!");
            IsWaveActive = false;

            // Thưởng tiền (Nếu có hệ thống kinh tế)
            if (GameManager.Instance && GameManager.Instance.EconomyManager)
            {
                GameManager.Instance.EconomyManager.AddCurrency(0, 100 * currentWave);
            }

            // Nghỉ giải lao rồi tự gọi wave tiếp theo
            Debug.Log($"⏳ Next wave in {timeBetweenWaves} seconds...");
            yield return new WaitForSeconds(timeBetweenWaves);
            
            StartNextWave();
        }

        private void OnGUI()
        {
            // Hiển thị thông tin Wave lên màn hình
            if (ZombieManager.Instance)
            {
                string status = IsWaveActive ? "COMBAT" : "RESTING";
                GUI.Box(new Rect(10, 50, 200, 60), 
                    $"Wave: {currentWave} ({status})\n" +
                    $"Zombies Alive: {ZombieManager.Instance.ActiveCount}");
            }
        }
    }
}