using UnityEngine;
using UnityEditor;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using NUnit.Framework;


public class PoolableObject : MonoBehaviour
{
    [HideInInspector]
    public int poolAmount = 20;

    private ObjectPool objectPool;


    #region SPAWNING / DESPAWNING

    public static PoolableObject Spawn(PoolableObject predicateObject, Vector3 spawnPosition){
        ObjectPool pool = ObjectPoolsManager.instance.FindPoolByObject(predicateObject);
        PoolableObject result = null;

        result = pool == null ? Instantiate(predicateObject) : pool.GetAvailableInstance();
        
        result.transform.position = spawnPosition;
        result.gameObject.SetActive(true);
        
        result.OnSpawned();
        return result;
    }

    public static PoolableObject Spawn(PoolableObject predicateObject, Vector3 spawn_position, Scene parentScene){
        PoolableObject result = PoolableObject.Spawn(predicateObject, spawn_position);
        result.transform.parent = null;
        SceneManager.MoveGameObjectToScene(result.gameObject, parentScene);

        return result;
    }

    public static PoolableObject Spawn(PoolableObject predicateObject, Vector3 spawn_position, Transform parent){
        PoolableObject result = PoolableObject.Spawn(predicateObject, spawn_position);
        result.transform.parent = parent;

        return result;
    }

    public void Despawn(){
        PreDespawned();
        if(!objectPool){
            Destroy(gameObject);
            return;
        }
        
        objectPool.DespawnInstance(this);
    }


    
    #endregion


    #region VIRTUAL METHODS

    protected virtual void OnSpawned(){}
    protected virtual void PreDespawned(){}

    #endregion


    #region SETTERS AND GETTERS

    public ObjectPool GetObjectPool(){
        return objectPool;
    }

    public void SetObjectPool(ObjectPool pool){
        objectPool = pool;
    }

    #endregion
}


