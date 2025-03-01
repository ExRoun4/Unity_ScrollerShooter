using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;

public class GameDataSystem : MonoBehaviour
{
    #region ENUMS

    public enum PlayerWeapons {
        BASIC_WEAPON = 0
    }

    public enum PlayerShips {
        START_JET = 0
    }

    #endregion

    #region DATA HOLDERS

    [Serializable]
    public struct PlayerWeaponHolder {
        public PlayerWeapons index;
        public PlayerWeaponBase prefab;
    }

    [Serializable]
    public class PlayerWeaponData {
        public int weaponIndex = 0;
        public int currentLevel = 1;

        public PlayerWeaponHolder GetWeaponHolder(){
            return GameDataSystem.instance.playerWeapons[weaponIndex];
        }
    }

    [Serializable]
    public struct PlayerShipHolder {
        public PlayerShips index;
        public GameObject modelPrefab;
        public PlayerShipAttributes attributes;
    }

    [Serializable]
    public class PlayerShipAttributes {
        public float maxHealth;
        public float maxEnergy;

        public PlayerShipAttributes(){
            maxHealth = 1.0f;
            maxEnergy = 1.0f;
        }
    }

    #endregion


    #region DATA CLASSES

    [Serializable]
    public class PlayerData {
        public const int START_PLAYER_LIFES = 3;

        public int currentWeaponIndex = 0;
        public List<PlayerWeaponData> allWeaponsData = new ();
        public int currentShipIndex = 0;
        public int currentCurrency = 0;
        public int lifesLeft = 3;

        public int currentLevelIndex = 0; // ALL LEVEL DATA STORED IN GAMEROOT

        public bool isDirty = false;


        public PlayerShipHolder GetShip(){
            return GameDataSystem.instance.playerShips[currentShipIndex];
        }

        public PlayerWeaponData GetCurrentWeapon(){
            return allWeaponsData[currentWeaponIndex];
        }

        public void Reset(){
            currentWeaponIndex = 0;
            currentShipIndex = 0;
            currentCurrency = 0;
            lifesLeft = 3;
            currentLevelIndex = 0;
            isDirty = false;

            RebuildWeaponsData();
        }

        public void RebuildWeaponsData(){
            allWeaponsData.Clear();
            PlayerWeaponHolder[] playerWeapons = GameDataSystem.instance.playerWeapons;

            for(int i = 0; i < playerWeapons.Length; i++){
                AppendNewWeapon(i);
            }
        }

        public void CheckWeaponsForCompability(){
            PlayerWeaponHolder[] playerWeapons = GameDataSystem.instance.playerWeapons;
            if(allWeaponsData.Count == playerWeapons.Length) return;

            // REMOVE EXCESS
            if(allWeaponsData.Count > playerWeapons.Length) {
                allWeaponsData = new List<PlayerWeaponData>(playerWeapons.Length);
                return;
            }

            // APPEND NEW
            for(int i = 0; i < playerWeapons.Length; i++){
                if(i <= allWeaponsData.Count - 1) continue;
                AppendNewWeapon(i);
            }
        }

        private void AppendNewWeapon(int index){
            PlayerWeaponData newData = new ();
            newData.weaponIndex = index;
            allWeaponsData.Add(newData);
        }
    }

    #endregion

    const string fileName = "GameData";
    
    public static GameDataSystem instance;

    public PlayerWeaponHolder[] playerWeapons;
    public PlayerShipHolder[] playerShips;

    private PlayerData playerData;
    private string dataSavePath;


    #region INITIALIZATION

    private void Awake()
    {
        ProduceAssertions();

        instance = this;
        dataSavePath = $"{Application.persistentDataPath}/{fileName}.json";
        
        LoadData();
    }

    private void ProduceAssertions(){
        Assert.IsNull(instance, "There more, than 1 GameDataSystem");
        Assert.IsNotEmpty(playerWeapons, "PlayerWeapons is empty");
        Assert.IsNotEmpty(playerShips, "PlayerShipts is empty");
    }
    
    #endregion


    #region SAVE AND LOAD

    private void LoadData(){
        if(!File.Exists(dataSavePath)) {
            InitDefaultPlayerData();
            return;
        }

        using(StreamReader reader = new StreamReader(dataSavePath)){
            string json = reader.ReadToEnd();
            playerData = JsonUtility.FromJson<PlayerData>(json);
            playerData.CheckWeaponsForCompability();

            DebugLog("Player data loaded");
        }
    }

    private void InitDefaultPlayerData(){
        DebugLog("Save file not found, initializing default data");
        
        playerData = new();
        playerData.currentWeaponIndex = 0;
        playerData.RebuildWeaponsData();

        SaveData(false);
    }

    public void SaveData(bool makeDataDirty = true){
        playerData.isDirty = makeDataDirty;

        string directory = Path.GetDirectoryName(@dataSavePath);
        Directory.CreateDirectory(directory);

        string json = JsonUtility.ToJson(playerData);
        using(StreamWriter writer = new StreamWriter(dataSavePath)){
            writer.WriteLine(json);
        }

        DebugLog("Player data saved");
    }

    #endregion


    #region TOOLS

    public void DebugLog(string message){
        print($"[GameDataSystem] {message}");
    }

    #endregion


    #region DATA ACTIONS

    public void ResetPlayerData(){
        playerData.Reset();
        SaveData(false);
    }

    public void ReducePlayerLifes(int withValue){
        playerData.lifesLeft -= withValue;
        SaveData();
    }

    public void RestorePlayerLifes(){
        playerData.lifesLeft = PlayerData.START_PLAYER_LIFES;
        SaveData();
    }

    public void IncreaseCurrentLevelIndex(){
        playerData.currentLevelIndex++;
        SaveData();
    }

    public void AddPlayerCurrency(int value){
        playerData.currentCurrency += value;
        SaveData();
    }

    #endregion


    #region SETTERS AND GETTERS

    public PlayerData GetPlayerData(){
        return playerData;
    }

    public bool IsDataDirty(){
        return playerData.isDirty;
    }

    #endregion
}
