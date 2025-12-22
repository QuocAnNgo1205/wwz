using UnityEngine;
using System.Collections.Generic;

// ĐÂY LÀ CÁI GAMEMANAGER ĐANG TÌM:
namespace ZombieCoopFPS.Building
{
    public class BuildingManager : MonoBehaviour
    {
        public void Initialize()
        {
            Debug.Log("BuildingManager: Initialized");
        }

        public void ResetSystem()
        {
            Debug.Log("BuildingManager: Resetting system...");
        }
    }
    
    // Class dữ liệu Building
    [System.Serializable]
    public class BuildingData
    {
        public string StructureID;
        public int Cost;
        public GameObject Prefab;
    }
}