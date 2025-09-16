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

    public IEnumerator Timer(PlayerPowerUp player) // <- coroutine per il timer di ricarica
    {
        float timer = rechargeTime * (int)level;
        Debug.Log($"Iniziando ricarica per {powerUpName} - {timer} secondi");

        EnterUse(player);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
            StayUse(player);
        }
        yield return null;
        ExitUse(player);
        Debug.Log($"Ricarica completata per {powerUpName}");
    }

    public void Use(PlayerPowerUp player)
    {
        if (timer != null)
        {
            timer = Timer(player);
            player.StartCoroutine(timer);
        }
    }

    protected abstract void EnterUse(PlayerPowerUp player);
    protected abstract void StayUse(PlayerPowerUp player);
    protected abstract void ExitUse(PlayerPowerUp player);
}