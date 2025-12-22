using UnityEngine;
using System.Collections.Generic;

namespace ZombieCoopFPS.Economy
{
    public class EconomyManager : MonoBehaviour
    {
        // Dictionary lưu tiền của từng người chơi (ID -> Số tiền)
        private Dictionary<int, int> playerCurrency = new Dictionary<int, int>();

        public void Initialize()
        {
            Debug.Log("EconomyManager: Initialized");
        }

        public void ResetSystem()
        {
            playerCurrency.Clear();
        }

        // HÀM MỚI: Đăng ký người chơi
        public void RegisterPlayer(int playerId)
        {
            if (!playerCurrency.ContainsKey(playerId))
            {
                playerCurrency.Add(playerId, 0); // Khởi điểm 0 đồng
            }
        }

        // HÀM MỚI: Lấy số tiền hiện tại
        public int GetCurrency(int playerId)
        {
            if (playerCurrency.ContainsKey(playerId))
                return playerCurrency[playerId];
            return 0;
        }

        // HÀM MỚI: Thêm tiền (GameTester gọi cái này)
        public void AddCurrency(int playerId, int amount)
        {
            if (!playerCurrency.ContainsKey(playerId)) RegisterPlayer(playerId);
            playerCurrency[playerId] += amount;
        }

        public bool CanAfford(int playerId, int amount) 
        {
            return GetCurrency(playerId) >= amount;
        }

        public bool TryPurchase(int playerId, int amount)
        {
            if (CanAfford(playerId, amount))
            {
                playerCurrency[playerId] -= amount;
                return true;
            }
            return false;
        }
    }
}