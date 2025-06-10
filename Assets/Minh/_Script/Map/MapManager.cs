using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private List<MapData> fightMaps;
    [SerializeField] private List<MapData> bossMaps;
    [SerializeField] private List<MapData> restMaps;
    [SerializeField] private int numFloor;
    private List<MapData> dungeonSequence = new List<MapData>();
    private int currentMapIndex = 0;

    public static MapManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        fightMaps = Resources.LoadAll<MapData>("SO/Fight").ToList();
        bossMaps = Resources.LoadAll<MapData>("SO/Boss").ToList();
        restMaps = Resources.LoadAll<MapData>("SO/Rest").ToList();
        GenerateDungeonSequence();
        LoadInitialMap();
    }

    private void GenerateDungeonSequence()
    {
        dungeonSequence.Clear();

        // Floor 1: fightMaps[0]
        if (fightMaps.Count > 0)
        {
            dungeonSequence.Add(fightMaps[0]);
        }

        // Floor 2: bossMaps[0]
        if (bossMaps.Count > 0)
        {
            dungeonSequence.Add(bossMaps[0]);
        }

        int currentFloor = 3;
        while (dungeonSequence.Count < numFloor)
        {
            MapData previousMap = dungeonSequence.Count > 1 ? dungeonSequence[dungeonSequence.Count - 1] : null;

            if (previousMap != null && previousMap.MapType == EMapType.Bossfight)
            {
                // Sau tầng boss, chọn map từ restMaps hoặc fightMaps, không trùng với map trước đó
                MapData nextMap = GetNextMapExcludingType(previousMap, new[] { EMapType.Bossfight });
                if (nextMap != null)
                {
                    dungeonSequence.Add(nextMap);
                }
            }
            else if (dungeonSequence.Count % 3 == 2 && currentFloor <= numFloor)
            {
                // Tầng boss (mỗi 3 tầng)
                MapData bossMap = null;
                int bossIndex = currentFloor == numFloor ? 1 : 0;
                if (bossMaps.Count > bossIndex && bossMaps[bossIndex] != dungeonSequence.LastOrDefault())
                {
                    bossMap = bossMaps[bossIndex];
                }
                else
                {
                    bossMap = GetNextMapExcludingType(previousMap, new[] { EMapType.Bossfight });
                }
                if (bossMap != null)
                {
                    dungeonSequence.Add(bossMap);
                }
                currentFloor += 3; 
            }
            else
            {
                EMapType nextType = previousMap != null ? previousMap.MapType : EMapType.Fight;
                MapData nextMap = GetNextMapExcludingType(previousMap, new[] { nextType });
                if (nextMap != null)
                {
                    dungeonSequence.Add(nextMap);
                }
            }
        }

        if (numFloor > 2 && bossMaps.Count > 1 && bossMaps[1] != dungeonSequence.LastOrDefault())
        {
            dungeonSequence[numFloor - 1] = bossMaps[bossMaps.Count - 1];
        }

        Debug.Log("Dungeon Sequence:");
        foreach (var map in dungeonSequence)
        {
            Debug.Log($"Map: {map.MapType}, Name: {map.name}");
        }
    }

    private MapData GetNextMapExcludingType(MapData previousMap, EMapType[] excludedTypes)
    {
        List<MapData> availableMaps = new List<MapData>();
        EMapType preferredType = previousMap != null ? (previousMap.MapType == EMapType.Bossfight ? EMapType.Rest : EMapType.Fight) : EMapType.Fight;

        if (!excludedTypes.Contains(preferredType))
        {
            switch (preferredType)
            {
                case EMapType.Fight:
                    availableMaps = fightMaps.Where(m => m != previousMap).ToList();
                    break;
                case EMapType.Rest:
                    availableMaps = restMaps.Where(m => m != previousMap).ToList();
                    break;
            }
        }

        // Nếu không có map phù hợp với preferredType, thử các loại khác
        if (availableMaps.Count == 0)
        {
            foreach (EMapType type in new[] { EMapType.Fight, EMapType.Rest })
            {
                if (!excludedTypes.Contains(type))
                {
                    switch (type)
                    {
                        case EMapType.Fight:
                            availableMaps = fightMaps.Where(m => m != previousMap).ToList();
                            break;
                        case EMapType.Rest:
                            availableMaps = restMaps.Where(m => m != previousMap).ToList();
                            break;
                    }
                    if (availableMaps.Count > 0) break;
                }
            }
        }

        if (availableMaps.Count == 0)
        {
            return null; // Hoặc thêm logic fallback nếu cần
        }

        return availableMaps[Random.Range(0, availableMaps.Count)];
    }

    public List<MapData> GetDungeonSequence()
    {
        return dungeonSequence;
    }

    private void LoadInitialMap()
    {
        dungeonSequence[currentMapIndex].UpdateMapLayout();
        MapController.Instance.LoadMap(dungeonSequence[currentMapIndex]);
    }

    public void ProceedToNextMap(int exitIndex = -1)
    {
        currentMapIndex++;
        if (currentMapIndex < dungeonSequence.Count)
        {
            dungeonSequence[currentMapIndex].UpdateMapLayout();
            MapController.Instance.LoadMap(dungeonSequence[currentMapIndex]);
            Debug.Log($"Loaded Map: {dungeonSequence[currentMapIndex].MapType} at index {currentMapIndex}");
        }
        else
        {
            Debug.Log("Dungeon completed!");
        }
    }
}