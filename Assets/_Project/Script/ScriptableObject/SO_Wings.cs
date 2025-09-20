using UnityEngine;

[CreateAssetMenu(fileName = "Wings", menuName = "PowerUps/Wings")]
public class SO_Wings : SO_BasePowerUp
{
    protected override void EnterUse(PlayerPowerUp player)
    {
        player.GetPlayerController().EnterWings();
    }

    protected override void StayUse(PlayerPowerUp player)
    {

    }

    protected override void ExitUse(PlayerPowerUp player)
    {
        player.GetPlayerController().ExitWings();
    }
}
