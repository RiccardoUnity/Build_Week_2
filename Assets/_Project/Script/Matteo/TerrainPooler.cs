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

    private Transform player;
    private float nextSpawnZ;

    [SerializeField] private bool useSecondPool = false;
    [SerializeField] private bool useThirdPool = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nextSpawnZ = 0f;

        // Inizializza Pool A
        for (int i = 0; i < poolSize; i++)
        {
            GameObject segmentA = Instantiate(terrainPrefabA, new Vector3(0, 0, nextSpawnZ), Quaternion.identity);
            poolA.Enqueue(segmentA);
            nextSpawnZ += segmentLength;
        }

        // Inizializza Pool B 
        for (int i = 0; i < poolSize; i++)
        {
            GameObject segmentB = Instantiate(terrainPrefabB, new Vector3(0, -1000, 0), Quaternion.identity); // Fuori vista
            poolB.Enqueue(segmentB);
        }

        // Inizializza Pool C
        for (int i = 0; i < poolSize; i++)
        {
            GameObject segmentC = Instantiate(terrainPrefabC, new Vector3(0, -1000, 0), Quaternion.identity); // Fuori vista
            poolC.Enqueue(segmentC);
        }
    }

    public void SpawnNextSegment()
    {
        GameObject segment;

        if (!useSecondPool && !useThirdPool)
        {
            segment = poolA.Dequeue();
            segment.transform.position = new Vector3(0, 0, nextSpawnZ);
            poolA.Enqueue(segment);
        }
        else if (useSecondPool && !useThirdPool)
        {
            segment = poolB.Dequeue();
            segment.transform.position = new Vector3(0, 0, nextSpawnZ);
            poolB.Enqueue(segment);
        }
        else if (useThirdPool)
        {
            segment = poolC.Dequeue();
            segment.transform.position = new Vector3(0, 0, nextSpawnZ);
            poolC.Enqueue(segment);
        }
        else
        {
            Debug.LogWarning("Nessun pool attivo!");
            return;
        }

        nextSpawnZ += segmentLength;
    }

    public void SwitchToSecondPool()
    {
        useSecondPool = true;
        useThirdPool = false;
    }

    public void SwitchToThirdPool()
    {
        useSecondPool = false;
        useThirdPool = true;
    }
}
