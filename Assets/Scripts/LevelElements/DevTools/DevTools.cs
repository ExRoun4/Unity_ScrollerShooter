using UnityEngine;


[ExecuteAlways]
public class DevTools : MonoBehaviour
{
    const float finishPortalClampZOffset = 15.0f;

    public Transform enemySpawners;
    public PlayerVisualizer playerVisualizer;
    public LevelFinishGameLine finishGameLine;

    private void Awake()
    {
        if(!Application.IsPlaying(gameObject)) return;
        Destroy(gameObject);
    }

    private void Update()
    {
        if(Application.IsPlaying(gameObject)) return;

        ProduceClampingObjects();
    }

    private void ProduceClampingObjects(){
        Vector3 cameraXBorders = playerVisualizer.cameraForProportions.ViewportToWorldPoint(
            new Vector3(
                1, 
                0, 
                playerVisualizer.cameraForProportions.transform.localPosition.y
            )
        );
        
        enemySpawners.position = Vector3.zero;
        enemySpawners.rotation = Quaternion.identity;
        
        float maxEnemySpawnerZPos = 0.0f;
        foreach(Transform enemySpawner in enemySpawners.transform){
            enemySpawner.position = new Vector3(
                Mathf.Clamp(enemySpawner.position.x, -cameraXBorders.x, cameraXBorders.x), 
                0.0f, 
                Mathf.Max(enemySpawner.position.z, 0.0f)
            );

            if(enemySpawner.position.z > maxEnemySpawnerZPos){
                maxEnemySpawnerZPos = enemySpawner.position.z;
            }
        }

        finishGameLine.transform.position = new Vector3(0.0f, 0.0f, maxEnemySpawnerZPos + finishPortalClampZOffset);
    }
}
