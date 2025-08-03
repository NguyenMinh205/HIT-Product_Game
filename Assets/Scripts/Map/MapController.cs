using System;
using System.Collections.Generic;
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
    private List<SpecialTile> specialTile = new List<SpecialTile>();
    public List<SpecialTile> SpecialTile => specialTile;

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
        if (specialTile != null) specialTile.Clear();
        if (GameData.Instance.startData.isKeepingPlayGame)
        {
            specialTile = GameData.Instance.mainGameData.specialTiles;
        }
        else
        {
            foreach (var tile in mapData.MoveTiles)
            {
                if (tile != null)
                {
                    SpecialTile special = new SpecialTile
                    {
                        tileData = tile,
                        isInto = false,
                    };
                    specialTile.Add(special);
                }
            }
            GameData.Instance.mainGameData.specialTiles = specialTile;
            //GameData.Instance.SaveMainGameData();
        }
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
                Vector3 adjustedPos = new Vector3(worldPos.x + offsetX, worldPos.y + 0.5f, worldPos.z);

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
                                    }
                                    break;
                                }
                            }
                        }
                        if (GameData.Instance.startData.isKeepingPlayGame)
                        {
                            int posX = GameData.Instance.mainGameData.playerNodePosition.x - gridX;
                            int posY = GameData.Instance.mainGameData.playerNodePosition.y - gridY;
                            PlayerMapController newPlayer = PoolingManager.Spawn<PlayerMapController>(playerPrefab, adjustedPos + new Vector3(posX, posY, 0), Quaternion.identity, mapStore);
                            newPlayer.Initialize(tilemap, currentMapData, GameData.Instance.mainGameData.playerNodePosition, GameData.Instance.mainGameData.playerNodePosition - new Vector2Int(bounds.xMin, bounds.yMin), characterDatabase.GetCharacterById(GameData.Instance.startData.selectedCharacterId).skins[GameData.Instance.startData.selectedSkinIndex].skin);
                        }
                        else
                        {
                            PlayerMapController newPlayer = PoolingManager.Spawn<PlayerMapController>(playerPrefab, adjustedPos, Quaternion.identity, mapStore);
                            newPlayer.Initialize(tilemap, currentMapData, new Vector2Int(gridX, gridY), new Vector2Int(x,y), characterDatabase.GetCharacterById(GameData.Instance.startData.selectedCharacterId).skins[GameData.Instance.startData.selectedSkinIndex].skin);
                        }
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
                                    var special = specialTile?.Find(s => s.tileData.position == tile.position && s.tileData.tileType == tile.tileType);

                                    if (special.isInto)
                                        break;

                                    if (tile.tileIcon != null)
                                    {
                                        GameObject spawnedObject = PoolingManager.Spawn(tile.tileIcon, adjustedPos, Quaternion.identity, mapStore);
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

    public void SetRoomVisited(Vector2Int position)
    {
        var tile = specialTile.Find(t => t.tileData.position == position);
        if (tile != null)
        {
            tile.isInto = true;
        }
        GameData.Instance.mainGameData.specialTiles = specialTile;
        //GameData.Instance.SaveMainGameData();
    }
}

[Serializable]
public class SpecialTile
{
    public TileData tileData;
    public bool isInto;
}