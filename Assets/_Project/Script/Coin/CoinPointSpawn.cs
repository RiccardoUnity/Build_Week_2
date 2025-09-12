using UnityEngine;
using SGM;
using LaneName = SGM.S_GameManager;

public class CoinPointSpawn : MonoBehaviour
{
    private CoinLocalManager _coinLocalManager;
    private Lane _lane;

    void Awake()
    {
        _coinLocalManager = GetComponentInParent<CoinLocalManager>();
        _coinLocalManager.AddCoinInList(this);

        if (transform.parent.tag.Equals(LaneName.GetLaneName(0)))
        {
            _lane = Lane.Left;
        }
        else if (transform.parent.tag.Equals(LaneName.GetLaneName(1)))
        {
            _lane = Lane.Center;
        }
        else if (transform.parent.tag.Equals(LaneName.GetLaneName(2)))
        {
            _lane = Lane.Right;
        }
    }

    public Lane GetLane() => _lane;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == CoinManager.Instance.GetPlayerLayerMask())
        {
            CoinManager.Instance.CoinPickUp();
            gameObject.SetActive(false);
        }
    }

    void OnDestroy() => _coinLocalManager.RemoveCoinInList(this);
}
