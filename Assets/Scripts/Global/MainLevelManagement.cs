using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLevelManagement : MonoBehaviour
{
    public enum GameEndReasons {
        WIN = 0, LOSE
    }

    public static MainLevelManagement instance;
    
    public GameObject enemiesParent;
    public GameObject projectilesParent;

    
    private void Start(){
        Assert.IsNull(instance, "[LevelManagement] There already 1 level management instance");

        instance = this;
    }


    #region BEHAVIOR

    public PlayerBase SpawnPlayer(Scene toScene){
        return (PlayerBase) PoolableObject.Spawn(
            GameRoot.instance.playerPrefab, 
            Vector3.zero, 
            toScene
        );
    }

    public void StartGame(){
        PlayerBase player = GameRoot.instance.GetActivePlayer();
        player.ActivatePlayer();

        player.InitWeapon(PlayerBase.WEAPONS_INDEXES.BASIC_WEAPON, 5); // TEST
    }

    public void EndGame(GameEndReasons reason){
        if(reason == GameEndReasons.WIN){
            print("[LevelManagement] Game won");
        }
    }

    #endregion


    #region EDITOR
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(Vector3.zero, new Vector3(1.0f, 1.0f, 2.0f));
        GizmoUtils.DrawArrow(Vector3.zero, Vector3.forward, 3.0f, 35.0f);
    }

    #endregion
}
