using UnityEngine;
using System.Collections.Generic;
using ZombieCoopFPS.Enemy;  
using ZombieCoopFPS.Combat;

namespace ZombieCoopFPS.Data
{
    /// <summary>
    /// Game configuration - Central settings for the entire game
    /// Create: Assets/Create/Game/Game Configuration
    /// </summary>
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Game Configuration")]
    public class GameConfiguration : ScriptableObject
    {
        [Header("Game Settings")]
        public int MaxPlayers = 4;
        public float BaseDifficulty = 1.0f;
        public float DifficultyScaling = 0.1f; // Per wave
        
        [Header("Economy")]
        public int StartingCurrency = 500;
        public int StandardZombieReward = 10;
        public int SpecialZombieRewardMultiplier = 5;
        
        [Header("Zombie Spawning")]
        public int InitialPoolSize = 50;
        public int MaxPoolSize = 200;
        public int MaxActiveZombies = 150;
        public float BaseSpawnRate = 2f;
        
        [Header("Performance")]
        public int MaxZombieUpdatesPerFrame = 50;
        public float ZombieCullingDistance = 100f;
        public float FullDetailDistance = 30f;
        public float SimplifiedDetailDistance = 60f;
        
        [Header("Building")]
        public float MaxBuildDistance = 5f;
        
        [Header("Mission")]
        public int MaxActiveMissions = 3;
        public float MissionGenerationInterval = 120f;
    }
    
    /// <summary>
    /// Extended zombie data with all properties
    /// </summary>
    [CreateAssetMenu(fileName = "New Zombie", menuName = "Game/Zombie Data")]
    public class ZombieDataSO : ScriptableObject
    {
        [Header("Identity")]
        public ZombieType Type;
        public string DisplayName;
        [TextArea(3, 5)]
        public string Description;
        
        [Header("Stats")]
        public float MaxHealth = 100f;
        public float MoveSpeed = 3f;
        public float AttackDamage = 10f;
        public float AttackRange = 2f;
        public float AttackCooldown = 1.5f;
        public float DetectionRange = 20f;
        
        [Header("Special Abilities")]
        public bool HasSpecialAbility = false;
        public float AbilityCooldown = 10f;
        public float AbilityRange = 5f;
        public float AbilityDamage = 50f;
        
        [Header("Rewards")]
        public int KillReward = 10;
        public float ExperienceReward = 5f;
        
        [Header("Visuals")]
        public GameObject Prefab;
        public Material[] MaterialVariants;
        public RuntimeAnimatorController AnimatorController;
        
        [Header("Audio")]
        public AudioClip[] IdleSounds;
        public AudioClip[] AttackSounds;
        public AudioClip[] HurtSounds;
        public AudioClip[] DeathSounds;
        
        [Header("VFX")]
        public GameObject DeathEffect;
        public GameObject BloodEffect;
        public GameObject SpawnEffect;
    }
    
    /// <summary>
    /// Weapon configuration data
    /// </summary>
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Game/Weapon Data")]
    public class WeaponDataSO : ScriptableObject
    {
        [Header("Identity")]
        public string WeaponID;
        public string DisplayName;
        [TextArea(2, 4)]
        public string Description;
        public Sprite Icon;
        
        [Header("Weapon Type")]
        public WeaponType Type;
        public FireMode FireMode = FireMode.SemiAuto;
        
        [Header("Damage")]
        public float BaseDamage = 25f;
        public float HeadshotMultiplier = 2f;
        public float Range = 100f;
        public DamageType DamageType = DamageType.Physical;
        
        [Header("Fire Rate")]
        public float FireRate = 0.1f; // Time between shots
        public int BurstCount = 3; // For burst fire
        public float BurstDelay = 0.05f;
        
        [Header("Ammo")]
        public int MagazineSize = 30;
        public int MaxAmmo = 300;
        public float ReloadTime = 2f;
        
        [Header("Accuracy")]
        public float BaseSpread = 0.5f;
        public float MaxSpread = 5f;
        public float SpreadIncreasePerShot = 0.1f;
        public float SpreadRecoveryRate = 2f;
        
        [Header("Economy")]
        public int PurchaseCost = 500;
        public int AmmoCost = 50; // Per magazine
        
        [Header("Visuals")]
        public GameObject Prefab;
        public GameObject MuzzleFlash;
        public GameObject ImpactEffect;
        public GameObject TracerPrefab;
        
        [Header("Audio")]
        public AudioClip FireSound;
        public AudioClip ReloadSound;
        public AudioClip EmptySound;
    }
    
    public enum FireMode
    {
        SemiAuto,
        FullAuto,
        Burst
    }
    
    /// <summary>
    /// Building/Structure data
    /// </summary>
    [CreateAssetMenu(fileName = "New Building", menuName = "Game/Building Data")]
    public class BuildingDataSO : ScriptableObject
    {
        [Header("Identity")]
        public string StructureID;
        public string DisplayName;
        [TextArea(2, 4)]
        public string Description;
        public Sprite Icon;
        
        [Header("Building")]
        public GameObject Prefab;
        public int BuildCost = 100;
        public float BuildTime = 2f;
        public StructureCategory Category;
        
        [Header("Stats")]
        public float MaxHealth = 500f;
        public bool CanBeRepaired = true;
        public float RepairCostPercentage = 0.5f; // 50% of build cost
        
        [Header("Upgrades")]
        public bool CanBeUpgraded = false;
        public int MaxLevel = 3;
        public int[] UpgradeCosts = new int[] { 200, 400, 800 };
        public float[] HealthMultipliers = new float[] { 1f, 1.5f, 2f, 3f };
        
        [Header("Placement")]
        public Vector3 PlacementSize = Vector3.one;
        public float SnapToGrid = 0f; // 0 = no snap, >0 = grid size
        public bool RequiresGroundSurface = true;
        public float MaxPlacementAngle = 45f; // Max slope angle
        
        [Header("Visuals")]
        public Material ValidPlacementMaterial;
        public Material InvalidPlacementMaterial;
        public GameObject DestructionEffect;
    }
    
    public enum StructureCategory
    {
        Defensive,  // Barricades, Walls
        Offensive,  // Turrets, Traps
        Utility     // Ammo boxes, Supply stations
    }
    
    /// <summary>
    /// Mission template data
    /// </summary>
    [CreateAssetMenu(fileName = "New Mission", menuName = "Game/Mission Data")]
    public class MissionDataSO : ScriptableObject
    {
        [Header("Identity")]
        public string MissionID;
        public string Title;
        [TextArea(3, 5)]
        public string Description;
        
        [Header("Objectives")]
        public MissionType Type;
        public int TargetCount; // Kill X zombies, collect X items, etc.
        public string[] ObjectiveTags; // Tags for objectives
        
        [Header("Rewards")]
        public int CurrencyReward = 500;
        public int ExperienceReward = 100;
        public List<string> ItemRewards; // Item IDs
        
        [Header("Time")]
        public bool HasTimeLimit = false;
        public float TimeLimit = 300f; // 5 minutes
        
        [Header("Difficulty")]
        public int RequiredPlayerLevel = 1;
        public MissionDifficulty Difficulty = MissionDifficulty.Normal;
        
        [Header("Spawn")]
        public bool SpawnsZombies = false;
        public ZombieType[] ZombieTypesToSpawn;
        public int ZombieCount = 10;
    }
    
    public enum MissionType
    {
        KillZombies,
        DefendPosition,
        CollectItems,
        Survive,
        Escort,
        Repair
    }
    
    public enum MissionDifficulty
    {
        Easy,
        Normal,
        Hard,
        Extreme
    }
}

namespace ZombieCoopFPS.Combat
{
    using ZombieCoopFPS.Data;
    
    /// <summary>
    /// Weapon base class - Implements IWeapon interface
    /// Use this as base for all weapon types
    /// </summary>
    public class WeaponBase : MonoBehaviour, IWeapon
    {
        [Header("Configuration")]
        [SerializeField] protected WeaponDataSO weaponData;
        
        [Header("Components")]
        [SerializeField] protected Transform muzzlePoint;
        [SerializeField] protected Animator animator;
        [SerializeField] protected AudioSource audioSource;
        
        protected int currentAmmo;
        protected int reserveAmmo;
        protected float nextFireTime;
        protected float currentSpread;
        protected bool isReloading;
        
        public WeaponDataSO WeaponData => weaponData;
        public bool IsReloading => isReloading;
        
        protected virtual void Awake()
        {
            if (weaponData != null)
            {
                currentAmmo = weaponData.MagazineSize;
                reserveAmmo = weaponData.MaxAmmo;
            }
        }
        
        public virtual void Fire()
        {
            if (!CanFire()) return;
            
            nextFireTime = Time.time + weaponData.FireRate;
            currentAmmo--;
            
            // Raycast hit detection
            Ray ray = GetFireRay();
            if (Physics.Raycast(ray, out RaycastHit hit, weaponData.Range))
            {
                ProcessHit(hit);
            }
            
            // Visual effects
            PlayMuzzleFlash();
            PlayFireSound();
            
            // Increase spread
            currentSpread = Mathf.Min(currentSpread + weaponData.SpreadIncreasePerShot, 
                                     weaponData.MaxSpread);
        }
        
        protected virtual Ray GetFireRay()
        {
            Vector3 direction = muzzlePoint.forward;
            
            // Apply spread
            direction += Random.insideUnitSphere * currentSpread;
            direction.Normalize();
            
            return new Ray(muzzlePoint.position, direction);
        }
        
        protected virtual void ProcessHit(RaycastHit hit)
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                float damage = weaponData.BaseDamage;
                
                // Check for headshot
                if (hit.collider.CompareTag("Head"))
                {
                    damage *= weaponData.HeadshotMultiplier;
                }
                
                DamageInfo damageInfo = new DamageInfo(
                    damage, 
                    hit.point, 
                    weaponData.DamageType,
                    gameObject
                );
                
                damageable.TakeDamage(damageInfo);
            }
            
            // Spawn impact effect
            if (weaponData.ImpactEffect != null)
            {
                Instantiate(weaponData.ImpactEffect, hit.point, 
                           Quaternion.LookRotation(hit.normal));
            }
        }
        
        public virtual void Reload()
        {
            if (isReloading || currentAmmo == weaponData.MagazineSize || reserveAmmo <= 0)
                return;
            
            StartCoroutine(ReloadCoroutine());
        }
        
        protected virtual System.Collections.IEnumerator ReloadCoroutine()
        {
            isReloading = true;
            
            if (audioSource && weaponData.ReloadSound)
            {
                audioSource.PlayOneShot(weaponData.ReloadSound);
            }
            
            if (animator)
            {
                animator.SetTrigger("Reload");
            }
            
            yield return new WaitForSeconds(weaponData.ReloadTime);
            
            int ammoNeeded = weaponData.MagazineSize - currentAmmo;
            int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);
            
            currentAmmo += ammoToReload;
            reserveAmmo -= ammoToReload;
            
            isReloading = false;
        }
        
        public virtual bool CanFire()
        {
            return currentAmmo > 0 && 
                   Time.time >= nextFireTime && 
                   !isReloading;
        }
        
        protected virtual void Update()
        {
            // Recover spread over time
            currentSpread = Mathf.Max(0, currentSpread - weaponData.SpreadRecoveryRate * Time.deltaTime);
        }
        
        public int GetCurrentAmmo() => currentAmmo;
        public int GetReserveAmmo() => reserveAmmo;
        public WeaponType GetWeaponType() => weaponData.Type;
        
        public void AddAmmo(int amount)
        {
            reserveAmmo = Mathf.Min(reserveAmmo + amount, weaponData.MaxAmmo);
        }
        
        protected virtual void PlayMuzzleFlash()
        {
            if (weaponData.MuzzleFlash != null && muzzlePoint != null)
            {
                GameObject flash = Instantiate(weaponData.MuzzleFlash, 
                                              muzzlePoint.position, 
                                              muzzlePoint.rotation);
                Destroy(flash, 0.1f);
            }
        }
        
        protected virtual void PlayFireSound()
        {
            if (audioSource && weaponData.FireSound)
            {
                audioSource.PlayOneShot(weaponData.FireSound);
            }
        }
    }
}

