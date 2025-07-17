using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : Singleton<GameManager>
{
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
    [SerializeField] private TextMeshProUGUI numOfCoinTxt;

    public GameObject RewardUI => rewardUI;
    public IntoRoomTrigger IntoRoom
    {
        get => intoRoomTrigger;
        set => intoRoomTrigger = value;
    }

    private void CloseAllRoomsAndUIs()
    {
        HealingRoom.SetActive(false);
        MysteryRoom.SetActive(false);
        PachinkoRoom.SetActive(false);
        SmithRoom.SetActive(false);
        ShredderRoom.SetActive(false);
        BossRoom.SetActive(false);
        DefaultRoom.SetActive(false);

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
    }

    public IEnumerator OpenRoomFight()
    {
        yield return new WaitForSeconds(0.5f);
        OpenRoom();
        DefaultRoom.SetActive(true);
        defaultClawMachineBox.SetActive(true);
        currentRoom = DefaultRoom;
        GamePlayController.Instance.StartFightRoom();
    }

    public IEnumerator OpenRoomBossFight()
    {
        yield return new WaitForSeconds(0.5f);
        OpenRoom();
        BossRoom.SetActive(true);
        defaultClawMachineBox.SetActive(true);
        currentRoom = BossRoom;
    }

    public IEnumerator OpenRoomHealing()
    {
        yield return new WaitForSeconds(0.5f);
        OpenRoom();
        HealingRoom.SetActive(true);
        defaultClawMachineBox.SetActive(true);
        currentRoom = HealingRoom;
    }

    public IEnumerator OpenRoomMystery()
    {
        yield return new WaitForSeconds(0.5f);
        OpenRoom();
        MysteryRoom.SetActive(true);
        defaultClawMachineBox.SetActive(true);
        currentRoom = MysteryRoom;
    }

    public IEnumerator OpenRoomPerkReward()
    {
        yield return new WaitForSeconds(0.5f);
        OpenRoom();
        tumblerMachineBox.SetActive(true);
        uiTumblerRoom.SetActive(true);
        DefaultRoom.SetActive(true);
        currentRoom = DefaultRoom;
    }

    public IEnumerator OpenRoomPachinko()
    {
        yield return new WaitForSeconds(0.5f);
        OpenRoom();
        PachinkoRoom.SetActive(true);
        uiPachinkoRoom.SetActive(true);
        pachinkoMachineBox.SetActive(true);
        currentRoom = PachinkoRoom;
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
        ShredderRoom.SetActive(true);
        uiShredderRoom.SetActive(true);
        currentRoom = ShredderRoom;
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
            ObserverManager<IDMap>.PostEven(IDMap.UpdateHpBar,GamePlayController.Instance.PlayerController.CurrentPlayer);
        }
    }

}