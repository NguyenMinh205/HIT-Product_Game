using System.Collections;
using System.Collections.Generic;
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

    [Space]
    [Header("Machine")]
    [SerializeField] private GameObject DefaultClawMachineBox;
    [SerializeField] private GameObject PachinkoMachineBox;
    [SerializeField] private GameObject TumblerMachineBox;

    [Space]
    [Header("UI")]
    [SerializeField] private GameObject uiInRoom; 
    [SerializeField] private GameObject uiMap;
    [SerializeField] private GameObject uiPachinkoRoom;
    [SerializeField] private GameObject uiTumblerRoom;
    [SerializeField] private GameObject uiSmithRoom;
    [SerializeField] private GameObject uiShredderRoom;

    [Header("Test Out Room")]
    [SerializeField] private Button btnOutRoom;

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
    }

    private void OpenRoom()
    {
        CloseAllRoomsAndUIs();
        PlayerMapController.Instance.IsIntoRoom = true;
        StartCoroutine(DelayPlayerMoveInMap(0.5f));
        MapManager.Instance.SetActiveRoomVisual(false);
        uiInRoom.SetActive(true);
        uiMap.SetActive(false);
        btnOutRoom.gameObject.SetActive(true);
    }

    public void OpenRoomFight()
    {
        OpenRoom();
        DefaultRoom.SetActive(true);
        currentRoom = DefaultRoom;
        GameController.Instance.StartRoom();
    }

    public void OpenRoomBossFight()
    {
        OpenRoom();
        BossRoom.SetActive(true);
        currentRoom = BossRoom;
    }

    public void OpenRoomHealing()
    {
        OpenRoom();
        HealingRoom.SetActive(true);
        currentRoom = HealingRoom;
    }

    public void OpenRoomMystery()
    {
        OpenRoom();
        MysteryRoom.SetActive(true);
        currentRoom = MysteryRoom;
    }

    public void OpenRoomPerkReward()
    {
        OpenRoom();
        DefaultRoom.SetActive(true);
        currentRoom = DefaultRoom;
    }

    public void OpenRoomPachinko()
    {
        OpenRoom();
        PachinkoRoom.SetActive(true);
        uiPachinkoRoom.SetActive(true);
        currentRoom = PachinkoRoom;
    }

    public void OpenRoomSmith()
    {
        OpenRoom();
        SmithRoom.SetActive(true);
        uiSmithRoom.SetActive(true);
        currentRoom = SmithRoom;
    }

    public void OpenRoomShredder()
    {
        OpenRoom();
        ShredderRoom.SetActive(true);
        uiShredderRoom.SetActive(true);
        currentRoom = ShredderRoom;
    }

    public void OutRoom()
    {
        if (currentRoom != null)
        {
            btnOutRoom.gameObject.SetActive(false);
            currentRoom.SetActive(false);
            CloseAllRoomsAndUIs();
            MapController.Instance.SetActiveMapStore(true);
            MapManager.Instance.SetActiveRoomVisual(true);
            uiMap.SetActive(true);
            PlayerMapController.Instance.IsIntoRoom = false;
            PlayerMapController.Instance.IsMoving = false;
            if (intoRoomTrigger != null)
            {
                intoRoomTrigger.gameObject.SetActive(false);
            }
            currentRoom = null;
        }
    }

    IEnumerator DelayPlayerMoveInMap(float time)
    {
        yield return new WaitForSeconds(time);
        MapController.Instance.SetActiveMapStore(false);
    }
}