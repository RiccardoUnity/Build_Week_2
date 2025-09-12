using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGM;

public class CoinLocalManager : MonoBehaviour
{
    private List<CoinPointSpawn> _coinPointSpawns = new List<CoinPointSpawn>();
    private float _ratioToSpawn = 0.5f;
    [SerializeField] private Lane _lane = Lane.Left;
    private bool _isCleanned;

    public void AddCoinInList(CoinPointSpawn coin) => _coinPointSpawns.Add(coin);

    private void OnEnable()
    {
        StartCoroutine(SetUp());
    }

    IEnumerator SetUp()
    {
        yield return null;
        Clean();
        float randomStart = 1f - _ratioToSpawn;
        randomStart = Random.Range(0f, randomStart);
        int coinStart = (int)Mathf.Lerp(0, _coinPointSpawns.Count, randomStart);
        int coinTotal = (int)(_coinPointSpawns.Count / _ratioToSpawn);
        for (int i = coinStart; i < coinTotal; i++)
        {
            _coinPointSpawns[i].gameObject.SetActive(true);
        }
    }

    private void Clean()
    {
        if (!_isCleanned)
        {
            foreach (CoinPointSpawn coin in _coinPointSpawns)
            {
                if (coin.GetLane() != _lane)
                {
                    Destroy(coin.gameObject);
                }
            }
            _isCleanned = true;
        }
    }

    private void OnDisable()
    {
        foreach (CoinPointSpawn coin in _coinPointSpawns)
        {
            coin.gameObject.SetActive(false);
        }
        _coinPointSpawns.RemoveAll(coin => coin == null);
    }

    public void SetRatioToSpawn(float value) => Mathf.Clamp01(value);
}
