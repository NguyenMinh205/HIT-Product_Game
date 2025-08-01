using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public enum EventID
{
    OnStartRound,
    OnStartPlayerTurn,
    OnTakeDamage,
    OnTakeCoin,
    OnReceiverDamage,
    OnDealDamage,
    OnGoldChanged,
    OnHealthChanged,
    OnStartEnemyTurn,
    OnUseClaw,
    OnBasketEmpty,
    OnClawsEmpty,
    OnEndRound,
    OnEndEnemyTurn,
    OnEnemyDead,

}
public enum TurnPlay
{
    Player,
    Enemy,
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

    [SerializeField] private NPCController npcController;   
    public NPCController NpcController => npcController;
    private IntoRoomTrigger intoRoomTrigger;

    [Space]
    [Header("TurnDisplay")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI textTurn;

    [Space]
    [Header("CheckTurn")]

    private bool isEndGame = false;
    private bool isNotFight = false;
    private bool isMysteryRoom = false;
    public bool IsEndGame => isEndGame;
    public bool isLoseGame = false;
    public bool IsLoseGame { get; set; }

    private Vector2Int directionPlayer;
    [SerializeField] private Inventory inventoryInHealingRoom;
    [SerializeField] private Inventory inventoryInMysteryRoom;

    public Vector2Int Dir
    {
        set => directionPlayer = value;
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
        if (isEndGame) return;

        ChangeTurn(newTurn);

        switch (newTurn)
        {
            case TurnPlay.Enemy:
                if (IsEndGame) return;
                TurnEnemy();
                break;

            case TurnPlay.Player:
                if (IsEndGame) return;
                TurnPlayer();
                break;
        }
    }

    private void ChangeTurn(TurnPlay turn)
    {
        textTurn.text = turnGame == TurnPlay.Player ? "Your Turn" : "Enemy Turn";

        canvasGroup.alpha = 0f;

        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(1f, 0.4f).SetEase(Ease.OutQuad));
        seq.AppendInterval(0.5f); // chờ 0.5s
        seq.Append(canvasGroup.DOFade(0f, 0.4f).SetEase(Ease.InQuad));
        seq.OnComplete(() =>
        {
            turnGame = turn;
        });
    }
    public void CheckTurnPlayer()
    {
        if(clawController.IsListClawNull && ItemTube.Instance.IsItemNull)
        {
            Turn = TurnPlay.Enemy;
        }
    }

    //Start Room

    //Spawn Enemy Or NPC
    public void SpawnEnemyOrNPC(string typeRoom)
    {
        switch(typeRoom)
        {
            case "BossRoom":
                Debug.Log("Start Boss Room");
                enemyController.SpawnBoss();
                break;
            case "FightRoom":
                Debug.Log("Start Fight Room");
                enemyController.Spawn();
                break;
            case "HealingRoom":
                Debug.Log("Start Healing Room");
                npcController.SpawnNPC("healingRoom");
                break;
            case "MysteryRoom":
                Debug.Log("Start Mystery Room");
                npcController.SpawnNPC("mysteryRoom");
                break;

            default:
                break;
        }
    }
    public void SpawnItemStartInRoom(string typeRoom)
    {
        switch (typeRoom)
        {
            case "BossRoom":
                Debug.Log("Spawn Item Normal");
                itemController.Spawn(playerController.CurrentPlayer.Inventory);
                break;
            case "FightRoom":
                Debug.Log("Spawn Item Normal");
                itemController.Spawn(playerController.CurrentPlayer.Inventory);
                break;
            case "HealingRoom":
                Debug.Log("Spawn Item In Healing Room");
                itemController.Spawn(inventoryInHealingRoom);
                break;
            case "MysteryRoom":
                Debug.Log("Spawn Item In Mystery Room");
                itemController.Spawn(inventoryInMysteryRoom);
                break;

            default:
                break;
        }
    }
    public void StartFightRoom(string typeRoom)
    {
        isEndGame = false;                          //Tat Check End Game

        SpawnEnemyOrNPC(typeRoom);                  //Set Enemy
        playerController.SpawnPlayer();             //Set Player
        SpawnItemStartInRoom(typeRoom);             // Set Item sau Player

        if(clawController != null)                  //Set Claw
        {
            clawController.Spawn();
            clawController.IsStart = true;
            clawController.SetCurrentClaw();
        }
        ChangeTurn(TurnPlay.Player);
    }


    public void TurnPlayer()
    {
        playerController.ResetShield();
        clawController.ResetMachineClaw();
        playerController.CurrentPlayer.AddItem();
        itemController.SpawnItem(playerController.CurrentPlayer.AddedItems);
        enemyController.SetActionEnemyNext();
    }

    public void TurnEnemy()
    {
        StartCoroutine(enemyController.EnemyAction());
    }







    public void LoseGame()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayDefeatSound();
        isEndGame = true;
        clawController.EndGame();
        clawController.IsStart = false;
        enemyController.EndGame();
        playerController.EndGame();
        itemController.EndGame();
        isLoseGame = true;

        GameData.Instance.startData.isKeepingPlayGame = false;
        GameData.Instance.SaveStartGameData();
        GameManager.Instance.BackHome();
    }




    public void WinGame()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayVictorySound();
        isEndGame = true;
        clawController.EndGame();
        clawController.IsStart = false;
        enemyController.EndGame();
        playerController.EndGame();
        itemController.EndGame();

        int bonusGold = 3 + MapManager.Instance.MapIndex;
        playerController.CurrentPlayer.Stats.ChangeCoin(bonusGold);
        playerController.SavePlayerData();

        GameManager.Instance.RewardUI.SetActive(true);
        RewardManager.Instance.InitReward();
    }

    public void EndGame()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayVictorySound();
        isEndGame = true;
        clawController.EndGame();
        clawController.IsStart = false;
        npcController.EndGame();
        playerController.EndGame();
        itemController.EndGame();

        int bonusGold = 3 + MapManager.Instance.MapIndex;
        //playerController.CurrentPlayer.Stats.ChangeCoin(bonusGold);
        playerController.SavePlayerData();

        if(isMysteryRoom)
        {
            isMysteryRoom = false;
            ObserverManager<IDMysteryRoom>.PostEven(IDMysteryRoom.CallReward);
        }
        else
        {
            GameManager.Instance.OutRoom();
        }
    }
}