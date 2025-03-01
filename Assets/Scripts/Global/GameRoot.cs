using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    public static GameRoot instance;

    public ObjectPool objectPoolPrefab;
    public PlayerBase playerPrefab;
    public GameCamera gameCamera;
    public ObjectPoolsManager objectsPoolManager;
    public MainMenu mainMenu;

    private PlayerBase activePlayer;
    private Scene activeLevelScene;


    private void Awake(){
        instance = this;

        Assert.IsNotNull(playerPrefab, "Missing playerPrefab");
    }


    public async void InitGameOnLevel(Scene level){
        // POOLING
        DebugLog("Started pooling objects");
        await objectsPoolManager.StartPooling();
        DebugLog($"All objects pooled ({objectsPoolManager.objectToPoolDict.Count})");

        PlayerBase player = MainLevelManagement.instance.SpawnPlayer(level);
        
        SetActivePlayer(player);
        SetActiveLevel(level);

        gameCamera.ReparentToObject(player.cameraMount, true);

        MainLevelManagement.instance.StartGame();
    }

    public async void ClearGameLevel(){
        // UNLOAD POOLS
        DebugLog("Started clearing level");

        objectsPoolManager.StartClearPools();
        DebugLog("All pools cleared");

        // CLEAR LEVEL
        DebugLog("Started unload game level");
        gameCamera.ReparentToRootScene();
        activePlayer.Despawn();
        await SceneManager.UnloadSceneAsync(activeLevelScene);

        SetActiveLevel(default(Scene));
        SetActivePlayer(null);
        DebugLog("Game level unloaded");
    }

    public void DebugLog(string message){
        print($"[GAME INIT] {message}");
    }


    #region SETTERS AND GETTERS

    public PlayerBase GetActivePlayer(){
        return activePlayer;
    }

    public void SetActivePlayer(PlayerBase player){
        if(player != null){
            Assert.IsNull(activePlayer, "There already one active player");
        }

        activePlayer = player;
    }

    public Scene GetActiveLevel(){
        return activeLevelScene;
    }

    public void SetActiveLevel(Scene level){
        if(level != default(Scene)){
            Assert.IsTrue(activeLevelScene == default(Scene), "There already one active level scene");
        }

        activeLevelScene = level;
    }

    public Scene GetRootScene(){
        return SceneManager.GetSceneByName("GameRootScene");
    }

    #endregion
}
