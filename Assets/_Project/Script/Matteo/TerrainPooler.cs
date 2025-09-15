using UnityEngine;
using System.Collections.Generic;

public class TerrainPooler : MonoBehaviour
{
    [Header("Impostazioni Pool")]
    [SerializeField] private GameObject terrainPrefabA;
    [SerializeField] private GameObject terrainPrefabB;
    [SerializeField] private GameObject terrainPrefabC;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private float segmentLength = 30f;

    private Queue<GameObject> poolA = new Queue<GameObject>();
    private Queue<GameObject> poolB = new Queue<GameObject>();
    private Queue<GameObject> poolC = new Queue<GameObject>();

    private float nextSpawnZ = 0f;

    private int consecutiveCount = 0;
    private int currentPoolIndex = 0;

    void Start()
    {
        // Inizializza pool iniziale
        for (int i = 0; i < poolSize; i++)
        {
            GameObject segmentA = Instantiate(terrainPrefabA, new Vector3(0, 0, nextSpawnZ), Quaternion.identity);
            poolA.Enqueue(segmentA);
            nextSpawnZ += segmentLength;
            consecutiveCount++;
        }

        // Inizializza pool B (fuori vista)
        for (int i = 0; i < poolSize; i++)
        {
            GameObject segmentB = Instantiate(terrainPrefabB, new Vector3(0, -1000, 0), Quaternion.identity);
            poolB.Enqueue(segmentB);
        }

        // Inizializza pool C (fuori vista)
        for (int i = 0; i < poolSize; i++)
        {
            GameObject segmentC = Instantiate(terrainPrefabC, new Vector3(0, -1000, 0), Quaternion.identity);
            poolC.Enqueue(segmentC);
        }
    }

    public void SpawnNextSegment()
    {
    
        if (consecutiveCount >= 5)
        {
            int newPool = GetRandomOtherPool(currentPoolIndex);
            currentPoolIndex = newPool;
            consecutiveCount = 0;
        }

        GameObject segment = GetSegmentFromPool(currentPoolIndex);
        segment.transform.position = new Vector3(0, 0, nextSpawnZ);
        nextSpawnZ += segmentLength;

        consecutiveCount++;
    }

    private GameObject GetSegmentFromPool(int poolIndex)
    {
        GameObject segment;

        switch (poolIndex)
        {
            case 0:
                segment = poolA.Dequeue();
                poolA.Enqueue(segment);
                break;
            case 1:
                segment = poolB.Dequeue();
                poolB.Enqueue(segment);
                break;
            case 2:
                segment = poolC.Dequeue();
                poolC.Enqueue(segment);
                break;
            default:
                Debug.LogWarning("Pool index non valido, uso pool A");
                segment = poolA.Dequeue();
                poolA.Enqueue(segment);
                break;
        }

        return segment;
    }

    private int GetRandomOtherPool(int exclude)
    {
        List<int> otherPools = new List<int>();

        if (exclude != 0) otherPools.Add(0);
        if (exclude != 1) otherPools.Add(1);
        if (exclude != 2) otherPools.Add(2);

        return otherPools[Random.Range(0, otherPools.Count)];
    }
}
