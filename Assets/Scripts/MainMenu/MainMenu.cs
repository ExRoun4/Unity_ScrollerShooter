using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    const float BLACKHIDER_FADEIN_TIME = 1.0f;
    const float BLACKHIDER_FADEDIN_TIME = 0.5f;
    const float BLACKHIDER_FADEOUT_TIME = 5.5f;

    public GameObject main;
    public GameObject gameLobby;
    public GameObject newGameAlertPanel;
    public RawImage blackHider;


    private void Start()
    {
        main.SetActive(true);

        gameLobby.SetActive(false);
        newGameAlertPanel.SetActive(false);

        blackHider.color = Color.black;
        HideBlackHider();
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

    public async void EnterGameLobby(){
        await ShowBlackHider();

        HideAndResetMainPanel();
        newGameAlertPanel.SetActive(false);
        gameLobby.SetActive(true);

        await Task.Delay(TimeSpan.FromSeconds(BLACKHIDER_FADEDIN_TIME));
        await HideBlackHider();
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
