using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

[CreateAssetMenu(fileName = "New Map Data", menuName = "Map Data")]
public class MapData : ScriptableObject
{
    [SerializeField] private EMapType mapType;
    [SerializeField] private GameObject mapPrefab;
    [SerializeField] private List<List<EMapTileType>> mapLayout = new List<List<EMapTileType>>();
    [SerializeField] private List<TileData> moveTiles;
    private int numOfExitDoor = 0;
    private List<ExitTrigger> exitDoors = new List<ExitTrigger>();

    public EMapType MapType => mapType;
    public GameObject MapPrefab => mapPrefab;
    public List<List<EMapTileType>> MapLayout => mapLayout;
    public List<TileData> MoveTiles => moveTiles;
    public BoundsInt TilemapBounds { get; private set; }
    public int NumOfExitDoor => numOfExitDoor;
    public List<ExitTrigger> ExitDoors => exitDoors;

    public void UpdateMapLayout()
    {
        if (mapPrefab == null)
        {
            Debug.LogError("Map Prefab is not assigned!");
            return;
        }

        Tilemap tilemap = null;
        foreach (var t in mapPrefab.GetComponentsInChildren<Tilemap>())
        {
            if (t.CompareTag("MapFloor"))
            {
                tilemap = t;
                break;
            }
        }

        if (tilemap == null)
        {
            Debug.LogError("No Tilemap found in map prefab!");
            return;
        }

        BoundsInt bounds = tilemap.cellBounds;
        TilemapBounds = bounds;
        Debug.Log($"Tilemap bounds: {bounds}");
        mapLayout.Clear();
        ClearExitDoors(); // Xóa danh sách cửa thoát cũ và reset số lượng

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            mapLayout.Add(new List<EMapTileType>());
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(cellPos);

                if (tile != null)
                {
                    mapLayout[x - bounds.xMin].Add(EMapTileType.Empty);
                    Debug.Log($"Position ({x}, {y}) -> MapLayout Index ({x - bounds.xMin}, {y - bounds.yMin}) - Tile: Empty");
                }
                else
                {
                    mapLayout[x - bounds.xMin].Add(EMapTileType.Nothing);
                }
            }
        }

        Debug.Log("Map Layout before setting special tiles:");
        for (int x = 0; x < mapLayout.Count; x++)
        {
            string row = $"Row {x}: ";
            for (int y = 0; y < mapLayout[x].Count; y++)
            {
                row += mapLayout[x][y].ToString() + " ";
            }
            Debug.Log(row);
        }

        SetUpSpecialTiles(bounds);
    }

    public void SetUpSpecialTiles(BoundsInt bounds)
    {
        if (moveTiles == null || moveTiles.Count == 0)
        {
            Debug.LogError("Không có tile đặc biệt nào trong moveTiles");
            return;
        }

        foreach (var tile in moveTiles)
        {
            if (tile == null)
            {
                Debug.LogWarning("TileData là null trong moveTiles entry!");
                continue;
            }

            int layoutX = tile.position.x - bounds.xMin;
            int layoutY = tile.position.y - bounds.yMin;

            Debug.Log($"Vị trí Tilemap ({tile.position.x}, {tile.position.y}) -> Chỉ số MapLayout ({layoutX}, {layoutY})");

            if (layoutX >= 0 && layoutX < mapLayout.Count && layoutY >= 0 && layoutY < mapLayout[layoutX].Count)
            {
                if (mapLayout[layoutX][layoutY] == EMapTileType.Empty)
                {
                    mapLayout[layoutX][layoutY] = tile.tileType;
                    Debug.Log($"Đặt tile đặc biệt tại Vị trí Tilemap ({tile.position.x}, {tile.position.y}) -> Chỉ số MapLayout ({layoutX}, {layoutY}) thành {tile.tileType}");
                }
                else
                {
                    Debug.LogWarning($"Không thể đặt tile đặc biệt tại Vị trí Tilemap ({tile.position.x}, {tile.position.y}) -> Chỉ số MapLayout ({layoutX}, {layoutY}) vì nó là {mapLayout[layoutX][layoutY]} (không phải Empty)");
                }
            }
            else
            {
                Debug.LogWarning($"Vị trí Tilemap ({tile.position.x}, {tile.position.y}) -> Chỉ số MapLayout ({layoutX}, {layoutY}) vượt ra ngoài giới hạn map!");
            }
        }

        Debug.Log("Map Layout sau khi đặt tile đặc biệt:");
        for (int x = 0; x < mapLayout.Count; x++)
        {
            string row = $"Hàng {x}: ";
            for (int y = 0; y < mapLayout[x].Count; y++)
            {
                row += mapLayout[x][y].ToString() + " ";
            }
            Debug.Log(row);
        }
    }

    public void AddSpawnedExitTrigger(ExitTrigger exitTrigger)
    {
        if (exitTrigger != null && !exitDoors.Contains(exitTrigger))
        {
            exitDoors.Add(exitTrigger);
            numOfExitDoor++; 
        }
    }

    public void ClearExitDoors()
    {
        exitDoors.Clear();
        numOfExitDoor = 0;
    }
}