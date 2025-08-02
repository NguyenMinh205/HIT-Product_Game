using System;
using System.Collections;
using System.Collections.Generic;
using TranDuc;
using UnityEngine;

public class IntoRoomTrigger : MonoBehaviour
{
    [SerializeField] private string idNameRoom;
    public string IdNameRoom => idNameRoom;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayEnterRoomSound();
            CheckIDRoom(idNameRoom);
            RoomInGameManager.Instance.IntoRoom = this;
        }
    }

    public void CheckIDRoom(string idRoom)
    {
        switch(idRoom)
        {
            case "boss_fight":
                Debug.Log("Boss FIght");
                RoomInGameManager.Instance.OpenRoomBossFight();
                break;
            case "fight":
                Debug.Log("Fight");
                RoomInGameManager.Instance.OpenRoomFight();
                //StartCoroutine(GameManager.Instance.OpenRoomBossFight());
                //StartCoroutine(GameManager.Instance.OpenRoomMystery());
                break;
            case "gambling":
                Debug.Log("Gambling");
                RoomInGameManager.Instance.OpenRoomPachinko();
                break;
            case "hard_fight":
                Debug.Log("Hard FIght");
                RoomInGameManager.Instance.OpenRoomFight();
                break;
            case "healing":
                Debug.Log("Healing");
                RoomInGameManager.Instance.OpenRoomHealing();
                break;
            case "mystery_machine":
                Debug.Log("Mystery Claw Machine");
                RoomInGameManager.Instance.OpenRoomMystery();
                break;
            case "perk":
                Debug.Log("Perk Reward");
                RoomInGameManager.Instance.OpenRoomPerkReward();
                break;
            case "shredder":
                Debug.Log("Shredder");
                RoomInGameManager.Instance.OpenRoomShredder();
                break;
            case "upgrade":
                Debug.Log("Upgrade Smith");
                RoomInGameManager.Instance.OpenRoomSmith();
                break;
        }
    }
}
