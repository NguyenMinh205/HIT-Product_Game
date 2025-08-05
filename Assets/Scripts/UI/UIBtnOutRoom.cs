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
        MapSystem.Instance.SetActiveRoomVisual(true);
        intoRoomTrigger.gameObject.SetActive(false);
    }
}
