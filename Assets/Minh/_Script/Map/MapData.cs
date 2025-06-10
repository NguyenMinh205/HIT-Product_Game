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
    [SerializeField] private int numOfExitDoor;
    private HashSet<TileData> specialIconTiles; //Lưu các ô đặc biệt để làm phần hiển thị xem cửa sau (tầng sau) sẽ có những ô nào

    public EMapType MapType => mapType;
    public GameObject MapPrefab => mapPrefab;
    public List<List<EMapTileType>> MapLayout => mapLayout;
    public List<TileData> MoveTiles => moveTiles; // Getter cho moveTiles
    public BoundsInt TilemapBounds { get; private set; }
    public int NumOfExitDoor => numOfExitDoor;

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
            Debug.LogError("There are no special tiles in moveTiles");
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
            Debug.LogError("No Tilemap found in map prefab for spawning objects!");
            return;
        }

        foreach (var tile in moveTiles)
        {
            if (tile == null)
            {
                Debug.LogWarning("TileData is null in moveTiles entry!");
                continue;
            }

            int layoutX = tile.position.x - bounds.xMin;
            int layoutY = tile.position.y - bounds.yMin;

            Debug.Log($"Tilemap Pos ({tile.position.x}, {tile.position.y}) -> MapLayout Index ({layoutX}, {layoutY})");

            if (layoutX >= 0 && layoutX < mapLayout.Count &&
                layoutY >= 0 && layoutY < mapLayout[layoutX].Count)
            {
                if (mapLayout[layoutX][layoutY] == EMapTileType.Empty)
                {
                    mapLayout[layoutX][layoutY] = tile.tileType;
                    Debug.Log($"Special Tile set at Tilemap Pos ({tile.position.x}, {tile.position.y}) -> MapLayout Index ({layoutX}, {layoutY}) to {tile.tileType}");
                }
                else
                {
                    Debug.LogWarning($"Cannot set Special Tile at Tilemap Pos ({tile.position.x}, {tile.position.y}) -> MapLayout Index ({layoutX}, {layoutY}) because it is {mapLayout[layoutX][layoutY]} (not Empty)");
                }
            }
            else
            {
                Debug.LogWarning($"Tilemap Position ({tile.position.x}, {tile.position.y}) -> MapLayout Index ({layoutX}, {layoutY}) is out of map bounds!");
            }
        }

        Debug.Log("Map Layout after setting special tiles:");
        for (int x = 0; x < mapLayout.Count; x++)
        {
            string row = $"Row {x}: ";
            for (int y = 0; y < mapLayout[x].Count; y++)
            {
                row += mapLayout[x][y].ToString() + " ";
            }
            Debug.Log(row);
        }
    }
}