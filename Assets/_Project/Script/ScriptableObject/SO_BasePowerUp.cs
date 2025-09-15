using UnityEngine;
using System;
using System.Collections;

public enum Key
{
    One,
    Two,
    Three
}

public enum Level
{
    One,
    Two,
    Three
}

public enum PowerUp
{
    DoubleCoin,
    Wings,
    SlowTime
}

public abstract class SO_BasePowerUp : ScriptableObject
{
    [Header("Informazioni Base PowerUp")]
    public PowerUp PowerUp;
    public string powerUpName;
    [TextArea(3, 5)]
    public string description;
    public Sprite icon;
    public int cost;
    public Level level;
    public Key inputKey;

    [Header("Timer Ricarica")]
    public float rechargeTime = 10f;

    public IEnumerator TimerRecharge(float timer, Action callback) // <- coroutine per il timer di ricarica
    {
        Debug.Log($"Iniziando ricarica per {powerUpName} - {timer} secondi");

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        Debug.Log($"Ricarica completata per {powerUpName}");

        callback?.Invoke();
    }

    public abstract void Use();
}