using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    [Space]
    [Header("Room")]
    [SerializeField] private GameObject healingRoom;
    [SerializeField] private GameObject mysteryRoom;
    [SerializeField] private GameObject pachinkoRoom;
    [SerializeField] private GameObject smithRoom;
    public GameObject SmithRoom => smithRoom;
    [SerializeField] private GameObject shredderRoom;
    [SerializeField] private GameObject bossRoom;
    [SerializeField] private GameObject defaultRoom;
    [SerializeField] private GameObject perkRewardRoom;
    public GameObject PerkRewardRoom => perkRewardRoom;
    private IntoRoomTrigger intoRoomTrigger;
    private GameObject currentRoom;
    public GameObject CurrentRoom => currentRoom;

    [Space]
    [Header("Machine")]
    [SerializeField] private GameObject defaultClawMachineBox;
    [SerializeField] private GameObject pachinkoMachineBox;
    [SerializeField] private GameObject tumblerMachineBox;

    [Space]
    [Header("UI")]
    [SerializeField] private GameObject uiInRoom;
    [SerializeField] private GameObject uiMap;
    [SerializeField] private GameObject uiPachinkoRoom;
    [SerializeField] private GameObject uiTumblerRoom;
    [SerializeField] private GameObject uiSmithRoom;
    [SerializeField] private GameObject uiShredderRoom;
    [SerializeField] private GameObject rewardUI;
    [SerializeField] private GameObject finishUI;
    [SerializeField] private TextMeshProUGUI numOfCoinTxt;
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    [Space]
    [Header("Button")] // 
    [SerializeField] private Button btnRoll;

    private bool isFinishGame = false;
    public bool IsFinishGame
    {
        get => isFinishGame;
        set
        {
            isFinishGame = value;
        }
    }
    public GameObject RewardUI => rewardUI;
    public GameObject FinishUI => finishUI;
    public GameObject BtnRoll => btnRoll.gameObject;
    public IntoRoomTrigger IntoRoom
    {
        get => intoRoomTrigger;
        set
        {
            intoRoomTrigger = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        AudioManager.Instance.PlayMusicSelectRoom();
        isFinishGame = false;
        if (GameData.Instance.startData.isKeepingPlayGame)
        {
            GameData.Instance.LoadMainGameData();
        }
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.gameObject.SetActive(true);
        }
    }

    private void CloseAllRoomsAndUIs()
    {
        healingRoom.SetActive(false);
        mysteryRoom.SetActive(false);
        pachinkoRoom.SetActive(false);
        smithRoom.SetActive(false);
        shredderRoom.SetActive(false);
        bossRoom.SetActive(false);
        defaultRoom.SetActive(false);

        uiInRoom.SetActive(false);
        uiPachinkoRoom.SetActive(false);
        uiTumblerRoom.SetActive(false);
        uiSmithRoom.SetActive(false);
        uiShredderRoom.SetActive(false);

        defaultClawMachineBox.SetActive(false);
        pachinkoMachineBox.SetActive(false);
        tumblerMachineBox.SetActive(false);
    }

    private void OpenRoom()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            fadeCanvasGroup.alpha = 1f;
        }

        CloseAllRoomsAndUIs();
        AudioManager.Instance.PlayMusicInGame();
        PlayerMapController.Instance.IsIntoRoom = true;
        MapController.Instance.SetActiveMapStore(false);
        MapManager.Instance.SetActiveRoomVisual(false);
        uiMap.SetActive(false);
        uiInRoom.SetActive(true);
        GamePlayController.Instance.PlayerController.NumOfCoinInRoom.text = GamePlayController.Instance.PlayerController.CurPlayerStat.Coin.ToString();

        if (fadeCanvasGroup != null && Camera.main != null)
        {
            Camera.main.orthographicSize = 8f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(fadeCanvasGroup.DOFade(0f, 0.5f).SetEase(Ease.InOutQuad)); 
            sequence.Join(DOVirtual.Float(8f, 6f, 0.5f, value =>
            {
                Camera.main.orthographicSize = value;
            }).SetEase(Ease.InOutQuad));
            sequence.OnComplete(() =>
            {
                fadeCanvasGroup.gameObject.SetActive(false);
            });
        }
    }

    public IEnumerator OpenRoomFight()
    {
        yield return new WaitForSeconds(0.25f);
        OpenRoom();
        defaultRoom.SetActive(true);
        defaultClawMachineBox.SetActive(true);
        ObserverManager<IDBackGroundBoxMachine>.PostEven(IDBackGroundBoxMachine.FightNormal);
        ObserverManager<IDBasketBackGround>.PostEven(IDBasketBackGround.FightNormal);
        ObserverManager<IDMoveBackGround>.PostEven(IDMoveBackGround.FightNormal);
        currentRoom = defaultRoom;
        GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
        GamePlayController.Instance.EnemyController.SetPosEnemy(currentRoom, "FightRoom");
        GamePlayController.Instance.StartFightRoom("FightRoom");
    }

    public IEnumerator OpenRoomBossFight()
    {
        yield return new WaitForSeconds(0.25f);
        OpenRoom();
        bossRoom.SetActive(true);
        defaultClawMachineBox.SetActive(true);
        ObserverManager<IDBackGroundBoxMachine>.PostEven(IDBackGroundBoxMachine.FightBoss);
        ObserverManager<IDBasketBackGround>.PostEven(IDBasketBackGround.FightBoss);
        ObserverManager<IDMoveBackGround>.PostEven(IDMoveBackGround.FightBoss);
        currentRoom = bossRoom;
        GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
        GamePlayController.Instance.EnemyController.SetPosEnemy(currentRoom, "BossRoom");
        GamePlayController.Instance.StartFightRoom("BossRoom");
    }

    public IEnumerator OpenRoomHealing()
    {
        yield return new WaitForSeconds(0.25f);
        OpenRoom();
        healingRoom.SetActive(true);
        defaultClawMachineBox.SetActive(true);
        ObserverManager<IDBackGroundBoxMachine>.PostEven(IDBackGroundBoxMachine.Healing);
        ObserverManager<IDBasketBackGround>.PostEven(IDBasketBackGround.Healing);
        ObserverManager<IDMoveBackGround>.PostEven(IDMoveBackGround.Healing);
        currentRoom = healingRoom;
        GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
        GamePlayController.Instance.NpcController.SetPosSpawnNPC(currentRoom);
        GamePlayController.Instance.StartFunctionRoom("HealingRoom");
    }

    public IEnumerator OpenRoomMystery()
    {
        yield return new WaitForSeconds(0.25f);
        OpenRoom();
        mysteryRoom.SetActive(true);
        defaultClawMachineBox.SetActive(true);
        ObserverManager<IDBackGroundBoxMachine>.PostEven(IDBackGroundBoxMachine.Mystery);
        ObserverManager<IDBasketBackGround>.PostEven(IDBasketBackGround.Mystery);
        ObserverManager<IDMoveBackGround>.PostEven(IDMoveBackGround.Mystery);
        currentRoom = mysteryRoom;
        GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
        GamePlayController.Instance.NpcController.SetPosSpawnNPC(currentRoom);
        GamePlayController.Instance.StartFunctionRoom("MysteryRoom");
    }

    public IEnumerator OpenRoomPerkReward()
    {
        yield return new WaitForSeconds(0.25f);
        OpenRoom();
        tumblerMachineBox.SetActive(true);
        uiTumblerRoom.SetActive(true);
        perkRewardRoom.SetActive(true);
        TumblerMachine.Instance.Init();
        currentRoom = perkRewardRoom;
    }

    public IEnumerator OpenRoomPachinko()
    {
        yield return new WaitForSeconds(0.25f);
        OpenRoom();
        pachinkoRoom.SetActive(true);
        uiPachinkoRoom.SetActive(true);
        pachinkoMachineBox.SetActive(true);
        currentRoom = pachinkoRoom;
    }

    public IEnumerator OpenRoomSmith()
    {
        yield return new WaitForSeconds(0.25f);
        OpenRoom();
        smithRoom.SetActive(true);
        uiSmithRoom.SetActive(true);
        currentRoom = smithRoom;
        currentRoom.GetComponent<SmithRoomManager>().Init();
    }

    public IEnumerator OpenRoomShredder()
    {
        yield return new WaitForSeconds(0.25f);
        OpenRoom();
        shredderRoom.SetActive(true);
        uiShredderRoom.SetActive(true);
        currentRoom = shredderRoom;
        currentRoom.GetComponent<ShredderRoomManager>().Init();
    }

    public void OutRoom()
    {
        if (currentRoom != null)
        {
            currentRoom.SetActive(false);
            uiInRoom.SetActive(false);
            CloseAllRoomsAndUIs();
            MapController.Instance.SetActiveMapStore(true);
            MapManager.Instance.SetActiveRoomVisual(true);
            uiMap.SetActive(true);
            PlayerMapController.Instance.IsIntoRoom = false;
            PlayerMapController.Instance.IsMoving = false;
            if (intoRoomTrigger != null)
            {
                Debug.LogError("Out Room: " + intoRoomTrigger.IdNameRoom);
                intoRoomTrigger.gameObject.SetActive(false);
            }
            currentRoom = null;
            AudioManager.Instance.PlayMusicSelectRoom();
            MapController.Instance.SetRoomVisited(PlayerMapController.Instance.PosInMap);
            ObserverManager<IDMap>.PostEven(IDMap.UpdateHpBar, GamePlayController.Instance.PlayerController.CurrentPlayer);
            DOVirtual.DelayedCall(0.2f, () =>
            {
                numOfCoinTxt.text = GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.Coin.ToString();
            });
        }
    }

    public void BackHome()
    {
        if (GamePlayController.Instance.IsLoseGame)
        {
            isFinishGame = true;
            GameData.Instance.startData.isKeepingPlayGame = false;
            GameData.Instance.SaveStartGameData();
            GameData.Instance.ClearMainGameData();
            GamePlayController.Instance.IsLoseGame = false;
            SceneManager.LoadScene(0);
            return;
        }
        if (!isFinishGame)
        {
            AudioManager.Instance.PlaySoundClickButton();
            GameData.Instance.startData.isKeepingPlayGame = true;
            GameData.Instance.SaveMainGameData();
        }
        else
        {
            AudioManager.Instance.PlaySoundClickButton();
            GameData.Instance.startData.coin += GameData.Instance.mainGameData.playerData.stats.Coin;
            GameData.Instance.SaveStartGameData();
            GameData.Instance.ClearMainGameData();
        }
        SceneManager.LoadScene(0);
    }

    private void OnApplicationQuit()
    {
        if (!isFinishGame)
        {
            GameData.Instance.startData.isKeepingPlayGame = true;
            GameData.Instance.SaveStartGameData();
            if (intoRoomTrigger != null)
            {
                GamePlayController.Instance.PlayerController.CurPlayerStat.ResetStatAfterRound();
                GameData.Instance.mainGameData.playerData.stats = GamePlayController.Instance.PlayerController.CurPlayerStat;
            }
            GameData.Instance.SaveMainGameData();
            Debug.LogError("Application is quitting, saving game data.");
        }
    }
}