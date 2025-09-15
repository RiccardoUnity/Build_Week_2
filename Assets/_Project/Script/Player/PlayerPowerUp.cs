using UnityEngine;
using UnityEngine.Events;
using Save = SGM.S_SaveManager;

[RequireComponent(typeof(PlayerController))]
public class PlayerPowerUp : MonoBehaviour
{
    private PlayerController _playercontroller;

    [SerializeField] SO_DoubleCoins[] _doubleCoins;
    public UnityEvent doubleCoinsEvent;

    [SerializeField] SO_Wings[] _wings;
    public UnityEvent wingsEvent;

    [SerializeField] SO_SlowTime[] _slowTime;
    public UnityEvent slowTimeEvent;

    void Awake()
    {
        _playercontroller = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (_playercontroller.IsAlive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                foreach (var powerUp in _doubleCoins)
                {
                    if ((int)powerUp.level == Save.powerUp.DoubleCoinsLevel)
                    {
                        powerUp.Use(this);
                        DoubleCoinsEvent();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                foreach (var powerUp in _wings)
                {
                    if ((int)powerUp.level == Save.powerUp.WingsLevel)
                    {
                        powerUp.Use(this);
                        WingsEvent();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                foreach (var powerUp in _slowTime)
                {
                    if ((int)powerUp.level == Save.powerUp.SlowTimeLevel)
                    {
                        powerUp.Use(this);
                        SlowTimeEvent();
                    }
                }
            }
        }
    }

    private void DoubleCoinsEvent() => doubleCoinsEvent?.Invoke();

    private void WingsEvent() => wingsEvent?.Invoke();

    private void SlowTimeEvent() => slowTimeEvent?.Invoke();
}
