using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SGM;

public class PowerUpShop_manager : MonoBehaviour
{
    [System.Serializable]
    public class PowerUpButton
    {
        public PowerUpType _powerUpType;
        public Button _button;
    }
    public PowerUpButton[] _powerUpButtons;

    private Color originalColor = Color.white;


    private void Start()
    {

        foreach (var pb in _powerUpButtons)
        {
            if (pb._button != null)
            {
                Image img = pb._button.GetComponent<Image>();
                img.color = originalColor;

                EventTrigger trigger = pb._button.gameObject.GetComponent<EventTrigger>();
                if (trigger == null)
                    trigger = pb._button.gameObject.AddComponent<EventTrigger>();

                EventTrigger.Entry entryEnter = new EventTrigger.Entry();
                entryEnter.eventID = EventTriggerType.PointerEnter;
                entryEnter.callback.AddListener((data) => { img.color = GetColorForPowerUp(pb._powerUpType); });
                trigger.triggers.Add(entryEnter);

                EventTrigger.Entry entryExit = new EventTrigger.Entry();
                entryExit.eventID = EventTriggerType.PointerExit;
                entryExit.callback.AddListener((data) => { img.color = originalColor; });
                trigger.triggers.Add(entryExit);
            }
        }
    }

    private Color GetColorForPowerUp(PowerUpType type)
    {
        if (S_SaveManager.powerUp == null)
            S_SaveManager.GetPowerUp();

        int level = type switch
        {
            PowerUpType.DoubleCoin => S_SaveManager.powerUp.DoubleCoinsLevel,
            PowerUpType.Wings => S_SaveManager.powerUp.WingsLevel,
            PowerUpType.SlowTime => S_SaveManager.powerUp.SlowTimeLevel,
            _ => 0
        };

        return level switch
        {
            1 => Color.green,
            2 => Color.blue,
            3 => Color.yellow,
            _ => Color.grey
        };
    }
}

