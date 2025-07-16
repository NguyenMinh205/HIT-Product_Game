using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EventID
{
    OnStartPlayerTurn,
    OnTakeDamage,
    OnDealDamage,
    OnGoldChanged,
    OnHealthChanged,
    OnStartEnemyTurn,
    OnBasketEmpty,
    OnClawsEmpty
}

public enum TurnPlay
{
    Player,
    Enemy
}

public class GamePlayController : Singleton<GamePlayController>
{
    [SerializeField] private TurnPlay turnGame;

    [Space]
    [Header("Controller")]
    [SerializeField] private EnemyController enemyController;
    public EnemyController EnemyController => enemyController;
    [SerializeField] private PlayerManager playerController;
    public PlayerManager PlayerController => playerController;
    [SerializeField] private ClawController clawController;
    public ClawController ClawController => clawController;
    [SerializeField] private ItemController itemController;
    public ItemController ItemController => itemController;

    private IntoRoomTrigger intoRoomTrigger;

    [Space]
    [Header("TurnDisplay")]
    [SerializeField] private GameObject uiTurnChange;
    [SerializeField] private TextMeshProUGUI textTurn;

    [Space]
    [Header("CheckTurn")]
    public bool isCheckTurnByClaw;
    public bool isCheckTurnByItem;
    private bool isEndGame = false;
    public bool IsEndGame => isEndGame;

    private Vector2Int directionPlayer;

    public Vector2Int Dir
    {
        set => directionPlayer = value;
    }

    public IntoRoomTrigger IntoRoom
    {
        set => intoRoomTrigger = value;
    }

    protected override void Awake()
    {
        turnGame = TurnPlay.Player;
        isCheckTurnByClaw = false;
        isCheckTurnByItem = false;
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnBasketEmpty, HandleBasketEmpty);
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnClawsEmpty, HandleClawsEmpty);
    }

    private void HandleBasketEmpty(object obj)
    {
        isCheckTurnByItem = true;
        CheckTurnConditions();
        Debug.Log("Basket is empty, checking turn conditions.");
    }

    private void HandleClawsEmpty(object obj)
    {
        isCheckTurnByClaw = true;
        CheckTurnConditions();
        Debug.Log("Claws are empty, checking turn conditions.");
    }

    private void CheckTurnConditions()
    {
        if (isCheckTurnByClaw && (isCheckTurnByItem || !itemController.IsPickupItemSuccess) && turnGame == TurnPlay.Player)
        {
            Turn = TurnPlay.Enemy;
            Debug.Log("Both claws and basket are empty, switching to Enemy turn.");
        }
        else
        {
            if (isCheckTurnByItem)
            {
                isCheckTurnByItem = false;
                itemController.IsPickupItemSuccess = false;
            }
        } 
    }

    public TurnPlay Turn
    {
        get => turnGame;
        set
        {
            if (turnGame != value)
            {
                HandleTurnChange(value);
            }
        }
    }

    private void HandleTurnChange(TurnPlay newTurn)
    {
        turnGame = newTurn;
        ShowChangeTurn();

        switch (newTurn)
        {
            case TurnPlay.Enemy:
                TurnEnemy();
                break;

            case TurnPlay.Player:
                playerController.ResetShield();
                clawController.ResetMachineClaw();
                StartPlayerTurn();
                break;
        }
    }

    private void ShowChangeTurn()
    {
        textTurn.text = turnGame == TurnPlay.Player ? "Your Turn" : "Enemy Turn";
        uiTurnChange.SetActive(true);
        StartCoroutine(DelayTurnDisplay(0.8f));
    }

    private IEnumerator DelayTurnDisplay(float time)
    {
        yield return new WaitForSeconds(time);
        uiTurnChange.SetActive(false);
    }

    public void StartPlayerTurn()
    {
        ObserverManager<EventID>.PostEven(EventID.OnStartPlayerTurn);
        playerController.CurrentPlayer.AddItem();
        itemController.SpawnAdditionalItems();
        Debug.Log("Player Turn Started");
    }

    public void TurnEnemy()
    {
        enemyController.ResetShield();
        isCheckTurnByClaw = false;
        isCheckTurnByItem = false;
        StartCoroutine(enemyController.CheckEnemyToNextTurn());
    }

    public void StartFightRoom()
    {
        ShowChangeTurn();
        isEndGame = false;

        enemyController.Spawn();
        playerController.SpawnPlayer();
        clawController.Spawn();
        itemController.Spawn(playerController.CurrentPlayer.Inventory);

        clawController.IsStart = true;
        clawController.StartClaw();
    }

    public void LoseGame()
    {
        isEndGame = true;
        clawController.EndGame();
        clawController.IsStart = false;
        enemyController.EndGame();
        playerController.EndGame();
        itemController.EndGame();

        GameManager.Instance.OutRoom();
        StartCoroutine(PlayerMapController.Instance.MoveToPosition(-1 * directionPlayer));
    }

    public void WinGame()
    {
        isEndGame = true;
        clawController.EndGame();
        clawController.IsStart = false;
        enemyController.EndGame();
        playerController.EndGame();
        itemController.EndGame();

        int bonusGold = 3 + MapManager.Instance.MapIndex;
        playerController.CurrentPlayer.Stats.ChangeCoin(bonusGold);

        GameManager.Instance.RewardUI.SetActive(true);
        RewardManager.Instance.InitReward();
    }
}