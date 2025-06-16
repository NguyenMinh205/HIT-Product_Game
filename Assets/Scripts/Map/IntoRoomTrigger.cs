using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoRoomTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMapController.Instance.IsIntoRoom = true;

            MapController.Instance.SetActiveMapStore(false);
            MapManager.Instance.SetActiveRoomVisual(false);

            GameController.Instance.StartRoom();
        }
    }
}
