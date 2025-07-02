using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlayerMapController : Singleton<PlayerMapController>
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Vector2Int posInGrid;
    [SerializeField] private float moveDelay = 0.2f;

    private bool isIntoRoom;
    public bool IsIntoRoom
    {
        get => isIntoRoom;
        set => isIntoRoom = value;
    }
    public Vector2Int PosInGrid
    {
        get => posInGrid;
        set
        {
            posInGrid = value;
        }
    }

    private Rigidbody2D rb;
    private Tilemap tilemap;
    private MapData currentMapData;
    private bool isMoving = false;

    public bool IsMoving
    {
        set => isMoving = value;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isIntoRoom = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Tilemap mapTilemap, MapData mapData, Vector2Int spawnPos, Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        tilemap = mapTilemap;
        currentMapData = mapData;
        PosInGrid = spawnPos;
        rb.velocity = Vector2.zero;
        isMoving = false;
        Debug.Log($"Player initialized at grid position: {spawnPos}, world position: {transform.position}");
    }

    private void Update()
    {
        Debug.Log("Check Player Map Controller");
        if (isMoving || tilemap == null || currentMapData == null) return;

        if (isIntoRoom) return;
        Debug.Log("Check Player in Room");
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryMove(new Vector2Int(0, 1));
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            TryMove(new Vector2Int(0, -1));
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            TryMove(new Vector2Int(-1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            TryMove(new Vector2Int(1, 0));
        }
    }

    private void TryMove(Vector2Int direction)
    {
        Vector2Int newPos = posInGrid + direction;
        if (IsValidMove(newPos))
        {
            Debug.Log(" Check Function Try Move");
            GameController.Instance.Dir = direction;
            StartCoroutine(MoveToPosition(direction));
        }
    }

    private bool IsValidMove(Vector2Int newPos)
    {
        if (newPos.x < 0 || newPos.x >= currentMapData.MapLayout.Count ||
            newPos.y < 0 || newPos.y >= currentMapData.MapLayout[newPos.x].Count)
        {
            Debug.Log($"Move to ({newPos.x}, {newPos.y}) blocked: Out of bounds");
            return false;
        }

        EMapTileType tileType = currentMapData.MapLayout[newPos.x][newPos.y];
        if (tileType == EMapTileType.Nothing)
        {
            Debug.Log($"Move to ({newPos.x}, {newPos.y}) blocked: Tile is Nothing");
            return false;
        }

        Debug.Log($"Move to ({newPos.x}, {newPos.y}) allowed: Tile is {tileType}");
        return true;
    }

    public IEnumerator MoveToPosition(Vector2Int direction)
    {
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        Vector3 targetPosition = startPosition + new Vector3(direction.x, direction.y, 0) * tilemap.cellSize.x;

        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (direction.x > 0 ? 1 : -1);
            transform.localScale = scale;
        }

        while (elapsedTime < moveDelay)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDelay;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        posInGrid = posInGrid + direction;
        rb.velocity = Vector2.zero;
        isMoving = false;
    }
}