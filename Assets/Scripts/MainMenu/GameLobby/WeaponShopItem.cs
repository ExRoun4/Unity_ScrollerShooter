using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopItem : MonoBehaviour
{
    public WeaponShopDecl weaponDecl;

    public Button buyButton;
    public Button upgradeButton;
    public TextMeshProUGUI priceText;


    private void Start()
    {
        BuildUI();
    }

    public void BuildUI(){
        GameDataSystem.PlayerWeaponData weaponData = GameDataSystem.instance.GetPlayerData().allWeaponsData[(int)weaponDecl.weaponIndex];
        int playerCurrency = GameDataSystem.instance.GetPlayerData().currentCurrency;
        bool isWeaponOnMaxLevel = weaponData.currentLevel == GameDataSystem.PLAYER_WEAPONS_MAX_LEVEL;

        // INIT BUTTONS
        buyButton.gameObject.SetActive(!weaponData.isPurchased);
        buyButton.interactable = playerCurrency >= weaponDecl.levelPrices[0];

        upgradeButton.gameObject.SetActive(weaponData.isPurchased);
        upgradeButton.interactable = isWeaponOnMaxLevel ? false : playerCurrency >= weaponDecl.levelPrices[weaponData.currentLevel];
        
        // OTHER
        priceText.text = isWeaponOnMaxLevel ? "MAX" : weaponDecl.levelPrices[weaponData.currentLevel].ToString();
    }
}
