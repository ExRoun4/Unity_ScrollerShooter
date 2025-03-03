using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopItem : MonoBehaviour
{
    public WeaponShopDecl weaponDecl;
    
    public Button buyButton;
    public Button upgradeButton;
    public Button chooseButton;
    public TextMeshProUGUI priceText;
    public Slider levelsBar;

    private int currentInteractionPrice = 0;


    private void Start()
    {
        GameDataSystem.instance.GetPlayerData().currencyChanged.connect(RebuildUI);
        GameDataSystem.instance.GetPlayerData().weaponChanged.connect(CheckIfWeaponChoosed);
        MainMenu.instance.gameLobbyLoaded.connect(FullRebuildUI);
    }


    #region UI BUILDING

    public void FullRebuildUI()
    {
        RebuildUI();
        CheckIfWeaponChoosed();
    }

    public void RebuildUI(){
        if(GameRoot.instance.IsInGame()) return;

        GameDataSystem.PlayerWeaponData weaponData = GameDataSystem.instance.GetPlayerData().allWeaponsData[(int)weaponDecl.weaponIndex];
        int playerCurrency = GameDataSystem.instance.GetPlayerData().currentCurrency;
        bool isWeaponOnMaxLevel = weaponData.currentLevel == GameDataSystem.PLAYER_WEAPONS_MAX_LEVEL;
        if(!isWeaponOnMaxLevel) currentInteractionPrice = weaponDecl.levelPrices[weaponData.currentLevel];

        // INIT BUTTONS
        buyButton.gameObject.SetActive(!weaponData.isPurchased);
        buyButton.interactable = playerCurrency >= weaponDecl.levelPrices[0];

        upgradeButton.gameObject.SetActive(weaponData.isPurchased);
        upgradeButton.interactable = isWeaponOnMaxLevel ? false : playerCurrency >= currentInteractionPrice;

        // OTHER
        priceText.text = isWeaponOnMaxLevel ? "MAX" : currentInteractionPrice.ToString();
        levelsBar.value = weaponData.currentLevel;
    }

    public void CheckIfWeaponChoosed(){
        if(GameRoot.instance.IsInGame()) return;

        chooseButton.interactable = (int)weaponDecl.weaponIndex != GameDataSystem.instance.GetPlayerData().currentWeaponIndex;
    }

    #endregion


    #region INTERACTIONS

    public void BuyWeapon(){
        GameDataSystem.instance.PurhcaseWeapon((int)weaponDecl.weaponIndex, currentInteractionPrice);
    }

    public void UpgradeWeapon(){
        GameDataSystem.instance.UpgradeWeapon((int)weaponDecl.weaponIndex, currentInteractionPrice);
    }

    public void ChooseWeapon(){
        GameDataSystem.instance.ChooseWeapon((int)weaponDecl.weaponIndex);
    }

    #endregion
}
