using UnityEngine;
using System.Collections.Generic;
using ZombieCoopFPS.Data;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace ZombieCoopFPS.Combat
{
    /// <summary>
    /// Player weapon manager - handles multiple weapons and switching
    /// </summary>
    public class PlayerWeaponSystem : MonoBehaviour
    {
        [Header("Weapons")]
        [SerializeField] private List<WeaponBase> startingWeapons = new List<WeaponBase>();
        [SerializeField] private int maxWeapons = 3;
        
        [Header("References")]
        [SerializeField] private Transform weaponHolder;
        [SerializeField] private Camera playerCamera;
        
        private List<WeaponBase> weapons = new List<WeaponBase>();
        private int currentWeaponIndex = 0;
        private WeaponBase currentWeapon;
        
        #if ENABLE_INPUT_SYSTEM
        private Keyboard keyboard;
        private Mouse mouse;
        #endif
        
        private void Start()
        {
            if (playerCamera == null)
                playerCamera = GetComponentInChildren<Camera>();
            
            if (weaponHolder == null)
            {
                weaponHolder = new GameObject("WeaponHolder").transform;
                weaponHolder.parent = playerCamera.transform;
                weaponHolder.localPosition = new Vector3(0.3f, -0.3f, 0.5f);
            }
            
            // Add starting weapons
            foreach (var weapon in startingWeapons)
            {
                if (weapon != null)
                {
                    AddWeapon(weapon);
                }
            }
            
            if (weapons.Count > 0)
            {
                SwitchToWeapon(0);
            }
            
            #if ENABLE_INPUT_SYSTEM
            keyboard = Keyboard.current;
            mouse = Mouse.current;
            #endif
        }
        
        private void Update()
        {
            HandleInput();
        }
        
        private void HandleInput()
        {
            if (currentWeapon == null) return;
            
            bool firePressed = false;
            bool reloadPressed = false;
            bool weapon1Pressed = false;
            bool weapon2Pressed = false;
            bool weapon3Pressed = false;
            float scrollDelta = 0f;
            
            #if ENABLE_INPUT_SYSTEM
            if (mouse != null)
            {
                firePressed = mouse.leftButton.isPressed;
                scrollDelta = mouse.scroll.ReadValue().y;
            }
            if (keyboard != null)
            {
                reloadPressed = keyboard.rKey.wasPressedThisFrame;
                weapon1Pressed = keyboard.digit1Key.wasPressedThisFrame;
                weapon2Pressed = keyboard.digit2Key.wasPressedThisFrame;
                weapon3Pressed = keyboard.digit3Key.wasPressedThisFrame;
            }
            #else
            firePressed = Input.GetMouseButton(0);
            reloadPressed = Input.GetKeyDown(KeyCode.R);
            weapon1Pressed = Input.GetKeyDown(KeyCode.Alpha1);
            weapon2Pressed = Input.GetKeyDown(KeyCode.Alpha2);
            weapon3Pressed = Input.GetKeyDown(KeyCode.Alpha3);
            scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            #endif
            
            // Fire weapon
            if (firePressed && currentWeapon.CanFire())
            {
                currentWeapon.Fire();
            }
            
            // Reload
            if (reloadPressed)
            {
                currentWeapon.Reload();
            }
            
            // Switch weapons
            if (weapon1Pressed && weapons.Count > 0)
                SwitchToWeapon(0);
            else if (weapon2Pressed && weapons.Count > 1)
                SwitchToWeapon(1);
            else if (weapon3Pressed && weapons.Count > 2)
                SwitchToWeapon(2);
            
            // Scroll wheel switching
            if (scrollDelta > 0)
                SwitchToNextWeapon();
            else if (scrollDelta < 0)
                SwitchToPreviousWeapon();
        }
        
        public void AddWeapon(WeaponBase weaponPrefab)
        {
            if (weapons.Count >= maxWeapons)
            {
                Debug.LogWarning("Max weapons reached!");
                return;
            }
            
            WeaponBase weapon = Instantiate(weaponPrefab, weaponHolder);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            weapon.gameObject.SetActive(false);
            
            weapons.Add(weapon);
            Debug.Log($"Added weapon: {weapon.WeaponData.DisplayName}");
        }
        
        public void SwitchToWeapon(int index)
        {
            if (index < 0 || index >= weapons.Count) return;
            
            // Hide current weapon
            if (currentWeapon != null)
            {
                currentWeapon.gameObject.SetActive(false);
            }
            
            // Show new weapon
            currentWeaponIndex = index;
            currentWeapon = weapons[currentWeaponIndex];
            currentWeapon.gameObject.SetActive(true);
            
            Debug.Log($"Switched to: {currentWeapon.WeaponData.DisplayName}");
        }
        
        public void SwitchToNextWeapon()
        {
            if (weapons.Count == 0) return;
            int nextIndex = (currentWeaponIndex + 1) % weapons.Count;
            SwitchToWeapon(nextIndex);
        }
        
        public void SwitchToPreviousWeapon()
        {
            if (weapons.Count == 0) return;
            int prevIndex = (currentWeaponIndex - 1 + weapons.Count) % weapons.Count;
            SwitchToWeapon(prevIndex);
        }
        
        public WeaponBase GetCurrentWeapon() => currentWeapon;
        
        private void OnGUI()
        {
            if (currentWeapon == null) return;
            
            // Weapon HUD
            GUILayout.BeginArea(new Rect(Screen.width - 220, Screen.height - 120, 200, 100));
            
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.fontSize = 14;
            style.fontStyle = FontStyle.Bold;
            
            GUILayout.Box(currentWeapon.WeaponData.DisplayName, style);
            GUILayout.Label($"Ammo: {currentWeapon.GetCurrentAmmo()} / {currentWeapon.GetReserveAmmo()}");
            
            // Show all weapons
            for (int i = 0; i < weapons.Count; i++)
            {
                string prefix = i == currentWeaponIndex ? "► " : "  ";
                GUILayout.Label($"{prefix}[{i + 1}] {weapons[i].WeaponData.DisplayName}");
            }
            
            GUILayout.EndArea();
        }
    }
    
    /// <summary>
    /// Shotgun weapon implementation
    /// </summary>
    public class ShotgunWeapon : WeaponBase
    {
        [Header("Shotgun Settings")]
        [SerializeField] private int pelletsPerShot = 8;
        [SerializeField] private float pelletSpread = 5f;
        
        public override void Fire()
        {
            if (!CanFire()) return;
            
            nextFireTime = Time.time + weaponData.FireRate;
            currentAmmo--;
            
            // Fire multiple pellets
            for (int i = 0; i < pelletsPerShot; i++)
            {
                FirePellet();
            }
            
            PlayMuzzleFlash();
            PlayFireSound();
            
            currentSpread = Mathf.Min(currentSpread + weaponData.SpreadIncreasePerShot, weaponData.MaxSpread);
        }
        
        private void FirePellet()
        {
            Ray ray = GetFireRay();
            
            // Add extra spread for shotgun
            Vector3 spread = Random.insideUnitSphere * pelletSpread;
            ray.direction = (ray.direction + spread).normalized;
            
            if (Physics.Raycast(ray, out RaycastHit hit, weaponData.Range))
            {
                ProcessHit(hit);
            }
        }
    }
    
    /// <summary>
    /// Automatic rifle implementation
    /// </summary>
    public class AutomaticRifle : WeaponBase
    {
        [Header("Auto Fire")]
        [SerializeField] private bool isFullAuto = true;
        
        private void Update()
        {
            base.Update();
            
            if (isFullAuto)
            {
                #if ENABLE_INPUT_SYSTEM
                if (Mouse.current != null && Mouse.current.leftButton.isPressed)
                {
                    Fire();
                }
                #else
                if (Input.GetMouseButton(0))
                {
                    Fire();
                }
                #endif
            }
        }
    }
    
    /// <summary>
    /// Explosive/Grenade launcher
    /// </summary>
    public class ExplosiveWeapon : WeaponBase
    {
        [Header("Explosive Settings")]
        [SerializeField] private float explosionRadius = 5f;
        [SerializeField] private float explosionForce = 500f;
        [SerializeField] private GameObject explosionEffectPrefab;
        
        protected override void ProcessHit(RaycastHit hit)
        {
            // Create explosion at impact
            CreateExplosion(hit.point);
        }
        
        private void CreateExplosion(Vector3 position)
        {
            // Spawn effect
            if (explosionEffectPrefab != null)
            {
                GameObject effect = Instantiate(explosionEffectPrefab, position, Quaternion.identity);
                Destroy(effect, 3f);
            }
            
            // Find all targets in radius
            Collider[] colliders = Physics.OverlapSphere(position, explosionRadius);
            foreach (var col in colliders)
            {
                IDamageable damageable = col.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    float distance = Vector3.Distance(position, col.transform.position);
                    float damageScale = 1f - (distance / explosionRadius);
                    float damage = weaponData.BaseDamage * damageScale;
                    
                    damageable.TakeDamage(damage, position);
                }
                
                // Apply physics force
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, position, explosionRadius);
                }
            }
            
            GameEvents.TriggerExplosion(position, explosionRadius);
        }
    }
}

namespace ZombieCoopFPS.Combat
{
    /// <summary>
    /// Weapon pickup system
    /// </summary>
    public class WeaponPickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private WeaponDataSO weaponData;
        [SerializeField] private WeaponBase weaponPrefab;
        [SerializeField] private bool destroyOnPickup = true;
        
        private void Start()
        {
            // Rotate for visual effect
            StartCoroutine(RotatePickup());
        }
        
        private System.Collections.IEnumerator RotatePickup()
        {
            while (true)
            {
                transform.Rotate(Vector3.up, 50f * Time.deltaTime);
                yield return null;
            }
        }
        
        public bool CanInteract(GameObject interactor)
        {
            return interactor.CompareTag("Player");
        }
        
        public void Interact(GameObject interactor)
        {
            PlayerWeaponSystem weaponSystem = interactor.GetComponent<PlayerWeaponSystem>();
            if (weaponSystem != null && weaponPrefab != null)
            {
                weaponSystem.AddWeapon(weaponPrefab);
                Debug.Log($"Picked up {weaponData.DisplayName}");
                
                if (destroyOnPickup)
                {
                    Destroy(gameObject);
                }
            }
        }
        
        public string GetInteractionPrompt()
        {
            return $"Press E to pickup {weaponData.DisplayName}";
        }
        
        public InteractionType GetInteractionType() => InteractionType.PickUp;
        public float GetInteractionRange() => 3f;
        public void OnInteractorEnter(GameObject interactor) { }
        public void OnInteractorExit(GameObject interactor) { }
    }
}