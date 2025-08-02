using DG.Tweening;
using System.Collections.Generic;
using TranDuc;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInMap : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveDelay = 0.25f;

    private bool isMoving = false;
    private bool isIntoRoom;

    private Vector2Int posInMap;
    private Vector2Int posInGrid;

    private Tilemap tilemap;
    private MapRuntimeInstance mapInstance;

    public bool IsIntoRoom
    {
        get => isIntoRoom;
        set => isIntoRoom = value;
    }

    public Vector2Int PosInMap
    {
        get => posInMap;
        set => posInMap = value;
    }

    public Vector2Int PosInGrid
    {
        get => posInGrid;
        set => posInGrid = value;
    }

    public bool IsMoving
    {
        set => isMoving = value;
    }

    private void Start()
    {
        isIntoRoom = false;
    }

    public void Initialize(Tilemap mapTilemap, MapRuntimeInstance mapData, Vector2Int spawnPosMap, Vector2Int spawnPosGrid, Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        tilemap = mapTilemap;
        mapInstance = mapData;  
        posInMap = spawnPosMap;
        posInGrid = spawnPosGrid;
        rb.velocity = Vector2.zero;
        isMoving = false;
    }

    private void Update()
    {
        if (isMoving || tilemap == null || mapInstance == null || IsIntoRoom)
            return;

        if (Input.GetKeyDown(KeyCode.W)) TryMove(Vector2Int.up);
        else if (Input.GetKeyDown(KeyCode.S)) TryMove(Vector2Int.down);
        else if (Input.GetKeyDown(KeyCode.A)) TryMove(Vector2Int.left);
        else if (Input.GetKeyDown(KeyCode.D)) TryMove(Vector2Int.right);
    }

    private void TryMove(Vector2Int direction)
    {
        Vector2Int newGridPos = posInGrid + direction;
        if (IsValidMove(newGridPos))
        {
            Debug.Log("OK1");
            GamePlayController.Instance.Dir = direction;
            MoveToPosition(direction);
        }
    }

    private bool IsValidMove(Vector2Int newGridPos)
    {
        foreach (var entry in mapInstance.tileGrid)
        {
            var pos = entry.Key;
            var x = entry.Value;
            Debug.Log($"GridPos: {pos}, TileType(s): {string.Join(", ", x.tileTypes)}");
        }
        if (mapInstance.tileGrid.TryGetValue(newGridPos, out var tile))
        {
            Debug.Log("OK2");
            if (tile.tileTypes != EMapTileType.Nothing)
                return true;
        }
        return false;
    }

    private void MoveToPosition(Vector2Int direction)
    {
        if (isMoving) return;

        isMoving = true;
        AudioManager.Instance.PlayMoveSound();

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(direction.x, direction.y, 0) * tilemap.cellSize.x;

        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (direction.x > 0 ? 1 : -1);
            transform.localScale = scale;
        }

        transform.DOMove(targetPosition, moveDelay)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transform.position = targetPosition;
                posInMap += direction;
                posInGrid += direction;
                rb.velocity = Vector2.zero;
                DataManager.Instance.GameData.PlayerNodePosition = posInMap;

                MapSystem.Instance.SetRoomVisited(); 
                DOVirtual.DelayedCall(moveDelay, () => isMoving = false);
            });
    }
}
