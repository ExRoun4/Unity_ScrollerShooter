using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    const float BLACKHIDER_FADEIN_TIME = 1.0f;
    const float BLACKHIDER_FADED_STATIC_TIME = 0.5f;
    const float BLACKHIDER_FADEOUT_TIME = 5.5f;

    [Header("MAIN ELEMENTS")]
    public GameObject main;
    public GameObject gameLobby;
    public GameObject newGameAlertPanel;
    public RawImage blackHider;
    [Space(10)]
    
    [Header("OTHER GUI ELEMENTS")]
    public Button continueButton;

    public GlobalSignal gameLobbyLoaded = new ();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitGUI();
        blackHider.color = Color.black;
        HideBlackHider();
    }

    private void InitGUI(){
        main.SetActive(true);
        gameLobby.gameObject.SetActive(false);
        newGameAlertPanel.SetActive(false);
        continueButton.interactable = GameDataSystem.instance.IsDataDirty();
    }

    #region MAIN ACTIONS

    public void StartNewGame(bool confirmed = false){
        if(!confirmed && GameDataSystem.instance.IsDataDirty()){
            newGameAlertPanel.SetActive(true);
        } else {
            GameDataSystem.instance.ResetPlayerData();
            EnterGameLobby();
        }
    }

    public async void EnterGameLobby(bool showBlackHider = true){
        if(showBlackHider) await ShowBlackHider();

        HideAndResetMainPanel();
        newGameAlertPanel.SetActive(false);
        gameLobby.gameObject.SetActive(true);
        gameLobbyLoaded.emit();

        await Task.Delay(TimeSpan.FromSeconds(BLACKHIDER_FADED_STATIC_TIME));
        await HideBlackHider();
    }

    public async void StartGame(){
        await ShowBlackHider();

        gameLobby.gameObject.SetActive(false);

        await Task.Delay(TimeSpan.FromSeconds(BLACKHIDER_FADED_STATIC_TIME));
        await SceneLoadManager.instance.TryToLoadGameLevel();

        HideBlackHider();
    }

    public void HideAndResetMainPanel(){
        main.SetActive(false);
    }

    #endregion


    #region VISUAL ACTIONS

    public void ShowObject(GameObject obj){
        obj.SetActive(true);
    }

    public void HideObject(GameObject obj){
        obj.SetActive(false);
    }

    public async Awaitable ShowBlackHider(){
        blackHider.raycastTarget = true;
        blackHider.DOKill();
        await blackHider.DOFade(1.0f, BLACKHIDER_FADEIN_TIME).AsyncWaitForCompletion();
    }

    public async Awaitable HideBlackHider(){
        blackHider.raycastTarget = false;
        blackHider.DOKill();
        await blackHider.DOFade(0.0f, BLACKHIDER_FADEOUT_TIME).AsyncWaitForCompletion();
    }

    #endregion
}
