using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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

    public IntoRoomTrigger IntoRoom
    {
        get => intoRoomTrigger;
        set
        {
            intoRoomTrigger = value;
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
        CloseAllRoomsAndUIs();
        PlayerMapController.Instance.IsIntoRoom = true;
        MapController.Instance.SetActiveMapStore(false);
        MapManager.Instance.SetActiveRoomVisual(false);
        uiMap.SetActive(false);
        uiInRoom.SetActive(true);
        ObserverManager<UIMahcine>.PostEven(UIMahcine.OnUI, true);
    }

    public IEnumerator OpenRoomFight()
    {
        yield return new WaitForSeconds(0.5f);
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
        yield return new WaitForSeconds(0.5f);
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
        yield return new WaitForSeconds(0.5f);
        OpenRoom();
        healingRoom.SetActive(true);
        defaultClawMachineBox.SetActive(true);
        currentRoom = healingRoom;
    }

    public IEnumerator OpenRoomMystery()
    {
        yield return new WaitForSeconds(0.5f);
        OpenRoom();
        mysteryRoom.SetActive(true);
        defaultClawMachineBox.SetActive(true);
        currentRoom = mysteryRoom;
    }

    public IEnumerator OpenRoomPerkReward()
    {
        yield return new WaitForSeconds(0.5f);
        OpenRoom();
        tumblerMachineBox.SetActive(true);
        uiTumblerRoom.SetActive(true);
        perkRewardRoom.SetActive(true);
        currentRoom = perkRewardRoom;
    }

    public IEnumerator OpenRoomPachinko()
    {
        yield return new WaitForSeconds(0.5f);
        OpenRoom();
        pachinkoRoom.SetActive(true);
        uiPachinkoRoom.SetActive(true);
        pachinkoMachineBox.SetActive(true);
        currentRoom = pachinkoRoom;
    }

    public IEnumerator OpenRoomSmith()
    {
        yield return new WaitForSeconds(0.5f);
        OpenRoom();
        SmithRoom.SetActive(true);
        uiSmithRoom.SetActive(true);
        currentRoom = SmithRoom;
        currentRoom.GetComponent<SmithRoomManager>().Init();
    }

    public IEnumerator OpenRoomShredder()
    {
        yield return new WaitForSeconds(0.5f);
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
            CloseAllRoomsAndUIs();
            MapController.Instance.SetActiveMapStore(true);
            MapManager.Instance.SetActiveRoomVisual(true);
            uiMap.SetActive(true);
            numOfCoinTxt.text = GamePlayController.Instance.PlayerController.CurrentPlayer.Stats.Coin.ToString();
            PlayerMapController.Instance.IsIntoRoom = false;
            PlayerMapController.Instance.IsMoving = false;
            if (intoRoomTrigger != null)
            {
                intoRoomTrigger.gameObject.SetActive(false);
            }
            currentRoom = null;
            ObserverManager<UIMahcine>.PostEven(UIMahcine.OnUI, false);
            MapController.Instance.SetRoomVisited(PlayerMapController.Instance.PosInMap);
            ObserverManager<IDMap>.PostEven(IDMap.UpdateHpBar,GamePlayController.Instance.PlayerController.CurrentPlayer);
        }
    }

    public void BackHome()
    {
        if (GameData.Instance.startData.isKeepingPlayGame)
        {
            GameData.Instance.SaveMainGameData();
        }
        else
        {
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