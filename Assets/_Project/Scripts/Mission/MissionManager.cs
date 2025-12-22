using UnityEngine;
using System;
using System.Collections.Generic;

// ĐÂY LÀ CÁI GAMEMANAGER ĐANG TÌM:
namespace ZombieCoopFPS.Missions 
{
    public class MissionManager : MonoBehaviour
    {
        public void Initialize()
        {
            Debug.Log("MissionManager: Initialized");
        }

        public void GenerateNewMission()
        {
            Debug.Log("MissionManager: Generating new mission...");
        }

        public void ResetSystem()
        {
            Debug.Log("MissionManager: Resetting system...");
        }
    }

    // Các class phụ trợ cho Mission (để code không báo lỗi thiếu)
    [System.Serializable]
    public class MissionData
    {
        public string Title;
        public int Reward;
    }
}