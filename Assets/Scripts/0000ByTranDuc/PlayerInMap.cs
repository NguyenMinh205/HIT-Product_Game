using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TranDuc;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInMap : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2Int posInMap;
    [SerializeField] private Vector2Int posInGrid;
    [SerializeField] private float moveDelay = 0.25f;

    private bool isIntoRoom;
    public bool IsIntoRoom
    {
        get => isIntoRoom;
        set
        {
            isIntoRoom = value;
        }
    }
    public Vector2Int PosInGrid
    {
        get => posInGrid;
        set
        {
            posInGrid = value;
        }
    }

    public Vector2Int PosInMap
    {
        get => posInMap;
        set
        {
            posInMap = value;
        }
    }
    private Tilemap tilemap;
    private MapData currentMapData;
    private bool isMoving = false;
    public bool IsMoving
    {
        set => isMoving = value;
    }
    protected void Start()
    {
        isIntoRoom = false;
    }

    public void Initialize(Tilemap mapTilemap, MapData mapData, Vector2Int spawnPosMap, Vector2Int spawnPosGrid, Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        tilemap = mapTilemap;
        currentMapData = mapData;
        posInMap = spawnPosMap;
        posInGrid = spawnPosGrid;
        rb.velocity = Vector2.zero;
        isMoving = false;
    }

    private void Update()
    {
        if (isMoving || tilemap == null || currentMapData == null || IsIntoRoom)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            TryMove(new Vector2Int(0, 1));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            TryMove(new Vector2Int(0, -1));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            TryMove(new Vector2Int(-1, 0));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            TryMove(new Vector2Int(1, 0));
        }
    }

    private void TryMove(Vector2Int direction)
    {
        Vector2Int newPos = posInGrid + direction;
        if (IsValidMove(newPos))
        {
            GamePlayController.Instance.Dir = direction;
            MoveToPosition(direction);        }
    }

    private bool IsValidMove(Vector2Int newPos)
    {
        if (newPos.x < 0 || newPos.x >= currentMapData.MapLayout.Count || newPos.y < 0 || newPos.y >= currentMapData.MapLayout[newPos.x].Count)
        {
            return false;
        }

        EMapTileType tileType = currentMapData.MapLayout[newPos.x][newPos.y];
        if (tileType == EMapTileType.Nothing)
        {
            return false;
        }
        return true;
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
                DOVirtual.DelayedCall(moveDelay, () => isMoving = false);
            });
    }

}
