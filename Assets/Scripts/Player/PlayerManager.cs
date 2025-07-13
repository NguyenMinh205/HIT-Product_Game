using TMPro;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Transform playerParent;
    [SerializeField] private Transform posSpawnPlayer;
    [SerializeField] private CharacterDatabaseSO characterDatabase;
    [SerializeField] private CharacterStatSO playerStat;
    [SerializeField] private Inventory totalInventory;
    [SerializeField] private TextMeshProUGUI numOfCoin;
    [SerializeField] private TextMeshProUGUI numOfCoinInRoom;
    public Inventory TotalInventory => totalInventory;
    public CharacterStatSO PlayerStat => playerStat;

    private Character curCharacter;
    private Player currentPlayer;

    public Player CurrentPlayer
    {
        get => currentPlayer;
    }

    public Vector3 PosPlayer
    {
        get => currentPlayer.transform.position;
    }

    private void OnEnable()
    {
        curCharacter = characterDatabase.GetCharacterById(PlayerPrefs.GetString("SelectedCharacterId", ""));
        if (totalInventory != null && curCharacter.initialItems != null)
        {
            foreach (ItemInventory item in curCharacter.initialItems)
            {
                totalInventory.AddItem(item.itemBase, item.quantity);
            }
        }
    }

    public void SpawnPlayer()
    {
        currentPlayer = PoolingManager.Spawn(playerPrefab, posSpawnPlayer.position, Quaternion.identity, playerParent);
        currentPlayer.Initialize(curCharacter, playerStat.Clone(), PlayerPrefs.GetInt("SelectedSkinIndex", 0));
        currentPlayer.CalculationPositionPlayer(posSpawnPlayer.position);
    }
    public void ResetSheild()
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
        numOfCoin.text = CurrentPlayer.Stats.Coin.ToString();
        numOfCoinInRoom.text = CurrentPlayer.Stats.Coin.ToString();
    }    
}