using UnityEngine;
using UnityEngine.Events;
using Save = SGM.S_SaveManager;

//Singleton molto semplice
//Chiamare da Start in poi
public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    [SerializeField] private LayerMask _playerLayerMask = (1 << 6);
    private int _coinPickUp;

    private bool _isDoubleCoins;

    public UnityEvent<int> onCoinPickUp;

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

        FindAnyObjectByType<PlayerController>().playerDeadEvent.AddListener(SaveCoin);
    }

    public int GetPlayerLayerMask() => _playerLayerMask.value;

    public bool ChangeDoubleCoins() => _isDoubleCoins = !_isDoubleCoins;

    public void CoinPickUp()
    {
        if (_isDoubleCoins)
        {
            _coinPickUp += 2;
        }
        else
        {
            ++_coinPickUp;
        }
        onCoinPickUp?.Invoke(_coinPickUp);
    }

    public int GetCoinPickUp() => _coinPickUp;

    private void SaveCoin()
    {
        Save.SaveCoin(_coinPickUp);
        _coinPickUp = 0;
    }
}
