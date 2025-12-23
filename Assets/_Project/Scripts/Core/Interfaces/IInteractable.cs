using UnityEngine;

namespace ZombieCoopFPS
{
    // Cập nhật thêm Repair và Build vào danh sách
    public enum InteractionType 
    { 
        Use, 
        PickUp, 
        Open, 
        Talk, 
        Repair, // <--- Đã thêm cái này để sửa lỗi Barricade
        Build   // Thêm luôn cái này phòng hờ cho hệ thống xây dựng
    }

    public interface IInteractable
    {
        bool CanInteract(GameObject interactor);
        void Interact(GameObject interactor);
        string GetInteractionPrompt();
        InteractionType GetInteractionType();
        float GetInteractionRange();
        
        // Các hàm sự kiện khi nhìn vào/nhìn ra (cho UI)
        void OnInteractorEnter(GameObject interactor);
        void OnInteractorExit(GameObject interactor);
    }
}