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
    private List<Sprite> iconMaps = new List<Sprite>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PoolingManager.Despawn(other.gameObject);
            MapManager.Instance.ProceedToNextMap(SubsequentMap);
        }
    }

    public void UpdateIconMaps()
    {
        iconMaps.Clear();

        foreach (Transform child in listIconMapStore)
        {
            Destroy(child.gameObject);
        }

        if (SubsequentMap == null || SubsequentMap.MoveTiles == null)
        {
            return;
        }

        if (iconPrefab == null)
        {
            return;
        }

        Dictionary<EMapTileType, Sprite> uniqueTileTypes = new Dictionary<EMapTileType, Sprite>();

        foreach (var tileData in SubsequentMap.MoveTiles)
        {
            if (tileData == null || tileData.tileType == EMapTileType.Entrance || tileData.tileType == EMapTileType.Exit)
            {
                continue;
            }

            if (tileData.tileIcon != null)
            {
                SpriteRenderer spriteRenderer = tileData.tileIcon.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && spriteRenderer.sprite != null)
                {
                    if (!uniqueTileTypes.ContainsKey(tileData.tileType))
                    {
                        uniqueTileTypes.Add(tileData.tileType, spriteRenderer.sprite);
                    }
                }
            }
        }

        iconMaps.AddRange(uniqueTileTypes.Values);

        foreach (var sprite in iconMaps)
        {
            Image imageInstance = Instantiate(iconPrefab, listIconMapStore);
            imageInstance.sprite = sprite;
            imageInstance.gameObject.SetActive(true);

            if (sprite != null && imageInstance != null)
            {
                imageInstance.SetNativeSize();
                RectTransform rectTransform = imageInstance.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta *= 0.75f;
                }
            }
        }
    }
}