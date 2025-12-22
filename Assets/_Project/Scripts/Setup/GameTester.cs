using UnityEngine;
using ZombieCoopFPS.Core; // Để gọi GameManager
using ZombieCoopFPS.Enemy; // Để gọi ZombieAI

namespace ZombieCoopFPS.Setup
{
    // Class này giờ nằm riêng 1 file, Unity sẽ dễ dàng tìm thấy nó hơn
    public class GameTester : MonoBehaviour
    {
        [Header("Controls")]
        public KeyCode spawnKey = KeyCode.Z;
        public KeyCode spawnHordeKey = KeyCode.X;
        public KeyCode moneyKey = KeyCode.C;
        public KeyCode startKey = KeyCode.G;
        public KeyCode killKey = KeyCode.K;

        void Update()
        {
            if (GameManager.Instance == null) return;

            if (Input.GetKeyDown(startKey)) {
                GameManager.Instance.ChangeGameState(GameManager.GameState.Playing);
                Debug.Log("Game Started!");
            }
            // Z - Spawn 1 con
            if (Input.GetKeyDown(spawnKey)) SpawnTest(1);
            
            // X - Spawn 10 con
            if (Input.GetKeyDown(spawnHordeKey)) SpawnTest(10);
            
            // C - Thêm tiền
            if (Input.GetKeyDown(moneyKey)) GameManager.Instance.EconomyManager.AddCurrency(0, 500);
            
            // K - Kill All
            if (Input.GetKeyDown(killKey)) {
                foreach(var z in FindObjectsByType<ZombieAI>(FindObjectsSortMode.None)) 
                    z.TakeDamage(9999f, Vector3.zero);
            }
        }

        void SpawnTest(int count)
        {
            for(int i=0; i<count; i++) {
                Vector2 r = Random.insideUnitCircle * 15f;
                GameManager.Instance.ZombieManager.SpawnZombie(transform.position + new Vector3(r.x, 0, r.y));
            }
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 250, 200));
            GUI.Box(new Rect(0,0,250,200), "TEST CONTROLS");
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            GUILayout.Label($"[G] Start Game");
            GUILayout.Label($"[Z] Spawn 1 Zombie");
            GUILayout.Label($"[X] Spawn 10 Zombies");
            GUILayout.Label($"[C] +$500 Money");
            GUILayout.Label($"[K] Kill All");
            if(GameManager.Instance && GameManager.Instance.ZombieManager)
                GUILayout.Label($"Active Zombies: {GameManager.Instance.ZombieManager.ActiveZombieCount}");
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}