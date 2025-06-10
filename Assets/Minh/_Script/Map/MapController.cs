using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : Singleton<MapController>
{
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private Transform mapStore;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    private GameObject currentMapInstance;
    private MapData currentMapData;

    public void LoadMap(MapData mapData)
    {
        // Despawn all existing objects under mapStore before loading new map
        if (mapStore != null)
        {
            foreach (Transform child in mapStore)
            {
                PoolingManager.Despawn(child.gameObject);
            }
        }

        if (currentMapInstance != null)
        {
            PoolingManager.Despawn(currentMapInstance);
        }
        currentMapInstance = PoolingManager.Spawn(mapData.MapPrefab, this.transform.position - new Vector3(offsetX, offsetY, 0), Quaternion.identity, mapStore);
        currentMapData = mapData;
        SetupMap();
    }

    private void SetupMap()
    {
        Tilemap tilemap = currentMapInstance.GetComponentInChildren<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("No Tilemap found in map prefab!");
            return;
        }

        BoundsInt bounds = currentMapData.TilemapBounds;

        for (int x = 0; x < currentMapData.MapLayout.Count; x++)
        {
            for (int y = 0; y < currentMapData.MapLayout[x].Count; y++)
            {
                int gridX = x + bounds.xMin;
                int gridY = y + bounds.yMin;
                Vector3Int gridPos = new Vector3Int(gridX, gridY, 0);
                Vector3 worldPos = tilemap.CellToWorld(gridPos);
                Vector3 adjustedPos = new Vector3(worldPos.x + offsetX, worldPos.y + offsetY / 2, worldPos.z);

                switch (currentMapData.MapLayout[x][y])
                {
                    case EMapTileType.Entrance:
                        if (currentMapData.MoveTiles != null)
                        {
                            foreach (var tile in currentMapData.MoveTiles)
                            {
                                if (tile != null && tile.position.x == gridX && tile.position.y == gridY && tile.tileType == EMapTileType.Entrance)
                                {
                                    if (tile.tileIcon != null)
                                    {
                                        GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
                                        Debug.Log($"Spawned {tile.tileIcon.name} at {adjustedPos} (Grid: {x}, {y}) for Entrance");
                                    }
                                    break;
                                }
                            }
                        }
                        PlayerController newPlayer = PoolingManager.Spawn<PlayerController>(playerPrefab, adjustedPos, Quaternion.identity, mapStore);
                        newPlayer.Initialize(tilemap, currentMapData, new Vector2Int(x, y));
                        Debug.Log($"Player spawned at Entrance: {adjustedPos} (Grid: {x}, {y})");
                        break;
                    case EMapTileType.Exit:
                        if (currentMapData.MoveTiles != null)
                        {
                            foreach (var tile in currentMapData.MoveTiles)
                            {
                                if (tile != null && tile.position.x == gridX && tile.position.y == gridY && tile.tileType == EMapTileType.Exit)
                                {
                                    if (tile.tileIcon != null)
                                    {
                                        GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
                                        Debug.Log($"Spawned {tile.tileIcon.name} at {adjustedPos} (Grid: {x}, {y}) for Exit");
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    case EMapTileType.Fight:
                        if (currentMapData.MoveTiles != null)
                        {
                            foreach (var tile in currentMapData.MoveTiles)
                            {
                                if (tile != null && tile.position.x == gridX && tile.position.y == gridY && tile.tileType == EMapTileType.Fight)
                                {
                                    if (tile.tileIcon != null)
                                    {
                                        GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
                                        Debug.Log($"Spawned {tile.tileIcon.name} at {adjustedPos} (Grid: {x}, {y}) for Fight");
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    case EMapTileType.BossFight:
                        if (currentMapData.MoveTiles != null)
                        {
                            foreach (var tile in currentMapData.MoveTiles)
                            {
                                if (tile != null && tile.position.x == gridX && tile.position.y == gridY && tile.tileType == EMapTileType.BossFight)
                                {
                                    if (tile.tileIcon != null)
                                    {
                                        GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
                                        Debug.Log($"Spawned {tile.tileIcon.name} at {adjustedPos} (Grid: {x}, {y}) for BossFight");
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    case EMapTileType.Healing:
                        if (currentMapData.MoveTiles != null)
                        {
                            foreach (var tile in currentMapData.MoveTiles)
                            {
                                if (tile != null && tile.position.x == gridX && tile.position.y == gridY && tile.tileType == EMapTileType.Healing)
                                {
                                    if (tile.tileIcon != null)
                                    {
                                        GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
                                        Debug.Log($"Spawned {tile.tileIcon.name} at {adjustedPos} (Grid: {x}, {y}) for Healing");
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    case EMapTileType.Gambling:
                        if (currentMapData.MoveTiles != null)
                        {
                            foreach (var tile in currentMapData.MoveTiles)
                            {
                                if (tile != null && tile.position.x == gridX && tile.position.y == gridY && tile.tileType == EMapTileType.Gambling)
                                {
                                    if (tile.tileIcon != null)
                                    {
                                        GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
                                        Debug.Log($"Spawned {tile.tileIcon.name} at {adjustedPos} (Grid: {x}, {y}) for Gambling");
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    case EMapTileType.UpgradeItems:
                        if (currentMapData.MoveTiles != null)
                        {
                            foreach (var tile in currentMapData.MoveTiles)
                            {
                                if (tile != null && tile.position.x == gridX && tile.position.y == gridY && tile.tileType == EMapTileType.UpgradeItems)
                                {
                                    if (tile.tileIcon != null)
                                    {
                                        GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
                                        Debug.Log($"Spawned {tile.tileIcon.name} at {adjustedPos} (Grid: {x}, {y}) for UpgradeItems");
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    case EMapTileType.MysteryClawMachine:
                        if (currentMapData.MoveTiles != null)
                        {
                            foreach (var tile in currentMapData.MoveTiles)
                            {
                                if (tile != null && tile.position.x == gridX && tile.position.y == gridY && tile.tileType == EMapTileType.MysteryClawMachine)
                                {
                                    if (tile.tileIcon != null)
                                    {
                                        GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
                                        Debug.Log($"Spawned {tile.tileIcon.name} at {adjustedPos} (Grid: {x}, {y}) for MysteryClawMachine");
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    case EMapTileType.PerkReward:
                        if (currentMapData.MoveTiles != null)
                        {
                            foreach (var tile in currentMapData.MoveTiles)
                            {
                                if (tile != null && tile.position.x == gridX && tile.position.y == gridY && tile.tileType == EMapTileType.PerkReward)
                                {
                                    if (tile.tileIcon != null)
                                    {
                                        GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
                                        Debug.Log($"Spawned {tile.tileIcon.name} at {adjustedPos} (Grid: {x}, {y}) for PerkReward");
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}