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
            GameController.Instance.IntoRoom = this;
        }
    }

    private void CheckIDRoom(string idRoom)
    {
        switch(idRoom)
        {
            case "boss_fight":
                Debug.Log("Boss FIght");
                break;
            case "fight":
                OpenFightRoom();
                Debug.Log("FIght");
                break;
            case "gambling":
                Debug.Log("Gambling");
                break;
            case "hard_fight":
                Debug.Log("Hard FIght");
                OpenBossFightRoom();
                break;
            case "healing":
                Debug.Log("Healing");
                OpenRoomHealing();
                break;
            case "mystery_machine":
                Debug.Log("Mystery Claw Machine");
                OpenRoomMystery();
                break;
            case "perk":
                Debug.Log("Perk Reward");
                break;
            case "shredder":
                Debug.Log("Shredder");
                OpenRoomShredder();
                break;
            case "upgrade":
                Debug.Log("Upgrade Smith");
                OpenRoomSmith();
                break;
        }
    }
    public void SetActive(bool var)
    {
        gameObject.SetActive(var);
    }

    public void OpenBossFightRoom()
    {
        GameController.Instance.OpenRoomBossFight();
    }
    public void OpenFightRoom()
    {
        GameController.Instance.OpenRoomFight();
    }
    public void OpenRoomHealing()
    {
        GameController.Instance.OpenRoomHealing();
    }
    public void OpenRoomMystery()
    {
        GameController.Instance.OpenRoomMystery();
    }
    public void OpenRoomPachinko()
    {
        GameController.Instance.OpenRoomPachinko();
    }
    public void OpenRoomSmith()
    {
        GameController.Instance.OpenRoomSmith();
    }
    public void OpenRoomShredder()
    {
        GameController.Instance.OpenRoomShredder();
    }
}
