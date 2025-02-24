using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public int poolAmount = 20;
    public bool additionalPoolOnAbsence = true;
    public PoolableObject objectToPool;

    private List<PoolableObject> availableInstances = new List<PoolableObject>();
    private List<PoolableObject> usedInstances = new List<PoolableObject>();
    private bool isPooling;

    #region POOLING

    public async Awaitable StartPool(int toPoolAmount = 0){
        if(toPoolAmount == 0) toPoolAmount = poolAmount;

        isPooling = true;
        for(int i = 0; i < toPoolAmount; i++){
            AsyncInstantiateOperation<PoolableObject> newInstance = InstantiateAsync(objectToPool);

            while(!newInstance.isDone) await Task.Yield();

            PoolableObject createdInstance = newInstance.Result[0];
            createdInstance.gameObject.SetActive(false);
            createdInstance.transform.parent = this.transform;
            createdInstance.SetObjectPool(this);

            availableInstances.Add(createdInstance);
        }

        isPooling = false;
    }

    public PoolableObject ForceSpawnAndPool(){
        PoolableObject createdInstance = Instantiate(objectToPool);

        createdInstance.gameObject.SetActive(false);
        createdInstance.transform.parent = this.transform;
        createdInstance.SetObjectPool(this);

        if(!isPooling && additionalPoolOnAbsence) StartCoroutine(StartPool(poolAmount - 1));

        return createdInstance;
    }

    #endregion


    #region POOL OBJECT ACTIONS

    public PoolableObject GetAvailableInstance(){
        PoolableObject result;
        if(availableInstances.Count > 0){
            result = availableInstances[0];
            availableInstances.Remove(result);
            usedInstances.Add(result);

            if(additionalPoolOnAbsence && availableInstances.Count == 0 && !isPooling){
                StartCoroutine(StartPool());
            }

            return result;
        }

        return ForceSpawnAndPool();
    }

    public void DespawnInstance(PoolableObject instance){
        if(!usedInstances.Contains(instance)){
            Destroy(instance);
            return;
        }

        instance.gameObject.SetActive(false);
        instance.transform.parent = this.transform;
        instance.transform.position = Vector3.zero;
        instance.transform.rotation = Quaternion.identity;

        availableInstances.Add(instance);
        usedInstances.Remove(instance);
    }

    #endregion


    #region SETTERS AND GETTERS

    public bool GetPoolObject(){
        return objectToPool;
    }

    #endregion
}
