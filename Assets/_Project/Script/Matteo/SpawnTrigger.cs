using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    private TerrainPooler pooler;
    private static int triggerCounter = 0; // Statico così vale per tutti i trigger

    void Start()
    {
        pooler = FindObjectOfType<TerrainPooler>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerCounter++;

            // Dopo 5 trigger, cambia pool
            if (triggerCounter == 5)
            {
                pooler.SwitchToSecondPool();
                Debug.Log("➡️ Passaggio al secondo terreno!");
            }
            else if (triggerCounter == 10)
            {
                pooler.SwitchToThirdPool();
            }

            pooler.SpawnNextSegment();
        }
    }
}
