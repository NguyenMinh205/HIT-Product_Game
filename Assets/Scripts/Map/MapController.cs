using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : Singleton<MapController>
{
    [SerializeField] private PlayerMapController playerPrefab;
    [SerializeField] private CharacterDatabaseSO characterDatabase;
    [SerializeField] private Transform mapStore;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    private GameObject currentMapInstance;
    private MapData currentMapData;

    public void SetActiveMapStore(bool val)
    {
        mapStore.gameObject.SetActive(val);
    }
    public void LoadMap(MapData mapData)
    {
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
                        PlayerMapController newPlayer = PoolingManager.Spawn<PlayerMapController>(playerPrefab, adjustedPos, Quaternion.identity, mapStore);
                        newPlayer.Initialize(tilemap, currentMapData, new Vector2Int(x, y), characterDatabase.GetCharacterById(PlayerPrefs.GetString("SelectedCharacterId", "")).skins[PlayerPrefs.GetInt("SelectedSkinIndex", 0)].skin);
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
                                        ExitTrigger exitTrigger = spawnedObject.GetComponent<ExitTrigger>();
                                        if (exitTrigger != null)
                                        {
                                            currentMapData.AddSpawnedExitTrigger(exitTrigger);
                                            Debug.Log($"Spawned {tile.tileIcon.name} với ExitTrigger tại {adjustedPos} (Grid: {x}, {y}) cho Exit");
                                        }
                                        else
                                        {
                                            Debug.LogError($"Tile Exit tại {adjustedPos} (Grid: {x}, {y}) không có component ExitTrigger!");
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        break;
                    case EMapTileType.Fight:
                    case EMapTileType.HardFight:
                    case EMapTileType.BossFight:
                    case EMapTileType.Healing:
                    case EMapTileType.UpgradeItems:
                    case EMapTileType.Shredder:
                    case EMapTileType.Gambling:
                    case EMapTileType.PerkReward:
                    case EMapTileType.MysteryClawMachine:
                        if (currentMapData.MoveTiles != null)
                        {
                            foreach (var tile in currentMapData.MoveTiles)
                            {
                                if (tile != null && tile.position.x == gridX && tile.position.y == gridY && tile.tileType == currentMapData.MapLayout[x][y])
                                {
                                    if (tile.tileIcon != null)
                                    {
                                        GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
                                        Debug.Log($"Spawned {tile.tileIcon.name} at {adjustedPos} (Grid: {x}, {y}) for {currentMapData.MapLayout[x][y]}");
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