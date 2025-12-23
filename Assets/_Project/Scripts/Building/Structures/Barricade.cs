using UnityEngine;
using ZombieCoopFPS.Combat;
using ZombieCoopFPS.Economy;
using ZombieCoopFPS.Core; // Để gọi GameManager

namespace ZombieCoopFPS.Building
{
    public class Barricade : MonoBehaviour, IBuildable, IRepairable, IInteractable
    {
        [SerializeField] private float maxHealth = 500f;
        [SerializeField] private int repairCostBase = 10;
        private float currentHealth;

        public void Initialize(int ownerId) 
        { 
            currentHealth = maxHealth; 
        }

        // --- IDamageable ---
        public void TakeDamage(float damage, Vector3 source)
        {
            currentHealth -= damage;
            if (currentHealth <= 0) OnDestroyed();
        }
        public void Heal(float amount) => currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        public float GetCurrentHealth() => currentHealth;
        public float GetMaxHealth() => maxHealth;
        public bool IsDead() => currentHealth <= 0;

        // --- IRepairable ---
        public int GetRepairCost() => 50; // Giả định
        public bool NeedsRepair() => currentHealth < maxHealth;
        public bool Repair(int playerId) 
        {
            // Logic trừ tiền và hồi máu
            Heal(maxHealth);
            return true;
        }

        // --- IInteractable ---
        public bool CanInteract(GameObject interactor) => NeedsRepair();
        public void Interact(GameObject interactor) { Repair(0); } // ID player tạm để 0
        public string GetInteractionPrompt() => "Repair Barricade";
        public InteractionType GetInteractionType() => InteractionType.Repair;
        public float GetInteractionRange() => 3f;

        // --- IBuildable ---
        public string GetBuildableID() => "Barricade_01";
        public int GetBuildCost() => 100;
        public bool CanPlaceAt(Vector3 pos, Quaternion rot) => true;
        public void OnPlaced() { }
        public void OnDestroyed() { Destroy(gameObject); }
        public void OnInteractorEnter(GameObject interactor) { }
        public void OnInteractorExit(GameObject interactor) { }
    }
}