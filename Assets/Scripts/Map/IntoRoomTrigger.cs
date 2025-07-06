using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoRoomTrigger : MonoBehaviour
{
    [SerializeField] private string idNameRoom;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckIDRoom(idNameRoom);
            GamePlayController.Instance.IntoRoom = this;
        }
    }

    private void CheckIDRoom(string idRoom)
    {
        switch(idRoom)
        {
            case "boss_fight":
                Debug.Log("Boss FIght");
                GameManager.Instance.OpenRoomBossFight();
                break;
            case "fight":
                Debug.Log("Fight");
                GameManager.Instance.OpenRoomFight();
                break;
            case "gambling":
                Debug.Log("Gambling");
                GameManager.Instance.OpenRoomPachinko();
                break;
            case "hard_fight":
                Debug.Log("Hard FIght");
                GameManager.Instance.OpenRoomFight();
                break;
            case "healing":
                Debug.Log("Healing");
                GameManager.Instance.OpenRoomHealing();
                break;
            case "mystery_machine":
                Debug.Log("Mystery Claw Machine");
                GameManager.Instance.OpenRoomMystery();
                break;
            case "perk":
                Debug.Log("Perk Reward");
                GameManager.Instance.OpenRoomPerkReward();
                break;
            case "shredder":
                Debug.Log("Shredder");
                GameManager.Instance.OpenRoomSmith();
                break;
            case "upgrade":
                Debug.Log("Upgrade Smith");
                GameManager.Instance.OpenRoomShredder();
                break;
        }
    }
}
