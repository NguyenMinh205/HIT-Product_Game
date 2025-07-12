using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public enum EventID
{
    OnStartPlayerTurn,
    OnTakeDamage,
    OnDealDamage,
    OnGoldChanged,
    OnHealthChanged,
    OnStartEnemyTurn,
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
    [Header("Room")]

    
    [Space]
    [Header("CheckTurn")]
    public bool isCheckTurnByClaw;
    public bool isCheckTurnByItem;

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
                isCheckTurnByClaw = false;
                isCheckTurnByItem = false;
                StartCoroutine(enemyController.CheckEnemyToNextTurn());
                break;

            case TurnPlay.Player:
                clawController.ResetMachineClaw();
                //playerController.CurrentPlayer.AddItem();
                StartPlayerTurn();
                break;
        }
    }

    private void Update()
    {
        TurnPlayer();
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
        Debug.Log("Player Turn Started");
    }

    public void TurnPlayer()
    {
        if (turnGame != TurnPlay.Player) return;

        if (isCheckTurnByClaw && isCheckTurnByItem)
        {
            Turn = TurnPlay.Enemy;
        }
    }

    public void TurnEnemy()
    {
        StartCoroutine(enemyController.CheckEnemyToNextTurn());
    }

    public void StartRoom()
    {
        Debug.Log("Open Room");
        ShowChangeTurn();

        enemyController.Spawn();
        playerController.SpawnPlayer();
        clawController.Spawn();
        itemController.Spawn(playerController.CurrentPlayer.Inventory);

        clawController.IsStart = true;
        clawController.StartClaw();
    }

    public void LoseGame()
    {
        Debug.Log("---------------Lose Game ---------------");
        clawController.EndGame();
        clawController.IsStart = false;
        enemyController.EndGame();
        playerController.EndGame();
        itemController.EndGame();

        GameManager.Instance.OutRoom();
        /*MapController.Instance.SetActiveMapStore(true);
        intoRoomTrigger.gameObject.SetActive(false);
        StartCoroutine(DelayOutTrigger(0.2f));
        MapManager.Instance.SetActiveRoomVisual(true);*/

        /*PlayerMapController.Instance.IsIntoRoom = false;
        PlayerMapController.Instance.IsMoving = false;*/

        StartCoroutine(PlayerMapController.Instance.MoveToPosition(-1 * directionPlayer));

    }

    IEnumerator DelayOutTrigger(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void WInGame()
    {
        Debug.Log("---------------Win Game ---------------");
        clawController.EndGame();
        clawController.IsStart = false;
        enemyController.EndGame();
        playerController.EndGame();
        itemController.EndGame();
        GameManager.Instance.OutRoom();

        /*MapController.Instance.SetActiveMapStore(true);
        intoRoomTrigger.gameObject.SetActive(false);
        MapManager.Instance.SetActiveRoomVisual(true);*/

        /*PlayerMapController.Instance.IsIntoRoom = false;
        PlayerMapController.Instance.IsMoving = false;*/

        ObserverManager<IDMap>.PostEven(IDMap.UpdateHpBar, playerController.CurrentPlayer);
    }
}