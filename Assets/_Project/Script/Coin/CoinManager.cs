using UnityEngine;

//Singleton molto semplice
//Chiamare da Start in poi
public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    [SerializeField] private LayerMask _playerLayerMask = (1 << 6);
    private int _coinPickUp;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance !=  this)
        {
            Destroy(gameObject);
        }
    }

    public int GetPlayerLayerMask() => _playerLayerMask.value;

    public void CoinPickUp() => _coinPickUp++;
}
