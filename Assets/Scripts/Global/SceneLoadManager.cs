using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager instance;


    private async void Start() {
        instance = this;

        await Task.Yield();
        
        TryToLoadLevel("LevelSample");
    }


    #region SCENE LOADING

    public async void TryToLoadLevel(string levelName) {
        GameRoot gameRoot = GameRoot.instance;
        if(gameRoot.GetActiveLevel() != default(Scene)){
            Debug.LogError($"Tryed to load level {levelName}, while level scene exists");
            return;
        }

        // LOAD SCENE
        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        while(!loadSceneOperation.isDone) await Task.Yield();
        while(MainLevelManagement.instance == null) await Task.Yield();

        gameRoot.InitGameOnLevel(SceneManager.GetSceneByName(levelName));
    }

    #endregion
}
