using System;
using System.Collections.Generic;
using UnityEngine;

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

        HashSet<Vector2Int> defined = new();

        foreach (var tile in tileDefinitions)
        {
            instance.tileGrid[tile.position] = new MapGridTile
            {
                position = tile.position,
                tileTypes = tile.tileTypes,
                iconPrefab = tile.iconPrefab,
                visited = tile.visited
            };
            defined.Add(tile.position);
        }
        var positions = new List<Vector2Int>(defined);
        for (int i = 0; i < positions.Count; i++)
        {
            for (int j = i + 1; j < positions.Count; j++)
            {
                Vector2Int from = positions[i];
                Vector2Int to = positions[j];
                if (from.x == to.x)
                {
                    int minY = Mathf.Min(from.y, to.y);
                    int maxY = Mathf.Max(from.y, to.y);
                    for (int y = minY + 1; y < maxY; y++)
                    {
                        Vector2Int pos = new Vector2Int(from.x, y);
                        if (!defined.Contains(pos))
                        {
                            instance.tileGrid[pos] = new MapGridTile
                            {
                                position = pos,
                                tileTypes = EMapTileType.Empty,
                                iconPrefab = null,
                                visited = false
                            };
                            defined.Add(pos);
                        }
                    }
                }
                else if (from.y == to.y)
                {
                    int minX = Mathf.Min(from.x, to.x);
                    int maxX = Mathf.Max(from.x, to.x);
                    for (int x = minX + 1; x < maxX; x++)
                    {
                        Vector2Int pos = new Vector2Int(x, from.y);
                        if (!defined.Contains(pos))
                        {
                            instance.tileGrid[pos] = new MapGridTile
                            {
                                position = pos,
                                tileTypes = EMapTileType.Empty,
                                iconPrefab = null,
                                visited = false
                            };
                            defined.Add(pos);
                        }
                    }
                }
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