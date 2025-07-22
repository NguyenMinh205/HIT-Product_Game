using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public StartGameDataSave startData;
    public MainGameDataSave mainGameData;

    protected override void Awake()
    {
        base.Awake();
        startData = new StartGameDataSave();
        mainGameData = new MainGameDataSave();
        LoadAllData();
    }

    #region LOAD DATA
    public void LoadAllData()
    {
        LoadStartGameData();
        LoadMainGameData();
    }

    public void LoadStartGameData()
    {
        LoadData(eData.StartGameData, ref startData);
    }

    public void LoadMainGameData()
    {
        LoadData(eData.MainGameData, ref mainGameData);
    }
    #endregion LOAD DATA

    #region CLEAR DATA
    public void ClearAllData()
    {
        startData = new StartGameDataSave();
        mainGameData = new MainGameDataSave();
        SaveGameManager.DeleteAllSave();
        SaveAllData();
    }

    public void ClearStartGameData()
    {
        startData = new StartGameDataSave();
        SaveGameManager.DeleteSave(eData.StartGameData);
        SaveStartGameData();
    }

    public void ClearMainGameData()
    {
        mainGameData = new MainGameDataSave();
        SaveGameManager.DeleteSave(eData.MainGameData);
        SaveMainGameData();
    }

    #endregion CLEAR DATA

    #region SAVE DATA
    public void SaveAllData()
    {
        SaveStartGameData();
        SaveMainGameData();
    }

    public void SaveStartGameData()
    {
        SaveData(eData.StartGameData, startData);
    }

    public void SaveMainGameData()
    {
        SaveData(eData.MainGameData, mainGameData);
    }

    public void SaveData<T>(eData filename, T value)
    {
        var save = new SaveUtility<T>();
        save.SaveData(filename, value);
    }

    public void LoadData<T>(eData filename, ref T variable)
    {
        var save = new SaveUtility<T>();
        save.LoadData(filename, ref variable);
    }
    #endregion SAVE DATA
}

[Serializable]
public class StartGameDataSave
{
    public float musicVolume = 0.5f;
    public float soundVolume = 0.5f;
    public int coin = 1000;
    public string selectedCharacterId;
    public int selectedSkinIndex;
    public int selectedDifficultyIndex;
    public float extraDamagePercent;
    public float extraHealthPercent;
    public List<CharacterState> characterStates = new List<CharacterState>();
    public bool isKeepingPlayGame = false;
}

[Serializable]
public class CharacterState
{
    public string id;
    public bool isUnlocked;
    public List<bool> skinUnlocks = new List<bool>();
}

[Serializable]
public class MainGameDataSave
{
    public PlayerData playerData;
    public Vector2Int playerNodePosition;
    public int curFloor;
    public MapData curMapData;
    public List<SpecialTile> specialTiles;
}

[Serializable]
public class PlayerData
{
    public CharacterStat stats;
    public Inventory inventory;
    public List<StartRoundBuffInfo> startRoundBuffs;
}