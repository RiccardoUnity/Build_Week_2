using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Save = SGM.S_SaveManager;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private CoinManager _coinManager;
    [SerializeField] private TextMeshProUGUI _coinText;

    void Start()
    {
        _coinText.text = Save.GetCoin().ToString();
        if (_coinManager != null)
            _coinManager.onCoinPickUp.AddListener(UpdateUI);
    }

    void UpdateUI(int newCoinValue)
    {
        _coinText.text = newCoinValue.ToString();
    }
}
