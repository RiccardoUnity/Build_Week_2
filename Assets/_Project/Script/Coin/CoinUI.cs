using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private CoinManager _coinManager;
    [SerializeField] private TextMeshProUGUI _coinText;

    void OnEnable()
    {
        if (_coinManager != null)
            _coinManager.onCoinPickUp.AddListener(UpdateUI);
    }

    void OnDisable()
    {
        if (_coinManager != null)
            _coinManager.onCoinPickUp.RemoveListener(UpdateUI);
    }

    void UpdateUI(int newCoinValue)
    {
        _coinText.text = newCoinValue.ToString();
    }
}
