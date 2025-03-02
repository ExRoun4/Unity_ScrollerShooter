using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager instance;
    
    public Dictionary<int, string> levels = new ();

    #region INITIALIZATION

    private void Awake() {
        instance = this;
        
        BuildWorlds();
    }

    private void BuildWorlds(){
        // WORLD 1
        AddLevel("LevelSample");
    }

    private void AddLevel(string worldName){
        levels[levels.Count] = worldName;
    }

    #endregion




    #region SCENE LOADING

    public async Awaitable TryToLoadGameLevel() {
        GameRoot gameRoot = GameRoot.instance;
        int levelIndex = GameDataSystem.instance.GetPlayerData().currentLevelIndex;
        
        if(levelIndex + 1 > levels.Count){
            Debug.LogError($"Run out of game levels");
            return;
        }

        string levelName = levels[levelIndex];
        if(gameRoot.GetActiveLevel() != default(Scene)){
            Debug.LogError($"Tryed to load level {levelName}, while level scene exists");
            return;
        }

        // LOAD SCENE
        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        while(!loadSceneOperation.isDone) await Task.Yield();
        while(MainLevelManagement.instance == null) await Task.Yield();

        await gameRoot.InitGameOnLevel(SceneManager.GetSceneByName(levelName));
    }

    #endregion
}
