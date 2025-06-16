using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public MapData SubsequentMap { get; set; }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PoolingManager.Despawn(other.gameObject);
            MapManager.Instance.ProceedToNextMap(SubsequentMap);
        }
    }
}