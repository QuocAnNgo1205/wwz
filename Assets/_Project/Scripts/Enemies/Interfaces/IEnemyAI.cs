using UnityEngine;

namespace ZombieCoopFPS.Enemy
{
    // Đây là bản kế hoạch bắt buộc mọi Zombie phải tuân theo
    public interface IEnemyAI
    {
        void Initialize();
        void FindTarget();
        void MoveTo(Vector3 destination);
        void Attack();
        
        // Đây là hàm mà lỗi CS0535 đang báo thiếu:
        bool IsInAttackRange();
        
        // Các biến bắt buộc phải có
        Transform Target { get; }
        bool IsAlive { get; }
    }
}