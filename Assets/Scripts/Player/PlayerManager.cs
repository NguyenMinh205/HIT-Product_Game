using TMPro;
using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerParent;
    [SerializeField] private Transform posSpawnPlayer;
    [SerializeField] private CharacterDatabaseSO characterDatabase;
    [SerializeField] private CharacterStat playerStatBase = new CharacterStat();
    [SerializeField] private CharacterStat curPlayerStat = new CharacterStat();
    [SerializeField] private Inventory totalInventory = new Inventory();
    [SerializeField] private TextMeshProUGUI numOfCoinInRoom;
    public Inventory TotalInventory => totalInventory;
    public CharacterStat CurPlayerStat { get { return curPlayerStat; } set { curPlayerStat = value; SavePlayerData(); } }

    private Character curCharacter;
    private Player currentPlayer;
    private ICharacterAbility ability;
    public List<StartRoundBuffInfo> startRoundBuffs = new List<StartRoundBuffInfo>();

    public Player CurrentPlayer
    {
        get => currentPlayer;
    }

    public Vector3 PosPlayer
    {
        get => currentPlayer.transform.position;
    }

    private void Start()
    {
        curCharacter = characterDatabase.GetCharacterById(GameData.Instance.startData.selectedCharacterId);
        ability = CharacterAbilityFactory.CreateAbility(curCharacter.id);
        if (GameData.Instance.startData.isKeepingPlayGame)
        {
            LoadPlayerData();
            return;
        }
        if (totalInventory != null && curCharacter.initialItems != null)
        {
            foreach (ItemInventory item in curCharacter.initialItems)
            {
                totalInventory.AddItem(item.itemId, item.quantity, item.quantity, item.isUpgraded);
            }
        }
        curPlayerStat = playerStatBase.Clone();
        if (ability != null && curPlayerStat != null)
        {
            ability.StartSetupStat();
        }

        SavePlayerData();
    }
    public void SetPosPlayer(GameObject currenRoom)
    {
        posSpawnPlayer = currenRoom.transform.Find("PlayerPos");
    }
    public void SpawnPlayer()
    {
        GameObject newObject = PoolingManager.Spawn(playerPrefab, posSpawnPlayer.position, Quaternion.identity, playerParent);
        currentPlayer = newObject.transform.Find("PlayerPrefab").GetComponent<Player>();
        currentPlayer.Initialize(curCharacter, curPlayerStat, GameData.Instance.startData.selectedSkinIndex);
        ability.StartSetupEffect(currentPlayer);
        if (startRoundBuffs.Count > 0)
        {
            foreach (var buff in startRoundBuffs)
            {
                currentPlayer.AddBuffEffect(buff.name, buff.value, buff.duration);
            }
        }
    }

    public void ResetShield()
    {
        int retainBlock = (int)(currentPlayer.Stats.RetainedBlock * currentPlayer.Stats.Shield);
        currentPlayer.Stats.ChangeShield(-(currentPlayer.Stats.Shield - retainBlock));
        currentPlayer.UpdateArmorUI();
    }

    public void EndGame()
    {
        currentPlayer?.EndGame();
    }

    public void UpdateCoinText()
    {
        numOfCoinInRoom.text = CurrentPlayer.Stats.Coin.ToString();
    }

    public void SavePlayerData()
    {
        GameData.Instance.mainGameData.playerData.stats = curPlayerStat.Clone();
        if (GameData.Instance.mainGameData.playerData.stats == null)
        {
            Debug.LogError("Player stats are null, cannot save player data!");
        }
        GameData.Instance.mainGameData.playerData.inventory = totalInventory;
        if (GameData.Instance.mainGameData.playerData.inventory == null)
        {
            Debug.LogError("Player inventory is null, cannot save player data!");
        }
        GameData.Instance.mainGameData.playerData.startRoundBuffs = new List<StartRoundBuffInfo>(startRoundBuffs);
        if (GameData.Instance.mainGameData.playerData.startRoundBuffs == null)
        {
            Debug.LogError("Player start round buffs are null, cannot save player data!");
        }
    }

    public void LoadPlayerData()
    {
        GameData.Instance.LoadMainGameData();
        DOVirtual.DelayedCall(0.25f, () =>
        {
            if (GameData.Instance.mainGameData.playerData != null)
            {
                if (GameData.Instance.mainGameData.playerData.stats == null)
                {
                    Debug.LogError("Player stats are null, cannot load player data!");
                }
                curPlayerStat = GameData.Instance.mainGameData.playerData.stats.Clone();
                totalInventory = GameData.Instance.mainGameData.playerData.inventory;
                startRoundBuffs = GameData.Instance.mainGameData.playerData.startRoundBuffs;
            }
            else
            {
                Debug.LogError("No player data found!");
            }
        });
    }
}

[Serializable]
public class StartRoundBuffInfo
{
    public string name;
    public int value;
    public int duration;

    public StartRoundBuffInfo(string name, int value, int duration)
    {
        this.name = name;
        this.value = value;
        this.duration = duration;
    }
}