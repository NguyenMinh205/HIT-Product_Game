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

public class GameController : Singleton<GameController>
{
    [SerializeField] private TurnPlay turnGame;

    [Space]
    [Header("Controller")]
    [SerializeField] public EnemyController enemyController;
    [SerializeField] public PlayerManager playerController;
    [SerializeField] private ClawController clawController;

    [Space]
    [Header("Object")]
    [SerializeField] private GameObject DefaultRoom;
    [SerializeField] private GameObject DefaultClawMachineBox;

    [Space]
    [Header("TurnDisplay")]
    [SerializeField] private GameObject uiTurnChange;
    [SerializeField] private TextMeshProUGUI textTurn;

    [Space]
    [Header("CheckTurn")]
    public bool isCheckTurnByClaw;
    public bool isCheckTurnByItem;

    [Space]
    [Header("Button")]
    [SerializeField] private Button btnNextRoom;

    private void Awake()
    {
        turnGame = TurnPlay.Player;
        isCheckTurnByClaw = false;
        isCheckTurnByItem = false;
    }

    private void Start()
    {
        btnNextRoom.onClick.AddListener(delegate
        {
            OutRoom();
        });
    }
    public TurnPlay Turn
    {
        get => this.turnGame;
        set 
        {
            if (this.turnGame != value)
            {
                this.turnGame = value;
                ShowChangeTurn();
            }
            if(value == TurnPlay.Enemy)
            {
                isCheckTurnByClaw = false;
                isCheckTurnByItem = false;
                TurnEnemy();
            }
            if(value == TurnPlay.Player)
            {
                ItemController.Instance.Spawn();
                enemyController.InitActionListEnemy();
                clawController.ResetMachineClaw();
            }
        }
    }
    public EnemyController Enemy
    {
        get => this.enemyController;
    }
    public PlayerManager Player
    {
        get => this.playerController;
    }

    private void Update()
    {
        TurnPlayer();
    }

    public void ShowChangeTurn()
    {
        if (turnGame == TurnPlay.Player)
            textTurn.text = "You Turn";
        else
            textTurn.text = "Enemy Turn";
        uiTurnChange.SetActive(true);
        StartCoroutine(DelayTurnDisplay(0.8f));
    }
    IEnumerator DelayTurnDisplay(float time)
    {
        yield return new WaitForSeconds(time);
        uiTurnChange.SetActive(false);
    }
    public void StartRoom()
    {
        Debug.Log("Open Room");
        ShowChangeTurn();
        DefaultRoom.SetActive(true);
        DefaultClawMachineBox.SetActive(true);

        enemyController.SpawnEnemy();
        playerController.SpawnPlayer();
        clawController.Spawn();
        ItemController.Instance.Spawn();

        clawController.IsStart = true;
        clawController.StartClaw();
        btnNextRoom.gameObject.SetActive(true);
    }
    public void TurnPlayer()
    {
        if (turnGame != TurnPlay.Player) return;

        if(isCheckTurnByClaw && isCheckTurnByItem)
        {
            Turn = TurnPlay.Enemy;
        }
    }
    public void TurnEnemy()
    {
        StartCoroutine(enemyController.CheckEnemyToNextTurn());
    }
    public void OutRoom()
    {
        DefaultRoom.SetActive(false);
        DefaultClawMachineBox.SetActive(false);
        btnNextRoom.gameObject.SetActive(false);

        clawController.EndGame();
        clawController.IsStart = false;
        ItemController.Instance.EndGame();
        enemyController.EndGame();
        playerController.EndGame();

        MapController.Instance.SetActiveMapStore(true);
        MapManager.Instance.SetActiveRoomVisual(true);

        PlayerMapController.Instance.IsIntoRoom = false;
        PlayerMapController.Instance.IsMoving = false;
    }
}

