using SGM;
using System.Collections;
using UnityEngine;

public abstract class SO_BasePowerUp : ScriptableObject
{
    [Header("Informazioni Base PowerUp")]
    public PowerUpType PowerUp;
    public string powerUpName;
    [TextArea(3, 5)]
    public string description;
    public Sprite icon;
    public int cost;
    public Level level;

    [Header("Timer Ricarica")]
    public float rechargeTime = 10f;
    IEnumerator timer;

    public IEnumerator Timer() // <- coroutine per il timer di ricarica
    {
        float timer = rechargeTime;
        Debug.Log($"Iniziando ricarica per {powerUpName} - {timer} secondi");

        EnterUse();
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
            StayUse();
        }
        yield return null;
        ExitUse();
        Debug.Log($"Ricarica completata per {powerUpName}");
    }

    public void Use(PlayerPowerUp player)
    {
        if (timer != null)
        {
            timer = Timer();
            player.StartCoroutine(timer);
        }
    }

    protected abstract void EnterUse();
    protected abstract void StayUse();
    protected abstract void ExitUse();
}