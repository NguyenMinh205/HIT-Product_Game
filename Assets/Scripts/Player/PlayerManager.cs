using TMPro;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : Singleton<PlayerManager>
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
        ability = CharacterAbilityFactory.CreateAbility(curCharacter.id);
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
        ability?.StartSetupEffect(currentPlayer);
    }

    public void ResetShield()
    {
        currentPlayer.Stats.ChangeShield(0);
        currentPlayer.Health.UpdateArmor(currentPlayer);
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
                if (curPlayerStat == null)
                {
                    Debug.LogError("Bug1");
                }
                totalInventory = GameData.Instance.mainGameData.playerData.inventory;
                if (totalInventory == null)
                {
                    Debug.LogError("Bug2");
                }
            }
            else
            {
                Debug.LogError("No player data found!");
            }
        });
    }
}