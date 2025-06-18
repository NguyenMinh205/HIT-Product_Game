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
    [Header("IsChange")]
    private bool isChange01;

    [Space]
    [Header("TurnDisplay")]
    [SerializeField] private GameObject uiTurnChange;
    [SerializeField] private TextMeshProUGUI textTurn;


    private void Awake()
    {
        turnGame = TurnPlay.Player;
        isChange01 = false;
    }
    public TurnPlay Turn
    {
        get => this.turnGame;
        set 
        {
            if (this.turnGame != value)
            {
                this.turnGame = value;
                this.isChange01 = true;
                ShowChangeTurn();
            }
            else
            {
                this.isChange01 = false;
            }
            if(value == TurnPlay.Enemy)
            {
                TurnEnemy();
            }
            if(value == TurnPlay.Player)
            {
                ItemController.Instance.Spawn();
            }
        }
    }
    public bool IsChange01
    {
        get => this.isChange01;
        set => this.isChange01 = value;
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
        if(turnGame == TurnPlay.Player)
        {
            TurnPlayer();
        }
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
    }
    public void TurnPlayer()
    {
        Debug.Log(ItemController.Instance.CheckNextTurn() && clawController.checkNextTurn() && clawController.IsStart == true);
        if(ItemController.Instance.CheckNextTurn() && clawController.checkNextTurn() && clawController.IsStart == true)
        {
            Turn = TurnPlay.Enemy;
        }
    }
    public void TurnEnemy()
    {
        enemyController.CheckEnemyToNextTurn();
    }
    public void OutRoom()
    {
        DefaultRoom.SetActive(false);
        DefaultClawMachineBox.SetActive(false);
    }
}

