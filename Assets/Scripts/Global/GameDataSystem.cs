using System;
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
    public class PlayerWeaponHolder {
        public PlayerWeapons index;
        public PlayerWeaponBase prefab;
    }

    [Serializable]
    public class PlayerCurrentWeapon {
        public int weaponIndex = 0;
        public int currentLevel = 1;

        public PlayerWeaponHolder GetWeaponHolder(){
            return GameDataSystem.instance.playerWeapons[weaponIndex];
        }
    }

    [Serializable]
    public class PlayerShipHolder {
        public PlayerShips index;
        public GameObject modelPrefab;
        public PlayerShipAttributes attributes = new ();
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
        public PlayerCurrentWeapon currentWeapon;
        public int currentShipIndex;

        public PlayerShipHolder GetShip(){
            return GameDataSystem.instance.playerShips[currentShipIndex];
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

            DebugLog("Player data loaded");
        }
    }

    private void InitDefaultPlayerData(){
        DebugLog("Save file not found, initializing default data");
        
        playerData = new();
        playerData.currentWeapon = new PlayerCurrentWeapon();

        SaveData();
    }

    public void SaveData(){
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


    #region SETTERS AND GETTERS

    public PlayerData GetPlayerData(){
        return playerData;
    }

    #endregion
}
