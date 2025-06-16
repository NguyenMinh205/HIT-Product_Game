using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Player playerPrefabs;
    [SerializeField] private Transform playerParent;
    [SerializeField] private Transform posSpawnPlayer;

    private Player currentPlayer;

    public Vector3 PosPlayer
    {
        get => currentPlayer.transform.position;
    }
    public void SpawnPlayer()
    {
        currentPlayer = PoolingManager.Spawn(playerPrefabs, posSpawnPlayer.position, Quaternion.identity);
        HpController.Instance.showHp(0);
    }

}
