using UnityEngine;

//Singleton molto semplice
//Chiamare da Start in poi
public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    [SerializeField] private LayerMask _playerLayerMask = (1 << 6);
    private int _coinPickUp;

    private bool _isDoubleCoins;

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

    public bool ChangeDoubleCoins() => _isDoubleCoins = !_isDoubleCoins;

    public void CoinPickUp()
    {
        if (_isDoubleCoins)
        {
            ++_coinPickUp;
        }
        else
        {
            _coinPickUp += 2;
        }
    }

    public int GetCoinPickUp() => _coinPickUp;
}
