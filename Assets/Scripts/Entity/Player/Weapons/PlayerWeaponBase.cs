using UnityEngine;

public class PlayerWeaponBase : MonoBehaviour
{
    public float energyConsuptionPerUse;
    public float useCountdown;
    public float damage;
    public ProjectileEntity projectilePrefab;
    
    protected PlayerBase ownerPlayer;

    protected int weaponLevel = 1;
    protected bool isActive = false;
    protected float useAccumulator;


    public void Init(PlayerBase _ownerPlayer, int _weaponLevel)
    {
        ownerPlayer = _ownerPlayer;
        weaponLevel = _weaponLevel;
    }

    #region BEHAVIOR

    private void Update()
    {
        if(isActive){
            useAccumulator += Time.deltaTime;

            if(Input.GetButton("Fire1")) TryUseWeapon();
        }
    }

    public void TryUseWeapon()
    {
        if(!isActive) return;
        if(useAccumulator < useCountdown) return;
        if(ownerPlayer.playerStats.GetCurrentEnergy() - energyConsuptionPerUse < energyConsuptionPerUse) return;

        useAccumulator = 0.0f;
        ownerPlayer.playerStats.ReduceEnergy(energyConsuptionPerUse);

        OnWeaponUse();
    }

    #endregion


    #region VIRTUAL

    protected virtual void OnWeaponUse() {}

    #endregion


    #region SETTERS AND GETTERS

    public void SetActive(bool value){
        isActive = value;
    }

    #endregion
}
