using UnityEngine;
using ZombieCoopFPS.Data;

namespace ZombieCoopFPS.Combat
{
    public class WeaponBase : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] protected WeaponDataSO weaponData;
        [SerializeField] protected Transform muzzlePoint;
        [SerializeField] protected ParticleSystem muzzleFlash;
        [SerializeField] protected AudioSource audioSource;
        [SerializeField] protected AudioClip fireSound;
        [SerializeField] protected AudioClip reloadSound;

        // Runtime Stats
        protected int currentAmmo;
        protected int reserveAmmo;
        protected float nextFireTime;
        protected bool isReloading;
        protected float currentSpread;

        public WeaponDataSO WeaponData => weaponData;

        protected virtual void Start()
        {
            if (weaponData != null)
            {
                currentAmmo = weaponData.MagazineSize;
                reserveAmmo = weaponData.MaxAmmo;
            }
        }

        protected virtual void Update()
        {
            // Spread recovery
            if (currentSpread > 0)
                currentSpread = Mathf.Lerp(currentSpread, 0, Time.deltaTime * 5f);
        }

        public virtual bool CanFire()
        {
            return Time.time >= nextFireTime && currentAmmo > 0 && !isReloading;
        }

        public virtual void Fire()
        {
            if (!CanFire()) return;

            nextFireTime = Time.time + weaponData.FireRate;
            currentAmmo--;

            // Raycast Shoot
            Ray ray = GetFireRay();
            if (Physics.Raycast(ray, out RaycastHit hit, weaponData.Range))
            {
                ProcessHit(hit);
            }

            PlayMuzzleFlash();
            PlayFireSound();

            // Spread recoil
            currentSpread = Mathf.Min(currentSpread + weaponData.SpreadIncreasePerShot, weaponData.MaxSpread);
        }

        protected virtual void ProcessHit(RaycastHit hit)
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // Giảm đam theo khoảng cách (Damage Dropoff)
                float dist = Vector3.Distance(transform.position, hit.point);
                float dmg = weaponData.BaseDamage * (1f - Mathf.Clamp01(dist / weaponData.Range) * 0.5f);
                
                damageable.TakeDamage(dmg, hit.point);
                Debug.Log($"Hit {hit.collider.name} for {dmg:F1} damage");
            }

            // Hit Effect (Vẽ tạm hình cầu đỏ)
            Debug.DrawLine(muzzlePoint.position, hit.point, Color.red, 0.5f);
        }

        public virtual void Reload()
        {
            if (isReloading || currentAmmo >= weaponData.MagazineSize || reserveAmmo <= 0) return;

            StartCoroutine(ReloadRoutine());
        }

        private System.Collections.IEnumerator ReloadRoutine()
        {
            isReloading = true;
            Debug.Log("Reloading...");
            if(audioSource && reloadSound) audioSource.PlayOneShot(reloadSound);

            yield return new WaitForSeconds(weaponData.ReloadTime);

            int ammoNeeded = weaponData.MagazineSize - currentAmmo;
            int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);

            currentAmmo += ammoToLoad;
            reserveAmmo -= ammoToLoad;
            isReloading = false;
            Debug.Log("Reload Complete!");
        }

        protected Ray GetFireRay()
        {
            Vector3 origin;
            Vector3 direction;

            // CÁCH SỬA: Kiểm tra xem Camera.main có tồn tại không trước khi dùng
            if (Camera.main != null)
            {
                // Nếu có Camera, bắn từ Camera (chuẩn FPS)
                Transform cam = Camera.main.transform;
                origin = cam.position;
                direction = cam.forward;
            }
            else
            {
                // Nếu KHÔNG tìm thấy Camera (để tránh lỗi NullReference)
                // Bắn từ vị trí nòng súng (muzzlePoint)
                Debug.LogWarning("⚠️ Warning: MainCamera not found! Shooting from muzzle instead.");
                
                if (muzzlePoint != null)
                {
                    origin = muzzlePoint.position;
                    direction = muzzlePoint.forward;
                }
                else
                {
                    // Trường hợp tệ nhất: không có cả muzzlePoint thì lấy vị trí của chính súng
                    origin = transform.position;
                    direction = transform.forward;
                }
            }

            // Tính toán độ giật (Spread)
            if (currentSpread > 0)
            {
                direction += Random.insideUnitSphere * currentSpread * 0.1f;
            }

            return new Ray(origin, direction);
        }
        protected void PlayMuzzleFlash() { if (muzzleFlash) muzzleFlash.Play(); }
        protected void PlayFireSound() { if (audioSource && fireSound) audioSource.PlayOneShot(fireSound); }

        public int GetCurrentAmmo() => currentAmmo;
        public int GetReserveAmmo() => reserveAmmo;
    }
}