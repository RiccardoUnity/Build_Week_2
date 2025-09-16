using UnityEngine;

[CreateAssetMenu(fileName = "DoubleCoins", menuName = "PowerUps/DoubleCoins")]
public class SO_DoubleCoins : SO_BasePowerUp
{
    protected override void EnterUse(PlayerPowerUp player)
    {
        CoinManager.Instance.ChangeDoubleCoins();
    }

    protected override void StayUse(PlayerPowerUp player)
    {

    }

    protected override void ExitUse(PlayerPowerUp player)
    {
        CoinManager.Instance.ChangeDoubleCoins();
    }
}
