using UnityEngine;
using ZombieCoopFPS.Combat; // Cần cái này để dùng IDamageable

namespace ZombieCoopFPS.Building
{
    public interface IBuildable
    {
        void Initialize(int ownerId);
        string GetBuildableID();
        int GetBuildCost();
        bool CanPlaceAt(Vector3 position, Quaternion rotation);
        void OnPlaced();
        void OnDestroyed();
    }

    public interface IRepairable : IDamageable
    {
        int GetRepairCost();
        bool NeedsRepair();
        bool Repair(int playerId);
    }
}