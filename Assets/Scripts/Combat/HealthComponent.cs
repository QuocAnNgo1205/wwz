using UnityEngine;
using Unity.Netcode;
using System;

namespace CoopZombieShooter.Combat
{
    /// <summary>
    /// Server-authoritative health system for players and enemies.
    /// Handles damage, death, and revive mechanics for co-op gameplay.
    /// </summary>
    public class HealthComponent : NetworkBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private bool canBeRevived = true;
        [SerializeField] private float reviveTime = 3f;
        [SerializeField] private float downedHealthRestorePercent = 0.3f; // 30% health after revive

        [Header("Damage Settings")]
        [SerializeField] private bool invulnerableOnSpawn = true;
        [SerializeField] private float invulnerabilityDuration = 2f;

        [Header("UI References")]
        [SerializeField] private GameObject downedUIPanel;

        // Network variables (server-authoritative)
        private NetworkVariable<float> currentHealth = new NetworkVariable<float>(
            100f, 
            NetworkVariableReadPermission.Everyone, 
            NetworkVariableWritePermission.Server
        );

        private NetworkVariable<bool> isDowned = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

        private NetworkVariable<bool> isDead = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

        // Local state
        private float invulnerabilityTimer;
        private bool isInvulnerable;

        // Events
        public event Action<float, float> OnHealthChanged; // current, max
        public event Action<float, GameObject> OnDamaged; // damage amount, instigator
        public event Action OnDowned;
        public event Action OnRevived;
        public event Action OnDeath;

        #region Properties

        public float CurrentHealth => currentHealth.Value;
        public float MaxHealth => maxHealth;
        public float HealthPercentage => currentHealth.Value / maxHealth;
        public bool IsDowned => isDowned.Value;
        public bool IsDead => isDead.Value;
        public bool IsAlive => !isDead.Value;

        #endregion

        #region Unity Lifecycle

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // Initialize health on server
            if (IsServer)
            {
                currentHealth.Value = maxHealth;
                isDowned.Value = false;
                isDead.Value = false;

                if (invulnerableOnSpawn)
                {
                    isInvulnerable = true;
                    invulnerabilityTimer = invulnerabilityDuration;
                }
            }

            // Subscribe to network variable changes
            currentHealth.OnValueChanged += OnHealthValueChanged;
            isDowned.OnValueChanged += OnDownedValueChanged;
            isDead.OnValueChanged += OnDeadValueChanged;

            // Initialize UI
            UpdateHealthUI();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            // Unsubscribe from events
            currentHealth.OnValueChanged -= OnHealthValueChanged;
            isDowned.OnValueChanged -= OnDownedValueChanged;
            isDead.OnValueChanged -= OnDeadValueChanged;
        }

        private void Update()
        {
            if (!IsServer) return;

            // Handle invulnerability timer
            if (isInvulnerable)
            {
                invulnerabilityTimer -= Time.deltaTime;
                if (invulnerabilityTimer <= 0)
                {
                    isInvulnerable = false;
                }
            }
        }

        #endregion

        #region Damage System

        /// <summary>
        /// Apply damage to this entity. Server-authoritative.
        /// </summary>
        /// <param name="damage">Amount of damage to apply</param>
        /// <param name="instigator">GameObject that caused the damage</param>
        public void TakeDamage(float damage, GameObject instigator = null)
        {
            if (!IsServer)
            {
                Debug.LogWarning("TakeDamage called on client! Damage must be applied on server.");
                return;
            }

            // Can't damage if already dead
            if (isDead.Value) return;

            // Check invulnerability
            if (isInvulnerable)
            {
                Debug.Log($"{gameObject.name} is invulnerable!");
                return;
            }

            // Can't take damage while downed (must be revived or killed)
            if (isDowned.Value) return;

            // Apply damage
            currentHealth.Value = Mathf.Max(0, currentHealth.Value - damage);

            // Notify clients to play hit effects
            PlayHitEffectsClientRpc(damage);

            // Trigger damage event
            OnDamaged?.Invoke(damage, instigator);

            Debug.Log($"{gameObject.name} took {damage} damage. Health: {currentHealth.Value}/{maxHealth}");

            // Check for downed/death state
            if (currentHealth.Value <= 0)
            {
                if (canBeRevived)
                {
                    EnterDownedState();
                }
                else
                {
                    Die();
                }
            }
        }

        /// <summary>
        /// Heal this entity. Server-authoritative.
        /// </summary>
        public void Heal(float amount)
        {
            if (!IsServer)
            {
                Debug.LogWarning("Heal called on client! Healing must be applied on server.");
                return;
            }

            if (isDead.Value || isDowned.Value) return;

            currentHealth.Value = Mathf.Min(maxHealth, currentHealth.Value + amount);
            Debug.Log($"{gameObject.name} healed {amount}. Health: {currentHealth.Value}/{maxHealth}");
        }

        #endregion

        #region Downed/Death System

        private void EnterDownedState()
        {
            if (!IsServer) return;

            isDowned.Value = true;
            OnDowned?.Invoke();

            Debug.Log($"{gameObject.name} is downed and needs revival!");

            // Notify clients
            EnterDownedStateClientRpc();
        }

        private void Die()
        {
            if (!IsServer) return;

            isDead.Value = true;
            isDowned.Value = false;
            currentHealth.Value = 0;

            OnDeath?.Invoke();

            Debug.Log($"{gameObject.name} has died!");

            // Notify clients
            DieClientRpc();
        }

        /// <summary>
        /// Revive a downed player. Called by another player.
        /// </summary>
        /// <param name="reviver">The player performing the revive</param>
        public void Revive(GameObject reviver)
        {
            if (!IsServer)
            {
                // Client requests revive from server
                RequestReviveServerRpc(reviver.GetComponent<NetworkObject>().NetworkObjectId);
                return;
            }

            if (!isDowned.Value || isDead.Value)
            {
                Debug.LogWarning("Cannot revive: player is not downed or is dead.");
                return;
            }

            // Restore health
            currentHealth.Value = maxHealth * downedHealthRestorePercent;
            isDowned.Value = false;

            // Grant temporary invulnerability after revive
            isInvulnerable = true;
            invulnerabilityTimer = invulnerabilityDuration;

            OnRevived?.Invoke();

            Debug.Log($"{gameObject.name} was revived by {reviver.name}!");

            // Notify clients
            ReviveClientRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestReviveServerRpc(ulong reviverNetworkId)
        {
            // Validate reviver exists and is nearby (add distance check in production)
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(reviverNetworkId, out NetworkObject reviverNetObj))
            {
                Revive(reviverNetObj.gameObject);
            }
        }

        #endregion

        #region Network Callbacks

        private void OnHealthValueChanged(float oldValue, float newValue)
        {
            OnHealthChanged?.Invoke(newValue, maxHealth);
            UpdateHealthUI();
        }

        private void OnDownedValueChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                // Show downed UI
                if (downedUIPanel != null && IsOwner)
                {
                    downedUIPanel.SetActive(true);
                }
            }
            else
            {
                // Hide downed UI
                if (downedUIPanel != null && IsOwner)
                {
                    downedUIPanel.SetActive(false);
                }
            }
        }

        private void OnDeadValueChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                // Handle death (disable controls, play animation, etc.)
                HandleDeathState();
            }
        }

        #endregion

        #region ClientRpc Methods

        [ClientRpc]
        private void PlayHitEffectsClientRpc(float damage)
        {
            // Play hit particle effects, sounds, screen shake, etc.
            Debug.Log($"Playing hit effect for {damage} damage");
            
            // Example: Play hit sound
            // AudioManager.Instance.PlaySound("PlayerHit");
            
            // Example: Screen shake for owner
            // if (IsOwner)
            //     CameraShake.Instance.Shake(0.2f, 0.1f);
        }

        [ClientRpc]
        private void EnterDownedStateClientRpc()
        {
            // Play downed animation, effects
            Debug.Log($"{gameObject.name} entered downed state on all clients");
            
            // Example: Play downed animation
            // GetComponent<Animator>()?.SetTrigger("Downed");
        }

        [ClientRpc]
        private void DieClientRpc()
        {
            // Play death animation, effects
            Debug.Log($"{gameObject.name} died on all clients");
            
            // Example: Play death animation
            // GetComponent<Animator>()?.SetTrigger("Death");
        }

        [ClientRpc]
        private void ReviveClientRpc()
        {
            // Play revive effects
            Debug.Log($"{gameObject.name} was revived on all clients");
            
            // Example: Play revive particle effect
            // ParticleSystem reviveEffect = GetComponentInChildren<ParticleSystem>();
            // reviveEffect?.Play();
        }

        #endregion

        #region UI Updates

        private void UpdateHealthUI()
        {
            // Update health bar, text, etc.
            // This would typically call a UI manager or health bar component
            
            if (IsOwner)
            {
                // Update local player's health UI
                // UIManager.Instance.UpdateHealthBar(HealthPercentage);
            }
        }

        private void HandleDeathState()
        {
            // Disable player controls, show death screen, etc.
            if (IsOwner)
            {
                var playerController = GetComponent<Unity.Netcode.Components.NetworkTransform>();
                if (playerController != null)
                {
                    // Disable movement
                    // playerController.enabled = false;
                }

                // Show death/respawn UI
                Debug.Log("You died! Show respawn UI here.");
            }
        }

        #endregion

        #region Public Utility Methods

        /// <summary>
        /// Check if this entity can take damage.
        /// </summary>
        public bool CanTakeDamage()
        {
            return IsAlive && !isDowned.Value && !isInvulnerable;
        }

        /// <summary>
        /// Reset health to full (useful for respawning).
        /// </summary>
        public void ResetHealth()
        {
            if (!IsServer) return;

            currentHealth.Value = maxHealth;
            isDowned.Value = false;
            isDead.Value = false;
            isInvulnerable = false;
        }

        /// <summary>
        /// Instantly kill this entity (bypass downed state).
        /// </summary>
        public void InstantKill()
        {
            if (!IsServer) return;

            currentHealth.Value = 0;
            Die();
        }

        #endregion

        #region Debug

        [ContextMenu("Debug: Take 20 Damage")]
        private void DebugTakeDamage()
        {
            if (IsServer)
            {
                TakeDamage(20f);
            }
        }

        [ContextMenu("Debug: Heal 30")]
        private void DebugHeal()
        {
            if (IsServer)
            {
                Heal(30f);
            }
        }

        [ContextMenu("Debug: Enter Downed State")]
        private void DebugEnterDowned()
        {
            if (IsServer)
            {
                EnterDownedState();
            }
        }

        [ContextMenu("Debug: Revive")]
        private void DebugRevive()
        {
            if (IsServer)
            {
                Revive(gameObject);
            }
        }

        #endregion
    }
}