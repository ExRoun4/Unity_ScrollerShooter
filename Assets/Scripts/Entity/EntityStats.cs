using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public PoolableObject ownerEntity;
    public float maxHealth = 1.0f;

    protected float currentHealth;
    protected bool isInvulnerable = false;


    protected virtual void Start(){
        currentHealth = maxHealth;
    }

    protected virtual void Death(){
        ownerEntity.Despawn();
    }
    
    
    public void DamageEntity(float damage){
        if(isInvulnerable) return;
        if(damage <= 0.0f) return;

        currentHealth -= damage;
        if(currentHealth <= 0.0f){
            Death();
        }
    }

    public void DamageEntityByPercent(float percent){
        DamageEntity(currentHealth / maxHealth * percent);
    }

    #region SETTERS AND GETTERS

    public bool IsInvulnerable(){
        return isInvulnerable;
    }

    public void SetInvulnerable(bool value){
        isInvulnerable = value;
    }

    #endregion
}
