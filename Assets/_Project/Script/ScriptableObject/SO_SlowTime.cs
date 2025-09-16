using UnityEngine;

[CreateAssetMenu(fileName = "New SlowTime", menuName = "PowerUps/Slow Time")]
public class SO_SlowTime : SO_BasePowerUp
{
    [SerializeField] private float _slowTime = 0.5f;

    protected override void EnterUse(PlayerPowerUp player)
    {
        Time.timeScale = _slowTime;
    }

    protected override void StayUse(PlayerPowerUp player)
    {

    }

    protected override void ExitUse(PlayerPowerUp player)
    {
        Time.timeScale = 1f;
    }
}
