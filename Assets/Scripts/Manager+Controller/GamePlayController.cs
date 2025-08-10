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
    OnStartListEnemyTurn,
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
    private string typeRoom;

    private bool isEndGame = false;
    private bool isNotFight = false;
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

    }

    private void ChangeTurn(TurnPlay turn)
    {
        textTurn.text = turnGame == TurnPlay.Player ? "Enemy Turn" : "Your Turn";

        canvasGroup.alpha = 0f;

        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(1f, 0.35f).SetEase(Ease.OutQuad));
        seq.AppendInterval(0.5f);
        seq.Append(canvasGroup.DOFade(0f, 0.3f).SetEase(Ease.InQuad));
        seq.OnComplete(() =>
        {
            turnGame = turn;
            switch (turnGame)
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
        });
    }
    public void StartTurnPlayer()
    {
        textTurn.text ="Your Turn";

        canvasGroup.alpha = 0f;

        Sequence seq = DOTween.Sequence();
        seq.Append(canvasGroup.DOFade(1f, 0.35f).SetEase(Ease.OutQuad));
        seq.AppendInterval(0.5f);
        seq.Append(canvasGroup.DOFade(0f, 0.3f).SetEase(Ease.InQuad));
        seq.OnComplete(() =>
        {
            clawController.IsStart = true;
            clawController.SetCurrentClaw();
            if (DataManager.Instance.GameData.IsFirstTimePlayGame)
            {
                DataManager.Instance.GameData.IsFirstTimePlayGame = false;
                DataManager.Instance.GameData.Save();
                ControllerUIInGame.Instance.ShowTutorial();
            }

        });
    }
    public void CheckTurnPlayer()
    {

        if(clawController.IsListClawNull && ItemTube.Instance.IsItemNull)
        {
            if (typeRoom == "HealingRoom" || typeRoom == "MysteryRoom")
            {
                EndGame();
                return;
            }

            Turn = TurnPlay.Enemy;
        }
    }

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
        isEndGame = false;
        this.typeRoom = typeRoom;

        SpawnEnemyOrNPC(typeRoom);
        playerController.SpawnPlayer();
        SpawnItemStartInRoom(typeRoom);

        if(clawController != null)
        {
            if(typeRoom == "HealingRoom" || typeRoom =="MysteryRoom")
                clawController.Spawn(GamePlayController.Instance.PlayerController.CurPlayerStat.ClawInGrannyRoom);
            else clawController.Spawn();

            StartTurnPlayer();
        }
    }

    public void TurnPlayer()
    {
        playerController.ResetShield();
        clawController.ResetMachineClaw();
        playerController.CurrentPlayer.AddItem();
        itemController.SpawnItem(playerController.CurrentPlayer.AddedItems);
        enemyController.SetActionEnemyNext();
        ObserverManager<EventID>.PostEven(EventID.OnStartPlayerTurn);
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

        DataManager.Instance.GameData.SetKeepPlayState(false);
        RoomInGameManager.Instance.BackHome();
    }

    public void WinGame()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayVictorySound();

        isEndGame = true;

        clawController.EndGame();
        clawController.IsStart = false;

        itemController.EndGame();

        playerController.EndGame();

        enemyController.EndGame();


        int bonusGold = 3 + MapSystem.Instance.MapIndex;
        playerController.CurrentPlayer.Stats.ChangeCoin(bonusGold);
        playerController.SavePlayerData();

        if(typeRoom == "BossRoom")
        {
            DataManager.Instance.GameData.IndexBoss++;
        }

        ControllerUIInGame.Instance.RewardUI.SetActive(true);
        RewardManager.Instance.InitReward();

        if (ControllerUIInGame.Instance.TutorialUI.activeSelf && !DataManager.Instance.GameData.IsFirstTimePlayGame)
        {
            ControllerUIInGame.Instance.CloseTutorial();
        }
    }

    public void EndGame()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayVictorySound();
        isEndGame = true;


        clawController.EndGame();
        clawController.IsStart = false;

        itemController.EndGame();
        playerController.EndGame();

        npcController.EndGame();

        playerController.SavePlayerData();

        if(typeRoom == "MysteryRoom")
        {
            ObserverManager<IDMysteryRoom>.PostEven(IDMysteryRoom.CallReward);
        }
        else
        {
            RoomInGameManager.Instance.OutRoom();
        }
    }
}