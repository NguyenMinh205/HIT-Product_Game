using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitTrigger : MonoBehaviour
{
    private MapData _subsequentMap;
    public MapData SubsequentMap
    {
        get => _subsequentMap;
        set
        {
            _subsequentMap = value;
            if (_subsequentMap != null)
            {
                UpdateIconMaps();
            }
        }
    }

    [SerializeField] private Transform listIconMapStore;
    [SerializeField] private Image iconPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PoolingManager.Despawn(other.gameObject);
            MapSystem.Instance.ProceedToNextMap(SubsequentMap);
            gameObject.SetActive(false);
        }
    }
    public void UpdateIconMaps()
    {
        foreach (Transform child in listIconMapStore)
        {
            Destroy(child.gameObject);
        }

        if (SubsequentMap == null || iconPrefab == null) return;

        MapRuntimeInstance runtime = SubsequentMap.CreateRuntimeInstance();
        Dictionary<EMapTileType, Sprite> uniqueTileSprites = new();

        foreach (var tile in runtime.tileGrid.Values)
        {     
            EMapTileType typex = tile.tileTypes;
            if (typex == EMapTileType.Entrance || typex == EMapTileType.Exit || uniqueTileSprites.ContainsKey(typex))
                continue;
            if (tile.iconPrefab != null)
            {
                SpriteRenderer renderer = tile.iconPrefab.GetComponent<SpriteRenderer>();
                if (renderer != null && renderer.sprite != null)
                {
                    uniqueTileSprites[typex] = renderer.sprite;
                }
            }
        }

        foreach (var sprite in uniqueTileSprites.Values)
        {
            var icon = Instantiate(iconPrefab, listIconMapStore);
            icon.sprite = sprite;
            icon.gameObject.SetActive(true);
            icon.SetNativeSize();

            var rect = icon.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.sizeDelta *= 0.75f;
            }
        }
    }
}
