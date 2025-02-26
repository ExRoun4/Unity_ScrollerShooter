using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class EnemySpawner : MonoBehaviour
{
    private Vector2 cameraDistanceToSpawn = new Vector2(2.0f, 6.5f);

    public EnemyBase enemyToSpawn;
    public Transform playerDistanceFix;

    private bool enemySpawned = false;


    #region BEHAVIOR

    private void Start()
    {
        if(!Application.IsPlaying(gameObject)) return;
        if(!enemyToSpawn) print($"'{name}' EnemySpawner doesnt have accessed enemy prefab to spawn");
    }

    private void Update()
    {
        if(!Application.IsPlaying(gameObject)){
            ClampPlayerDistanceFix();
        } else {
            PlayModeLogic();
        }
    }

    private void ClampPlayerDistanceFix(){
        playerDistanceFix.localPosition = new Vector3(
            0.0f, 
            0.0f, 
            Mathf.Clamp(playerDistanceFix.localPosition.z, -cameraDistanceToSpawn.y, -cameraDistanceToSpawn.x)
        );
    }

    private void PlayModeLogic(){
        if(MainLevelManagement.instance.GetGameState() != MainLevelManagement.GameState.STARTED) return;
        if(enemySpawned) return;

        // SPAWN
        Vector3 cameraPoint = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.transform.position.y));
        
        if(cameraPoint.z >= playerDistanceFix.position.z){
            if(enemyToSpawn){
                EnemyBase spawnedEnemy = (EnemyBase) PoolableObject.Spawn(
                    enemyToSpawn, 
                    transform.position, 
                    MainLevelManagement.instance.enemiesParent.transform
                );
            }

            enemySpawned = true;
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 1.5f);
        GizmoUtils.DrawArrow(transform.position, -Vector3.forward, -playerDistanceFix.transform.localPosition.z);
        Gizmos.DrawWireCube(
            transform.position - Vector3.forward * -playerDistanceFix.transform.localPosition.z, 
            new Vector3(1.5f, 1.5f, 0.25f)
        );
    } 
}
