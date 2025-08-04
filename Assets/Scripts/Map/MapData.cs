using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class MapGridTile
{
    public Vector2Int position;
    public EMapTileType tileTypes;
    public GameObject iconPrefab;
    public bool visited = false;
}

[CreateAssetMenu(fileName = "New Map Data", menuName = "Map/StaticMapData")]
public class MapData : ScriptableObject
{
    [SerializeField] private EMapType mapType;
    [SerializeField] private GameObject mapPrefab;
    [SerializeField] private List<MapGridTile> tileDefinitions = new();

    public EMapType MapType => mapType;
    public GameObject MapPrefab => mapPrefab;
    public List<MapGridTile> TileDefinitions => tileDefinitions;

    public MapRuntimeInstance CreateRuntimeInstance()
    {
        var instance = new MapRuntimeInstance
        {
            sourceData = this,
            tileGrid = new Dictionary<Vector2Int, MapGridTile>()
        };

        Tilemap floorTilemap = null;
        Tilemap wallTilemap = null;
        foreach (var t in mapPrefab.GetComponentsInChildren<Tilemap>())
        {
            if (t.CompareTag("MapFloor"))
            {
                floorTilemap = t;
            }
            else if (t.CompareTag("MapWall"))
            {
                wallTilemap = t;
            }
        }

        if (floorTilemap == null)
        {
            Debug.LogError("Không tìm thấy Tilemap với tag 'MapFloor' trong mapPrefab!");
            return instance;
        }

        BoundsInt bounds = floorTilemap.cellBounds;
        instance.tilemapBounds = bounds;

        HashSet<Vector2Int> defined = new();

        foreach (var pos in bounds.allPositionsWithin)
        {
            Vector2Int gridPos = new Vector2Int(pos.x, pos.y);
            if (floorTilemap.GetTile(pos) != null && !defined.Contains(gridPos))
            {
                instance.tileGrid[gridPos] = new MapGridTile
                {
                    position = gridPos,
                    tileTypes = EMapTileType.Empty,
                    iconPrefab = null,
                    visited = false
                };
                defined.Add(gridPos);
            }
        }

        if (wallTilemap != null)
        {
            foreach (var pos in bounds.allPositionsWithin)
            {
                Vector2Int gridPos = new Vector2Int(pos.x, pos.y);
                if (wallTilemap.GetTile(pos) != null && !defined.Contains(gridPos))
                {
                    instance.tileGrid[gridPos] = new MapGridTile
                    {
                        position = gridPos,
                        tileTypes = EMapTileType.Nothing,
                        iconPrefab = null,
                        visited = false
                    };
                    defined.Add(gridPos);
                }
            }
        }

        foreach (var tile in tileDefinitions)
        {
            if (instance.tileGrid.ContainsKey(tile.position))
            {
                instance.tileGrid[tile.position] = new MapGridTile
                {
                    position = tile.position,
                    tileTypes = tile.tileTypes,
                    iconPrefab = tile.iconPrefab,
                    visited = tile.visited
                };
            }
            else
            {
                Debug.LogWarning($"Defined tile at {tile.position} is outside floor bounds.");
            }
        }

        return instance;
    }
}

public class MapRuntimeInstance
{
    public MapData sourceData;
    public Dictionary<Vector2Int, MapGridTile> tileGrid;
    public List<ExitTrigger> spawnedExitTriggers = new();
    public BoundsInt tilemapBounds;

    public void AddExitTrigger(ExitTrigger trigger)
    {
        if (!spawnedExitTriggers.Contains(trigger))
        {
            spawnedExitTriggers.Add(trigger);
        }
    }

    public void ClearExitTriggers()
    {
        spawnedExitTriggers.Clear();
    }

    public int ExitCount => spawnedExitTriggers.Count;
}