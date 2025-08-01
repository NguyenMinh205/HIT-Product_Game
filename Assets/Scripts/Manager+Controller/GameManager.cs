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
    private GameObject currentMachine;

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
    private GameObject currentUI;

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

    private void OpenRoom(string typeRoom = null)
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            fadeCanvasGroup.alpha = 1f;
        }

        //AudioManager.Instance.PlayMusicInGame();
        PlayerMapController.Instance.IsIntoRoom = true;

        MapController.Instance.SetActiveMapStore(false);
        MapManager.Instance.SetActiveRoomVisual(false);

        uiMap.SetActive(false);
        uiInRoom.SetActive(true);

        if (currentRoom != null) currentRoom.SetActive(true); // Open Room
        if (currentMachine != null) currentMachine.SetActive(true); //Open Machine
        if (currentUI != null) currentUI.SetActive(true);

        if (currentMachine == defaultClawMachineBox) ItemTube.Instance.SetActionBG(true);

        GamePlayController.Instance.PlayerController.NumOfCoinInRoom.text = GamePlayController.Instance.PlayerController.CurPlayerStat.Coin.ToString();

        if (fadeCanvasGroup != null && Camera.main != null)
        {
            Camera.main.orthographicSize = 8f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(fadeCanvasGroup.DOFade(0f, 0.2f).SetEase(Ease.InOutQuad)); 
            sequence.Join(DOVirtual.Float(8f, 6f, 0.2f, value =>
            {
                Camera.main.orthographicSize = value;
            }).SetEase(Ease.InOutQuad));
            sequence.OnComplete(() =>
            {
                fadeCanvasGroup.gameObject.SetActive(false);
                CheckTypeRoom(typeRoom);
            });
        }
    }
    public void CheckTypeRoom(string typeRoom)
    {
        switch(typeRoom)
        {
            case "BossRoom":
                Debug.Log("Start Boss Room");
                GamePlayController.Instance.StartFightRoom(typeRoom);
                break;
            case "FightRoom":
                Debug.Log("Start Fight Room");
                GamePlayController.Instance.StartFightRoom(typeRoom);
                break;
            case "HealingRoom":
                Debug.Log("Start Healing Room");
                GamePlayController.Instance.StartFightRoom(typeRoom);
                break;
            case "MysteryRoom":
                Debug.Log("Start Mystery Room");
                GamePlayController.Instance.StartFightRoom(typeRoom);
                break;

            default:
                break;
        }
    }
    public void OpenRoomFight()
    {
        currentRoom = defaultRoom;
        currentMachine = defaultClawMachineBox;
        currentUI = uiInRoom;

        if(currentMachine != null) BoxBackGroundManager.Instance.SetFightRoom();

        GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
        GamePlayController.Instance.EnemyController.SetPosEnemy(currentRoom, "Fight");

        OpenRoom("FightRoom");
    }

    public void OpenRoomBossFight()
    {
        currentRoom = bossRoom;
        currentMachine = defaultClawMachineBox;
        currentUI = uiInRoom;
        OpenRoom("BossRoom");
        if (currentMachine != null) BoxBackGroundManager.Instance.SetBossRoom();

        GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
        GamePlayController.Instance.EnemyController.SetPosEnemy(currentRoom, "BossRoom");

    }

    public void OpenRoomHealing()
    {
        currentRoom = healingRoom;
        currentMachine = defaultClawMachineBox;
        currentUI = uiInRoom;
        OpenRoom("HealingRoom");
        if (currentMachine != null) BoxBackGroundManager.Instance.SetHealingRoom();
        


        GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
        GamePlayController.Instance.NpcController.SetPosSpawnNPC(currentRoom);

    }

    public void OpenRoomMystery()
    {
        currentRoom = mysteryRoom;
        currentMachine = defaultClawMachineBox;
        currentUI = uiInRoom;
        if (currentMachine != null) BoxBackGroundManager.Instance.SetMysteryRoom();
        
        GamePlayController.Instance.PlayerController.SetPosPlayer(currentRoom);
        GamePlayController.Instance.NpcController.SetPosSpawnNPC(currentRoom);

        OpenRoom("MysteruRoom");
    }

    public void OpenRoomPerkReward()
    {
        OpenRoom();
        tumblerMachineBox.SetActive(true);
        uiTumblerRoom.SetActive(true);
        perkRewardRoom.SetActive(true);
        TumblerMachine.Instance.Init();
        currentRoom = perkRewardRoom;
    }

    public void OpenRoomPachinko()
    {
        OpenRoom();
        pachinkoRoom.SetActive(true);
        uiPachinkoRoom.SetActive(true);
        pachinkoMachineBox.SetActive(true);
        currentRoom = pachinkoRoom;
    }

    public void OpenRoomSmith()
    {
        OpenRoom();
        smithRoom.SetActive(true);
        uiSmithRoom.SetActive(true);
        currentRoom = smithRoom;
        currentRoom.GetComponent<SmithRoomManager>().Init();
    }

    public void OpenRoomShredder()
    {


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
            currentUI.SetActive(false);
            currentMachine.SetActive(false);
            if (currentMachine == defaultClawMachineBox) ItemTube.Instance.SetActionBG(false);

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
        Time.timeScale = 1;
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