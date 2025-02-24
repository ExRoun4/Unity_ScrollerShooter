using UnityEngine;


[ExecuteAlways]
public class DevTools : MonoBehaviour
{
    public Transform enemySpawners;
    public PlayerVisualizer playerVisualizer;

    private void Awake()
    {
        if(!Application.IsPlaying(gameObject)) return;
        Destroy(gameObject);
    }

    private void Update()
    {
        if(Application.IsPlaying(gameObject)) return;

        enemySpawners.position = Vector3.zero;
        enemySpawners.rotation = Quaternion.identity;


        Vector3 cameraXBorders = playerVisualizer.cameraForProportions.ViewportToWorldPoint(
            new Vector3(
                1, 
                0, 
                playerVisualizer.cameraForProportions.transform.localPosition.y
            )
        );
        
        
        foreach(Transform enemySpawner in enemySpawners.transform){
            enemySpawner.position = new Vector3(
                Mathf.Clamp(enemySpawner.position.x, -cameraXBorders.x, cameraXBorders.x), 
                0.0f, 
                Mathf.Max(enemySpawner.position.z, 0.0f)
            );
        }
    }
}
