using UnityEngine;

public class EnemyStats : EntityStats
{
    public float contactPercentDamage = 0.25f;
    public bool destroyOnContactWithPlayer = true;


    private void OnTriggerEnter(Collider collider)
    {
        // PRODUCE CONTACT DAMAGE FOR PLAYER
        if(collider.tag == "Player"){
            if(GameRoot.instance.GetActivePlayer().playerStats.IsInvulnerable()) return;
            GameRoot.instance.GetActivePlayer().playerStats.DamageEntityByPercent(contactPercentDamage);

            if(destroyOnContactWithPlayer) Death();
        }
    }

    protected override void Death()
    {
        base.Death();
    }
}
