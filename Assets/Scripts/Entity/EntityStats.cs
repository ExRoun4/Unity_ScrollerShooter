using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public float maxHealth = 1.0f;

    protected float currentHealth;


    protected virtual void Start(){
        currentHealth = maxHealth;
    }
    
    
    public void DamageEntity(float damage){
        if(damage <= 0.0f) return;

        currentHealth -= damage;
        if(currentHealth <= 0.0f){
            Death();
        }
    }

    public virtual void Death(){
        Destroy(gameObject);
    }
}
