using System;
using System.Collections;
using System.Collections.Generic;
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
            GameManager.Instance.IntoRoom = this;
        }
    }

    public void CheckIDRoom(string idRoom)
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

                break;
            case "hard_fight":
                Debug.Log("Hard FIght");

                break;
            case "healing":
                Debug.Log("Healing");

                break;
            case "mystery_machine":
                Debug.Log("Mystery Claw Machine");

                break;
            case "perk":
                Debug.Log("Perk Reward");

                break;
            case "shredder":
                Debug.Log("Shredder");

                break;
            case "upgrade":
                Debug.Log("Upgrade Smith");

                break;
        }
    }
}
