using UnityEngine;
using UnityEditor;

[ExecuteAlways]
[RequireComponent(typeof(BoxCollider))]
public class EnemySpawner : MonoBehaviour
{
    private Vector2 playerDistanceToSpawn = new Vector2(7.5f, 15.0f);

    public EnemyBase enemyToSpawn;
    public Transform playerDistanceFix;

    private bool enemySpawned = false;

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
            Mathf.Clamp(playerDistanceFix.localPosition.z, -playerDistanceToSpawn.y, -playerDistanceToSpawn.x)
        );
    }

    private void PlayModeLogic(){
        if(enemySpawned) return;

        // SPAWN
        PlayerController playerBody = GameRoot.instance.GetActivePlayer().playerBody;
        if(playerBody.transform.position.z >= playerDistanceFix.position.z){
            EnemyBase spawnedEnemy = (EnemyBase) PoolableObject.Spawn(
                enemyToSpawn, 
                transform.position, 
                MainLevelManagement.instance.enemiesParent.transform
            );

            enemySpawned = true;
        }
    }

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
