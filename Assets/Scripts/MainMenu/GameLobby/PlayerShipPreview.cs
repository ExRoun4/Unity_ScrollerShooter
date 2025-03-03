using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShipPreview : MonoBehaviour
{
    const float PREVIEW_CAMERA_ROTATE_SPEED = 15.0f;
    const int REPAIR_PRICE_PER_1F_VALUE = 2500;

    [Serializable]
    public struct PlayerShipShopData {
        public GameDataSystem.PlayerShips shipIndex;
        public string shipName;
        public int upgradeToNextPrice;
        public GameObject previewObject;
    }

    public PlayerShipShopData[] playerShipsShopData;
    public GameObject previewCameraMount;
    public Button repairButton;
    public Button upgradeButton;
    public Button prevShipButton;
    public Button nextShipButton;
    public TextMeshProUGUI shipNameText;
    public TextMeshProUGUI shipRepairPriceText;
    public TextMeshProUGUI shipUpgradePriceText;


    private void Start()
    {
        Assert.IsTrue(playerShipsShopData.Length == GameDataSystem.instance.playerShips.Length, "Ships Preview != PlayerShips length");

        GlobalSignal shipChanged = GameDataSystem.instance.GetPlayerData().shipChanged;
        shipChanged.connect(FullRebuildUI);
        MainMenu.instance.gameLobbyLoaded.connect(FullRebuildUI);
    }

    private void FullRebuildUI(){
        ShowCurrentShipPreview();
        RebuildPlayerShipUI();
    }

    private void Update()
    {
        previewCameraMount.transform.Rotate(Vector3.up, -PREVIEW_CAMERA_ROTATE_SPEED * Time.deltaTime);
    }

    private int GetCurrentShipRepairPrice(){
        GameDataSystem.PlayerData playerData = GameDataSystem.instance.GetPlayerData();
        float playerHealthLeft = playerData.healthLeft;
        int currentShipIndex = playerData.currentShipIndex;
        float healthToRepair = GameDataSystem.instance.playerShips[currentShipIndex].attributes.maxHealth - playerHealthLeft;

        return (int) (healthToRepair * REPAIR_PRICE_PER_1F_VALUE);
    }

    private int GetShipUpgradePrice(){
        GameDataSystem.PlayerData playerData = GameDataSystem.instance.GetPlayerData();
        
        if(playerData.currentShipIndex + 1 == GameDataSystem.instance.playerShips.Length) return 0;

        return playerShipsShopData[playerData.currentShipIndex].upgradeToNextPrice;
    }


    #region UI BUILDING

    public void ShowCurrentShipPreview(){
        if(GameRoot.instance.IsInGame()) return;

        foreach(PlayerShipShopData preview in playerShipsShopData){
            preview.previewObject.SetActive(false);
        }

        PlayerShipShopData currentShipPreview = playerShipsShopData[GameDataSystem.instance.GetPlayerData().currentShipIndex];
        currentShipPreview.previewObject.SetActive(true);
    }

    public void RebuildPlayerShipUI(){
        if(GameRoot.instance.IsInGame()) return;

        GameDataSystem.PlayerData playerData = GameDataSystem.instance.GetPlayerData();
        int currentShipIndex = playerData.currentShipIndex;
        PlayerShipShopData shipData = playerShipsShopData[currentShipIndex];
        List<int> purchasedShipIndexes = playerData.purchasedShipsIndexes;

        bool isCurrentShipLast = currentShipIndex + 1 == GameDataSystem.instance.playerShips.Length;
        int repairPrice = GetCurrentShipRepairPrice();
        int playerCurrency = playerData.currentCurrency;

        // BUILD TEXTS
        shipNameText.text = playerShipsShopData[currentShipIndex].shipName;
        shipUpgradePriceText.text = isCurrentShipLast ? "MAX" : shipData.upgradeToNextPrice.ToString();
        shipRepairPriceText.text = repairPrice == 0 ? "MAX" : repairPrice.ToString();

        // SET BUTTONS INTERACTABLE
        repairButton.interactable = repairPrice == 0 ? false : playerCurrency >= repairPrice;
        upgradeButton.interactable = isCurrentShipLast ? false : playerCurrency >= shipData.upgradeToNextPrice;
        prevShipButton.interactable = currentShipIndex != 0;
        nextShipButton.interactable = isCurrentShipLast ? false : purchasedShipIndexes.Contains(currentShipIndex + 1);
    }

    #endregion


    #region ACTIONS

    public void RepairShip(){
        GameDataSystem.instance.RestorePlayerCurrentHealth(GetCurrentShipRepairPrice());
        RebuildPlayerShipUI();
    }

    public void UpgradeShipToNew(){
        GameDataSystem.instance.UpgradeShipToNew(GetShipUpgradePrice());
    }

    public void SwitchShipToPrev(){
        GameDataSystem.instance.AddCurrentShipIndexValue(-1);
    }

    public void SwitchShipToNext(){
        GameDataSystem.instance.AddCurrentShipIndexValue(1);
    }

    #endregion
}
