using UnityEngine;

public class PlayerStats : EntityStats
{
    const float energyRegenerationPercent = 0.15f; // PER SEC
    public float maxEnergy = 1.0f;

    private float currentEnergy;


    protected override void Start()
    {
        base.Start();
        
        currentEnergy = maxEnergy;
    }
        

    #region BEHAVIOR

    private void Update()
    {
        ProduceEnergyCalculation();
    }

    private void ProduceEnergyCalculation(){
        currentEnergy += energyRegenerationPercent * Time.deltaTime;
        
        currentEnergy = Mathf.Clamp(currentEnergy, 0.0f, maxEnergy);
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
