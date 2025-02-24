using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : CharacterEntityBase
{
    [Serializable]
    public class WeaponHolder {
        public WEAPONS_INDEXES index;
        public PlayerWeaponBase prefab;
    }

    public enum WEAPONS_INDEXES {
        BASIC_WEAPON = 0
    }

    const float moveForwardSpeed = 4.0f;
    const float playerBodyZPosOnStart = -15.0f;
    const float playerBodyMoveOnStartSpeed = 14.0f;

    public Transform cameraMount;
    public PlayerController playerBody;
    public PlayerStats playerStats;
    public WeaponHolder[] playerWeapons;

    private bool isActive = false;
    private PlayerWeaponBase currentWeapon;


    #region INITIALIZATION

    public async void ActivatePlayer(){
        isActive = true;
        
        // PLAYER START ANIMATION
        float playerBodyZPos = playerBody.transform.localPosition.z;
        playerBody.transform.localPosition += new Vector3(0.0f, 0.0f, playerBodyZPosOnStart);
        while(playerBody.transform.localPosition.z < playerBodyZPos){
            playerBody.transform.localPosition += new Vector3(0.0f, 0.0f, playerBodyMoveOnStartSpeed * Time.deltaTime);
            await Task.Yield();
        }
        playerBody.transform.localPosition = new Vector3(0.0f, 0.0f, playerBodyZPos);
        Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));

        playerBody.SetControlling(true);
        currentWeapon.SetActive(true);
    }

    public void InitWeapon(WEAPONS_INDEXES weaponIndex, int weaponLevel){
        WeaponHolder holder = GetWeaponHolderByIndex(weaponIndex);
        if(holder == null || !holder.prefab) return;

        currentWeapon = Instantiate(holder.prefab, playerBody.transform);
        currentWeapon.Init(this, weaponLevel);

    }

    #endregion


    private void Update()
    {
        if(isActive){
            transform.position += Vector3.forward * moveForwardSpeed * Time.deltaTime;
        }
    }

    private WeaponHolder GetWeaponHolderByIndex(WEAPONS_INDEXES index){
        foreach(WeaponHolder holder in playerWeapons){
            if(holder.index == index) return holder;
        }

        return null;
    }
}
