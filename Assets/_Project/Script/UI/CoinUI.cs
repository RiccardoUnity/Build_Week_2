using TMPro;
using UnityEngine;
using Save = SGM.S_SaveManager;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText;

    void Start()
    {
        _coinText.text = Save.GetCoin().ToString();
        CoinManager.Instance.onCoinPickUp.AddListener(UpdateUI);
    }

    void UpdateUI(int newCoinValue)
    {
        _coinText.text = newCoinValue.ToString();
    }
}
