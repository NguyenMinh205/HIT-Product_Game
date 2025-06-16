using UnityEngine;

public class TumblerBox : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Collider2D exitTrigger;
    [SerializeField] private Collider2D collider;

    public Transform SpawnPoint => spawnPoint;
    public Collider2D ExitTrigger => exitTrigger;

    public Collider2D Collider => collider;

    private void OnEnable()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("TumblerBox: SpawnPoint hoặc PerkMachine_Tube chưa được gán!");
        }
        collider.enabled = false;
    }
}