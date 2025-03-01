using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : CharacterEntityBase
{
    const float moveForwardSpeed = 4.0f;
    const float playerBodyMoveOnStartSpeed = 14.0f;
    const float playerBodyHidedZOffset = -15.0f;
    const float startInvulnurableTime = 5.0f;

    public Transform cameraMount;
    public PlayerController playerBody;
    public PlayerStats playerStats;

    private float bodyZPosOnStart;
    private bool isActive = false;
    private PlayerWeaponBase currentWeapon;


    #region INITIALIZATION AND ACTIVATION

    private void Awake()
    {
        bodyZPosOnStart = playerBody.transform.localPosition.z;
    }

    public async void ActivatePlayer(){
        isActive = true;
        ActivateInvulnurability();
        
        await AnimateStartShowing();

        playerBody.SetControlling(true);
        currentWeapon.SetActive(true);
    }

    public void DeactivatePlayer(bool destroyWeapon = true){
        isActive = false;
        playerBody.SetControlling(false);
        if(destroyWeapon) Destroy(currentWeapon);
    }

    public void InitWeapon(){
        GameDataSystem.PlayerWeaponData weapon = GameDataSystem.instance.GetPlayerData().GetCurrentWeapon();
        if(!weapon.GetWeaponHolder().prefab) return;

        currentWeapon = Instantiate(weapon.GetWeaponHolder().prefab, playerBody.transform);
        currentWeapon.Init(this, weapon.currentLevel);
    }

    public void InitShip(){
        GameDataSystem.PlayerShipHolder shipHolder = GameDataSystem.instance.GetPlayerData().GetShip();
        if(!shipHolder.modelPrefab) return;

        Instantiate(shipHolder.modelPrefab, playerBody.transform);
        playerStats.EvaluateAttributesFromShip(shipHolder.attributes);
    }

    #endregion

    #region PRIVATE BEHAVIOR

    private void Update()
    {
        if(isActive){
            transform.position += Vector3.forward * moveForwardSpeed * Time.deltaTime;
        }
    }

    #endregion


    #region ACTIONS

    public async Awaitable AnimateStartShowing(){
        playerBody.transform.localPosition = new Vector3(0.0f, 0.0f, playerBodyHidedZOffset);
        while(playerBody.transform.localPosition.z < bodyZPosOnStart){
            playerBody.transform.localPosition += new Vector3(0.0f, 0.0f, playerBodyMoveOnStartSpeed * Time.deltaTime);
            await Task.Yield();
        }
        playerBody.transform.localPosition = new Vector3(0.0f, 0.0f, bodyZPosOnStart);
        Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));
    }

    public async void ActivateInvulnurability(){
        playerStats.SetInvulnerable(true);
        await Task.Delay(TimeSpan.FromSeconds(startInvulnurableTime));
        playerStats.SetInvulnerable(false);
    }

    #endregion


    #region SETTERS AND GETTERS

    public PlayerWeaponBase GetPlayerWeapon(){
        return currentWeapon;
    }

    #endregion
}
