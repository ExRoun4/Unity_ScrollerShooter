using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLevelManagement : MonoBehaviour
{
    public enum GameState {
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

    private GameState currentGameState = GameState.NOT_INITIALIZED;
    

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
        currentGameState = GameState.EXITING_GAME;

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
        player.InitShip();
        player.InitWeapon();

        currentGameState = GameState.STARTED;
    }

    public void ProduceFinishGame(GameEndReasons reason){
        if(currentGameState != GameState.STARTED) return;

        if(reason == GameEndReasons.WIN){
            currentGameState = GameState.GAME_WON;
            ProduceWinningGame();
        } else {
            currentGameState = GameState.GAME_LOSED;
            ProduceLosedGame();
        }
    }


    #endregion


    #region SETTERS AND GETTERS

    public GameState GetGameState(){
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
