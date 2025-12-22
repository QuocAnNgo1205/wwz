using UnityEngine;
using System;
// CÁC DÒNG USING QUAN TRỌNG ĐỂ NỐI FILE
using ZombieCoopFPS.Enemy;    
using ZombieCoopFPS.Economy;  
using ZombieCoopFPS.Building; 
using ZombieCoopFPS.Missions; 

namespace ZombieCoopFPS.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public enum GameState { MainMenu, Playing, Paused, GameOver, Victory }
        public GameState CurrentState { get; private set; }
        public event Action<GameState> OnGameStateChanged;

        [Header("Manager References")]
        public ZombieManager ZombieManager; 
        public EconomyManager EconomyManager;
        public BuildingManager BuildingManager;
        public MissionManager MissionManager;
        
        [Header("Game Settings")]
        public int MaxPlayers = 4;
        public float GameDifficulty = 1f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeManagers();
        }

        private void InitializeManagers()
        {
            // Gọi hàm Initialize() của các manager con
            // Dấu ? để tránh lỗi nếu bạn chưa kéo script vào inspector
            ZombieManager?.Initialize();
            EconomyManager?.Initialize();
            BuildingManager?.Initialize();
            MissionManager?.Initialize();
        }

        public void ChangeGameState(GameState newState)
        {
            if (CurrentState == newState) return;
            
            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);
            HandleStateChange(newState);
        }

        private void HandleStateChange(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                    Time.timeScale = 1f;
                    ZombieManager?.StartSpawning(); 
                    MissionManager?.GenerateNewMission();
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    Time.timeScale = 0f;
                    ZombieManager?.StopSpawning(); 
                    break;
            }
        }

        public void RestartGame()
        {
            ZombieManager?.ResetSystem();
            EconomyManager?.ResetSystem();
            BuildingManager?.ResetSystem();
            MissionManager?.ResetSystem();
            
            ChangeGameState(GameState.Playing);
        }
    }
}