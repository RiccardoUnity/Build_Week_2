using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private TerrainPooler pooler;

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            pooler.SpawnNextSegment();
        }
    }
}
