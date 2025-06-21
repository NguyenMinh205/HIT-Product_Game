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
    [Header("Room")]
    [SerializeField] private GameObject HealingRoom;
    [SerializeField] private GameObject MysteryRoom;
    [SerializeField] private GameObject PachinkoRoom;
    [SerializeField] private GameObject SmithRoom;
    [SerializeField] private GameObject ShredderRoom;
    [SerializeField] private GameObject BossRoom;
    [SerializeField] private GameObject DefaultRoom;
    private IntoRoomTrigger intoRoomTrigger;

    [Space]
    [Header("Machine")]
    [SerializeField] private GameObject DefaultClawMachineBox;
    [SerializeField] private GameObject GachaMachineBox;
    [SerializeField] private GameObject PachinkoMachineBox;
    [SerializeField] private GameObject TumblerMachineBox;

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
    [SerializeField] private Button btnWinGame;
    [SerializeField] private Button btnLoseGame;

    private Vector2Int directionPlayer;
    public Vector2Int Dir
    {
        set => directionPlayer = value;
    }

    public IntoRoomTrigger IntoRoom
    {
        set => intoRoomTrigger = value;
    }

    private void Awake()
    {
        turnGame = TurnPlay.Player;
        isCheckTurnByClaw = false;
        isCheckTurnByItem = false;
    }

    private void Start()
    {
        btnWinGame.onClick.AddListener(delegate
        {
            WInGame();
        });
        btnLoseGame.onClick.AddListener(delegate
        {
            LoseGame();
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

        //Button test
        btnWinGame.gameObject.SetActive(true);
        btnLoseGame.gameObject.SetActive(true);
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
    public void LoseGame()
    {
        DefaultRoom.SetActive(false);
        DefaultClawMachineBox.SetActive(false);

        //Button test
        btnWinGame.gameObject.SetActive(false);
        btnLoseGame.gameObject.SetActive(false);

        clawController.EndGame();
        clawController.IsStart = false;
        ItemController.Instance.EndGame();
        enemyController.EndGame();
        playerController.EndGame();

        MapController.Instance.SetActiveMapStore(true);
        StartCoroutine(DelayOutTrigger(0.2f));
        MapManager.Instance.SetActiveRoomVisual(true);

        PlayerMapController.Instance.IsIntoRoom = false;

        PlayerMapController.Instance.IsMoving = false;

        StartCoroutine(PlayerMapController.Instance.MoveToPosition(-1*directionPlayer));

    }
    IEnumerator DelayOutTrigger(float time)
    {
        intoRoomTrigger.SetActive(false);
        yield return new WaitForSeconds(time);
        intoRoomTrigger.SetActive(true);
    }
    public void WInGame()
    {
        DefaultRoom.SetActive(false);
        DefaultClawMachineBox.SetActive(false);

        //Button test
        btnWinGame.gameObject.SetActive(false);
        btnLoseGame.gameObject.SetActive(false);

        clawController.EndGame();
        clawController.IsStart = false;
        ItemController.Instance.EndGame();
        enemyController.EndGame();
        playerController.EndGame();

        MapController.Instance.SetActiveMapStore(true);
        MapManager.Instance.SetActiveRoomVisual(true);

        PlayerMapController.Instance.IsIntoRoom = false;
        PlayerMapController.Instance.IsMoving = false;

        intoRoomTrigger.SetActive(false);
    }

    //Quan ly open room
    public void OpenRoom()
    {
        PlayerMapController.Instance.IsIntoRoom = true;

        StartCoroutine(DelayPlayerMoveInMap(0.5f));
        MapManager.Instance.SetActiveRoomVisual(false);
    }
    public void OpenRoomFight()
    {
        OpenRoom();
        GameController.Instance.StartRoom();
    }
    public void OpenRoomBossFight()
    {
        OpenRoom();
        BossRoom.SetActive(true);
    }
    public void OpenRoomHealing()
    {
        OpenRoom();
        HealingRoom.SetActive(true);
    }
    public void OpenRoomMystery()
    {
        OpenRoom();
        MysteryRoom.SetActive(true);
    }
    public void OpenRoomPachinko()
    {
        OpenRoom();
        PachinkoRoom.SetActive(true);
        UIController.Instance.OpenPachinkoUI(true);
    }
    public void OpenRoomSmith()
    {
        OpenRoom();
        SmithRoom.SetActive(true);
    }
    public void OpenRoomShredder()
    {
        OpenRoom();
        ShredderRoom.SetActive(true);
    }

    IEnumerator DelayPlayerMoveInMap(float time)
    {
        yield return new WaitForSeconds(time);
        MapController.Instance.SetActiveMapStore(false);
    }
}

