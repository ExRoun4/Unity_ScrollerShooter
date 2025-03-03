using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerStats : EntityStats
{
    const float energyRegenerationPercent = 0.15f; // PER SEC
    const float timeBeforeRespawn = 5.0f;
    
    public float maxEnergy = 1.0f;

    private PlayerBase player;
    private float currentEnergy;


    #region INITIALIZATION

    protected override void Start()
    {
        player = (PlayerBase) ownerEntity;
    }

    public void EvaluateAttributesFromShip(GameDataSystem.PlayerShipAttributes attributes){
        maxHealth = attributes.maxHealth;
        maxEnergy = attributes.maxEnergy;

        currentHealth = GameDataSystem.instance.GetPlayerData().healthLeft;
        currentEnergy = maxEnergy;
    }

    #endregion
        

    #region BEHAVIOR

    private void Update()
    {
        ProduceEnergyCalculation();
    }

    private void ProduceEnergyCalculation(){
        currentEnergy += energyRegenerationPercent * Time.deltaTime;
        
        currentEnergy = Mathf.Clamp(currentEnergy, 0.0f, maxEnergy);
    }

    private async void ProducePlayerRespawn(){
        // TODO - DESTROY EFFECT ETC
        player.DeactivatePlayer(false);
        player.playerBody.gameObject.SetActive(false);
        player.GetPlayerWeapon().SetActive(false);

        await Task.Delay(TimeSpan.FromSeconds(timeBeforeRespawn));
        
        player.playerBody.gameObject.SetActive(true);
        player.ActivatePlayer();
        currentEnergy = maxEnergy;
        currentHealth = maxHealth;
        player.ActivateInvulnurability();

        await player.AnimateStartShowing();
    }

    protected override void Death()
    {
        GameDataSystem.instance.ReducePlayerLifes(1);
        if(GameDataSystem.instance.GetPlayerData().lifesLeft > 0){
            ProducePlayerRespawn();
            return;
        }

        // END GAME
        MainLevelManagement.instance.ProduceFinishGame(MainLevelManagement.GameEndReasons.LOSE);
    }



    #endregion


    #region SETTERS AND GETTERS

    public float GetCurrentEnergy(){
        return currentEnergy;
    }

    public void ReduceEnergy(float withValue){
        currentEnergy -= withValue;
    }

    #endregion
}
