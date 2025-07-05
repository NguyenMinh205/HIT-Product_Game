using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;


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
    [SerializeField] public EnemyController enemyController;
    [SerializeField] public PlayerManager playerController;
    [SerializeField] private ClawController clawController;
    [SerializeField] private ItemController itemController;

    [Space]
    [Header("Room")]
    [SerializeField] private GameObject DefaultRoom;
    private IntoRoomTrigger intoRoomTrigger;

    [Space]
    [Header("Machine")]
    [SerializeField] private GameObject DefaultClawMachineBox;

    [Space]
    [Header("TurnDisplay")]
    [SerializeField] private GameObject uiTurnChange;
    [SerializeField] private TextMeshProUGUI textTurn;

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
                playerController.CurrentPlayer.CheckIsPoison();
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
        SetActiveRoom(true);  // bat room
        PlayerMapController.Instance.IsIntoRoom = true;

        // spawn enemy and player
        enemyController.Spawn();
        playerController.SpawnPlayer();
        clawController.Spawn();
        itemController.Spawn(playerController.CurrentPlayer.Inventory);

        clawController.IsStart = true;
        clawController.StartClaw();
    }
    public void LoseGame()
    {
        DefaultRoom.SetActive(false);
        DefaultClawMachineBox.SetActive(false);

        clawController.EndGame();
        clawController.IsStart = false;
        //ItemController.Instance.EndGame();
        enemyController.EndGame();
        //playerController.EndGame();

        MapController.Instance.SetActiveMapStore(true);
        StartCoroutine(DelayOutTrigger(0.2f));
        MapManager.Instance.SetActiveRoomVisual(true);

        PlayerMapController.Instance.IsIntoRoom = false;

        PlayerMapController.Instance.IsMoving = false;

        StartCoroutine(PlayerMapController.Instance.MoveToPosition(-1 * directionPlayer));

        SetActiveRoom(false);
    }
    IEnumerator DelayOutTrigger(float time)
    {
        intoRoomTrigger.SetActive(false);
        yield return new WaitForSeconds(time);
        intoRoomTrigger.SetActive(true);
    }
    public void WInGame()
    {
        clawController.EndGame();
        clawController.IsStart = false;
        //ItemController.Instance.EndGame();
        enemyController.EndGame();
        //playerController.EndGame();

        MapController.Instance.SetActiveMapStore(true);
        MapManager.Instance.SetActiveRoomVisual(true);

        PlayerMapController.Instance.IsIntoRoom = false;
        PlayerMapController.Instance.IsMoving = false;

        intoRoomTrigger.SetActive(false);

        SetActiveRoom(false);
    }
    IEnumerator DelayPlayerMoveInMap(float time)
    {
        yield return new WaitForSeconds(time);
        MapController.Instance.SetActiveMapStore(false);
    }

    public void SetActiveRoom(bool val)
    {
        DefaultRoom.SetActive(val);
        DefaultClawMachineBox.SetActive(val);
    }
}

