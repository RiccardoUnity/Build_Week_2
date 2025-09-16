using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using SGM;

public class OnDeathUIScoreBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI punteggioText;

    private void OnEnable()
    {
        MostraPunteggio();
    }
    private void MostraPunteggio()
    {
        int punteggio = CoinManager.Instance.GetCoinPickUp();
        punteggioText.text = punteggio.ToString();
    }
}


