using System.Collections;
using UnityEngine;
using Save = SGM.S_SaveManager;

public class TestSave : MonoBehaviour
{
    IEnumerator Start()
    {
        Save.GetPowerUp();
        //Save.powerUp.IncreasePowerUp(SGM.PowerUpType.Wings);
        yield return null;
        Debug.Log(Save.powerUp.WingsLevel);
    }
}
