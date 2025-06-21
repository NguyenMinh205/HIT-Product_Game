using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBtnOutRoom : MonoBehaviour
{
    public GameObject room;
    public IntoRoomTrigger intoRoomTrigger;
    [SerializeField] private Button btnOutRoom;

    private void Awake()
    {
        btnOutRoom.onClick.AddListener(delegate
        {
            OutRoom();
        });
    }
    public void OutRoom()
    {
        btnOutRoom.gameObject.SetActive(true);

        MapController.Instance.SetActiveMapStore(true);
        MapManager.Instance.SetActiveRoomVisual(true);

        PlayerMapController.Instance.IsIntoRoom = false;
        PlayerMapController.Instance.IsMoving = false;

        intoRoomTrigger.SetActive(false);
    }
}
