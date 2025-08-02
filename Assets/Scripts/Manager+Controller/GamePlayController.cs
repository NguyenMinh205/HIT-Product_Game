using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using TranDuc;

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

    [SerializeField] private NPCController npcController;   
    public NPCController NpcController => npcController;
    private IntoRoomTrigger intoRoomTrigger;

    [Space]
    [Header("TurnDisplay")]
    [SerializeField] private GameObject uiTurnChange;
    [SerializeField] private TextMeshProUGUI textTurn;
    private CanvasGroup uiTurnCanvasGroup;

    [Space]
    [Header("CheckTurn")]
    public bool isCheckTurnByClaw;
    public bool isCheckTurnByItem;
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

    public IntoRoomTrigger IntoRoom
    {
        set => intoRoomTrigger = value;
    }
    private void Start()
    {
        if (uiTurnChange != null)
        {
            uiTurnCanvasGroup = uiTurnChange.GetComponent<CanvasGroup>();
        }
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
            if(isNotFight)
            {
                isNotFight = false;
                EndGame();
                return;
            }
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
        if (isEndGame) return;
        turnGame = newTurn;
        ShowChangeTurn(newTurn);
    }
    private void SwitchTurn(TurnPlay newTurn)
    {
        switch (newTurn)
        {
            case TurnPlay.Enemy:
                if (IsEndGame) return;
                TurnEnemy();
                break;

            case TurnPlay.Player:
                if (IsEndGame) return;
                playerController.ResetShield();
                clawController.ResetMachineClaw();
                StartPlayerTurn();
                break;
        }
    }

    private void ShowChangeTurn(TurnPlay turn)
    {
        if (uiTurnCanvasGroup == null || textTurn == null) return;
        textTurn.text = turnGame == TurnPlay.Player ? "Your Turn" : "Enemy Turn";
        uiTurnCanvasGroup.alpha = 0f;
        uiTurnCanvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            uiTurnCanvasGroup.DOFade(0f, 0.5f).SetEase(Ease.InQuad).OnComplete(delegate
            {
                DOVirtual.DelayedCall(0.25f, () => SwitchTurn(turn));
            }).SetDelay(1f)
        );
    }


    public void StartPlayerTurn()
    {
        ObserverManager<EventID>.PostEven(EventID.OnStartPlayerTurn);
        playerController.CurrentPlayer.AddItem();
        itemController.SpawnAdditionalItems();
        itemController.IsPickupItemSuccess = false;
        Debug.Log("Player Turn Started");
    }

    public void TurnEnemy()
    {
        enemyController.ResetShield();
        isCheckTurnByClaw = false;
        isCheckTurnByItem = false;
        StartCoroutine(enemyController.CheckEnemyToNextTurn());
    }

    public void StartFightRoom(string typeFight)
    {
        isEndGame = false;

        if (typeFight == "BossRoom")
        {
            enemyController.SpawnBoss();
        }
        else
        {
            enemyController.Spawn();
        }

        playerController.SpawnPlayer();
        clawController.Spawn();
        itemController.Spawn(playerController.CurrentPlayer.Inventory);

        DOVirtual.DelayedCall(0.5f, () =>
        {
            if(clawController != null)
            {
                clawController.IsStart = true;
                clawController.StartClaw();
            }
            
        });

        Turn = TurnPlay.Player;
        isCheckTurnByClaw = false;
        isCheckTurnByItem = false;
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnBasketEmpty, HandleBasketEmpty);
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnClawsEmpty, HandleClawsEmpty);
        itemController.IsPickupItemSuccess = false;
    }

    public void StartFunctionRoom(string typeRoom)
    {
        isEndGame = false;
        if (typeRoom =="HealingRoom")
        {
            itemController.Spawn(inventoryInHealingRoom);
            npcController.SpawnNPC("healingRoom");
        }
        else if(typeRoom == "MysteryRoom")
        {
            itemController.Spawn(inventoryInMysteryRoom);
            npcController.SpawnNPC("mysteryRoom");
            isMysteryRoom = true;
        }
        isNotFight = true;
        playerController.SpawnPlayer();

        clawController.Spawn(1);

        DOVirtual.DelayedCall(0.5f, () =>
        {
            clawController.IsStart = true;
            clawController.StartClaw();
        });

        clawController.IsStart = true;
        clawController.StartClaw();
        Turn = TurnPlay.Player;
        isCheckTurnByClaw = false;
        isCheckTurnByItem = false;
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnBasketEmpty, HandleBasketEmpty);
        ObserverManager<EventID>.AddDesgisterEvent(EventID.OnClawsEmpty, HandleClawsEmpty);
        itemController.IsPickupItemSuccess = false;
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

        ObserverManager<EventID>.RemoveAddListener(EventID.OnBasketEmpty, HandleBasketEmpty);
        ObserverManager<EventID>.RemoveAddListener(EventID.OnClawsEmpty, HandleClawsEmpty);
        RoomInGameManager.Instance.BackHome();
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

        int bonusGold = 3 + MapSystem.Instance.MapIndex;
        playerController.CurrentPlayer.Stats.ChangeCoin(bonusGold);
        playerController.SavePlayerData();

        ObserverManager<EventID>.RemoveAddListener(EventID.OnBasketEmpty, HandleBasketEmpty);
        ObserverManager<EventID>.RemoveAddListener(EventID.OnClawsEmpty, HandleClawsEmpty);
        ControlerUIInGame.Instance.RewardUI.SetActive(true);
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

        int bonusGold = 3 + MapSystem.Instance.MapIndex;
        //playerController.CurrentPlayer.Stats.ChangeCoin(bonusGold);
        playerController.SavePlayerData();

        ObserverManager<EventID>.RemoveAddListener(EventID.OnBasketEmpty, HandleBasketEmpty);
        ObserverManager<EventID>.RemoveAddListener(EventID.OnClawsEmpty, HandleClawsEmpty);

        if(isMysteryRoom)
        {
            isMysteryRoom = false;
            ObserverManager<IDMysteryRoom>.PostEven(IDMysteryRoom.CallReward);
        }
        else
        {
            RoomInGameManager.Instance.OutRoom();
        }
    }
}