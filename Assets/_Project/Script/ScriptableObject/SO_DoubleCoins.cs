using UnityEngine;

[CreateAssetMenu(fileName = "DoubleCoins", menuName = "PowerUps/DoubleCoins")]
public class SO_DoubleCoins : SO_BasePowerUp
{
    protected override void EnterUse()
    {
        CoinManager.Instance.ChangeDoubleCoins();
    }

    protected override void StayUse()
    {

    }

    protected override void ExitUse()
    {
        CoinManager.Instance.ChangeDoubleCoins();
    }
}
