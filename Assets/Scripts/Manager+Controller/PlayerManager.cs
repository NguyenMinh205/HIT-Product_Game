using TMPro;
using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using TranDuc;

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
    public TextMeshProUGUI NumOfCoinInRoom => numOfCoinInRoom;
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
        curCharacter = characterDatabase.GetCharacterById(DataManager.Instance.GameData.SelectedCharacterId);
        ability = CharacterAbilityFactory.CreateAbility(curCharacter.id);
        if (DataManager.Instance.GameData.IsKeepingPlayGame)
        {
            LoadPlayerData();
            DataManager.Instance.GameData.SetKeepPlayState(false);

            DOVirtual.DelayedCall(0.25f, () =>
            {
                ObserverManager<IDMap>.PostEven(IDMap.UpdateHpBar);
            });
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
        currentPlayer.Initialize(curCharacter, curPlayerStat.Clone(), DataManager.Instance.GameData.SelectedSkinIndex);
        DOVirtual.DelayedCall(0.25f, () =>
        {
            ability.StartSetupEffect(currentPlayer);
            if (startRoundBuffs.Count > 0)
            {
                foreach (var buff in startRoundBuffs)
                {
                    currentPlayer.AddBuffEffect(buff.name, buff.value, buff.duration);
                }
            }
        });
        ObserverManager<EventID>.PostEven(EventID.OnStartRound);
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
        if (currentPlayer == null)
        {
            numOfCoinInRoom.text = curPlayerStat.Coin.ToString();
            return;
        }
        numOfCoinInRoom.text = currentPlayer.Stats.Coin.ToString();
    }

    public void SavePlayerData()
    {
        DataManager.Instance.GameData.Player.stats = curPlayerStat.Clone();
        if (DataManager.Instance.GameData.Player.stats == null)
        {
            Debug.LogError("Player stats are null, cannot save player data!");
        }
        DataManager.Instance.GameData.Player.inventory = totalInventory;
        if (DataManager.Instance.GameData.Player.inventory == null)
        {
            Debug.LogError("Player inventory is null, cannot save player data!");
        }
        DataManager.Instance.GameData.Player.startRoundBuffs = startRoundBuffs;
        if (DataManager.Instance.GameData.Player.startRoundBuffs == null)
        {
            Debug.LogError("Player start round buffs are null, cannot save player data!");
        }
    }

    public void LoadPlayerData()
    {
        DOVirtual.DelayedCall(0.25f, () =>
        {
            if (DataManager.Instance.GameData.Player != null)
            {
                if (DataManager.Instance.GameData.Player.stats == null)
                {
                    Debug.LogError("Player stats are null, cannot load player data!");
                }
                curPlayerStat = DataManager.Instance.GameData.Player.stats.Clone();
                totalInventory = DataManager.Instance.GameData.Player.inventory;
                startRoundBuffs = DataManager.Instance.GameData.Player.startRoundBuffs;
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