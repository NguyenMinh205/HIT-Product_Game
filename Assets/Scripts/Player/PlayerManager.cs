using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Player playerPrefabs;
    [SerializeField] private Transform playerParent;
    [SerializeField] private Transform posSpawnPlayer;
    [SerializeField] private float distancePlayerAndHealthBar;

    private Player currentPlayer;

    public Player CurrentPlayer
    {
        get => currentPlayer;
    }

    public Vector3 PosPlayer
    {
        get => currentPlayer.transform.position;
    }
    public void SpawnPlayer()
    {
        currentPlayer = PoolingManager.Spawn(playerPrefabs, posSpawnPlayer.position, Quaternion.identity);
        UIHealthBarController.Instance.InitHealthBarToObjectBase(currentPlayer);
        CalulationPositionPlayer(posSpawnPlayer.position);
    }
    public void CalulationPositionPlayer(Vector3 posSpawnPlayer)
    {
        Sprite sprite = currentPlayer.GetComponent<SpriteRenderer>().sprite;
        float height = sprite.bounds.extents.y;
        Vector3 newPos = posSpawnPlayer + Vector3.up * height + Vector3.up * distancePlayerAndHealthBar;

        currentPlayer.gameObject.transform.position = newPos;
    }
    public void EndGame()
    {
        currentPlayer.EndGame();
    }
}
