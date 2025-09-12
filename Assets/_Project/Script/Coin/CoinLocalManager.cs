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

    public void RemoveCoinInList(CoinPointSpawn coin) => _coinPointSpawns.Remove(coin);

    private void OnEnable()
    {
        StartCoroutine(SetUp());
    }

    IEnumerator SetUp()
    {
        yield return null;
        if (!_isCleanned)
        {
            foreach (CoinPointSpawn coin in _coinPointSpawns)
            {
                coin.gameObject.SetActive(false);
                if (coin.GetLane() != _lane)
                {
                    Destroy(coin.gameObject);
                }
            }
            yield return null;

            CoinPointSpawn[] coins = new CoinPointSpawn[_coinPointSpawns.Count];
            float minZ = float.MinValue;
            float maxZ = float.MaxValue;
            int index = 0;

            for (int j = 0; j < coins.Length; j++)
            {
                for (int i = 0; i < _coinPointSpawns.Count; i++)
                {
                    if (_coinPointSpawns[i].transform.localPosition.z > minZ
                        && _coinPointSpawns[i].transform.localPosition.z < maxZ)
                    {
                        maxZ = _coinPointSpawns[i].transform.localPosition.z;
                        index = i;
                    }
                }
                minZ = maxZ;
                maxZ = float.MaxValue;
                coins[j] = _coinPointSpawns[index];
            }
            _coinPointSpawns.Clear();
            for (int i = 0; i < coins.Length; ++i)
            {
                _coinPointSpawns.Add(coins[i]);
            }

            _isCleanned = true;
        }

        float randomStart = 1f - _ratioToSpawn;
        randomStart = Random.Range(0f, randomStart);
        int coinStart = (int)Mathf.Lerp(0, _coinPointSpawns.Count, randomStart);
        int coinTotal = (int)(_coinPointSpawns.Count * _ratioToSpawn);
        for (int i = coinStart; i < coinTotal; i++)
        {
            _coinPointSpawns[i].gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        foreach (CoinPointSpawn coin in _coinPointSpawns)
        {
            coin.gameObject.SetActive(false);
        }
    }

    public void SetRatioToSpawn(float value) => Mathf.Clamp01(value);
}
