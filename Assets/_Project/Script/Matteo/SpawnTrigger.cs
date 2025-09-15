using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    private TerrainPooler pooler;

    void Start()
    {
        pooler = FindObjectOfType<TerrainPooler>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            pooler.SpawnNextSegment();
        }
    }
}
