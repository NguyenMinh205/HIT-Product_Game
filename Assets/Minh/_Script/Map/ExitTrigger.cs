using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PoolingManager.Despawn(other.gameObject);
            MapManager.Instance.ProceedToNextMap();
        }
    }
}