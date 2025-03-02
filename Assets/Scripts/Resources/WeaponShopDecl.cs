using UnityEngine;

[CreateAssetMenu(fileName = "WeaponShopDecl", menuName = "Scriptable Objects/WeaponShopDecl")]
public class WeaponShopDecl : ScriptableObject
{
    public GameDataSystem.PlayerWeapons weaponIndex;
    public int[] levelPrices = new int[GameDataSystem.PLAYER_WEAPONS_MAX_LEVEL]; // FIRST INDEX = PURCHASE PRICE
}
