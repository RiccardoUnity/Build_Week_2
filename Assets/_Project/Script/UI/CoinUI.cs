using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText;

    void Start()
    {
        _coinText.text = "0";
        CoinManager.Instance.onCoinPickUp.AddListener(UpdateUI);
    }

    void UpdateUI(int newCoinValue)
    {
        _coinText.text = newCoinValue.ToString();
    }
}
