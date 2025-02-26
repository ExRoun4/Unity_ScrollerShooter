using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLevelManagement : MonoBehaviour
{
    public enum GAME_STATE {
        NOT_INITIALIZED = 0,
        STARTED,
        GAME_LOSED,
        GAME_WON,
        EXITING_GAME
    }

    public enum GameEndReasons {
        WIN = 0, 
        LOSE
    }

    public static MainLevelManagement instance;
    
    public GameObject enemiesParent;
    public GameObject projectilesParent;
    public LevelFinishGameLine finishGameLine;

    private GAME_STATE currentGameState = GAME_STATE.NOT_INITIALIZED;
    

    #region INITIALIZATION

    private void Awake(){
        ProduceAsserting();

        instance = this;
    }

    private void ProduceAsserting(){
        Assert.IsNull(instance, "[LevelManagement] There already 1 level management instance");
        Assert.IsNotNull(enemiesParent, "[LevelManagement] Missing enemiesParent object");
        Assert.IsNotNull(projectilesParent, "[LevelManagement] Missing projectilesParent object");
        Assert.IsNotNull(finishGameLine, "[LevelManagement] Missing finishGameLine");
    }

    #endregion 


    #region BEHAVIOR

    private async void ProduceWinningGame(){
        while(true){
            if(enemiesParent.transform.childCount > 0){
                await Task.Yield();
            }
            
            break;
        }
        
        CloseGame();
    }

    private void ProduceLosedGame(){
        CloseGame();
    }

    private void CloseGame(){
        currentGameState = GAME_STATE.EXITING_GAME;

        GameRoot.instance.GetActivePlayer().DeactivatePlayer();
        GameRoot.instance.ClearGameLevel();
    }

    #endregion


    #region ACTIONS

    public PlayerBase SpawnPlayer(Scene toScene){
        return (PlayerBase) PoolableObject.Spawn(
            GameRoot.instance.playerPrefab, 
            Vector3.zero, 
            toScene
        );
    }

    public void StartGame(){
        // INIT PLAYER
        PlayerBase player = GameRoot.instance.GetActivePlayer();
        player.ActivatePlayer();
        player.InitWeapon(PlayerBase.WEAPONS_INDEXES.BASIC_WEAPON, 5); // TEST

        currentGameState = GAME_STATE.STARTED;
    }

    public void ProduceFinishGame(GameEndReasons reason){
        if(currentGameState != GAME_STATE.STARTED) return;

        if(reason == GameEndReasons.WIN){
            currentGameState = GAME_STATE.GAME_WON;
            ProduceWinningGame();
        } else {
            currentGameState = GAME_STATE.GAME_LOSED;
            ProduceLosedGame();
        }
    }


    #endregion


    #region SETTERS AND GETTERS

    public GAME_STATE GetGameState(){
        return currentGameState;
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
