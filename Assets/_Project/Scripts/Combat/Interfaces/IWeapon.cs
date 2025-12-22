namespace ZombieCoopFPS.Combat
{
    public interface IWeapon
    {
        void Fire();
        void Reload();
        bool CanFire();
        int GetCurrentAmmo();
        int GetReserveAmmo();
        WeaponType GetWeaponType();
    }

    public enum WeaponType { Pistol, Rifle, Shotgun, SMG, Sniper, Explosive }
}