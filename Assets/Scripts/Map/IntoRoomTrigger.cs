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
                StartCoroutine(GameManager.Instance.OpenRoomBossFight());
                break;
            case "fight":
                Debug.Log("Fight");
                StartCoroutine(GameManager.Instance.OpenRoomFight());
                //StartCoroutine(GameManager.Instance.OpenRoomBossFight());
                break;
            case "gambling":
                Debug.Log("Gambling");
                StartCoroutine(GameManager.Instance.OpenRoomPachinko());
                break;
            case "hard_fight":
                Debug.Log("Hard FIght");
                StartCoroutine(GameManager.Instance.OpenRoomFight());
                break;
            case "healing":
                Debug.Log("Healing");
                StartCoroutine(GameManager.Instance.OpenRoomHealing());
                break;
            case "mystery_machine":
                Debug.Log("Mystery Claw Machine");
                StartCoroutine(GameManager.Instance.OpenRoomMystery());
                break;
            case "perk":
                Debug.Log("Perk Reward");
                StartCoroutine(GameManager.Instance.OpenRoomPerkReward());
                break;
            case "shredder":
                Debug.Log("Shredder");
                StartCoroutine(GameManager.Instance.OpenRoomShredder());
                break;
            case "upgrade":
                Debug.Log("Upgrade Smith");
                StartCoroutine(GameManager.Instance.OpenRoomSmith());
                break;
        }
    }
}
