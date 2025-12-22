using UnityEngine;

namespace ZombieCoopFPS.Economy
{
    public interface IInteractable
    {
        bool CanInteract(GameObject interactor);
        void Interact(GameObject interactor);
        string GetInteractionPrompt();
        InteractionType GetInteractionType();
        float GetInteractionRange();
    }

    public enum InteractionType { Purchase, PickUp, Activate, Build, Upgrade, Repair, Open }
    
    // Interface cho đồ mua bán
    public interface IPurchasable
    {
        int GetCost();
        string GetDisplayName();
        Sprite GetIcon();
        bool CanPurchase(int playerId);
        bool Purchase(int playerId);
    }
}