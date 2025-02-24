using UnityEngine;

public class ProjectileEntity : PoolableObject
{
    public float moveSpeed = 12.0f;
    public float lifeTime = 3.0f;
    public bool destroyOnDamage = true;

    private PoolableObject ownerEntity;
    private Vector3 direction;
    private float damage;
    private bool isActive = false;
    private float lifeTimeAccumulator;


    public void Init(PoolableObject _ownerEntity, Vector3 _direction, float _damage)
    {
        ownerEntity = _ownerEntity;
        direction = _direction;
        damage = _damage;

        isActive = true;
    }


    #region BEHAVIOR

    private void Update()
    {
        if(!isActive) return;
        if(lifeTime > 0.0f){
            lifeTimeAccumulator += Time.deltaTime;
            if(lifeTimeAccumulator >= lifeTime){
                Destroy();
                return;
            }
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(!isActive) return;
        if(collider.gameObject.GetComponent<CharacterEntityBase>()){
            if(collider == ownerEntity) return;
            if(collider.tag != "Untagged" && collider.tag == ownerEntity.tag) return;

            CharacterEntityBase entityToDamage = collider.GetComponent<CharacterEntityBase>();

            entityToDamage.entityStats.DamageEntity(damage);
            if(destroyOnDamage) Destroy();
        }
    }

    private void Destroy()
    {
        Despawn();
    }

    #endregion
}
