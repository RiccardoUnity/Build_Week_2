using TMPro;
using UnityEngine;
using Save = SGM.S_SaveManager;

public class PowerUpUI : MonoBehaviour
{
    [SerializeField] private GameObject _doubleCoins;
    [SerializeField] private GameObject _slowTime;
    [SerializeField] private GameObject _wings;
    [SerializeField] private TextMeshProUGUI _doubleCoinsLevel;
    [SerializeField] private TextMeshProUGUI _slowTimeLevel;
    [SerializeField] private TextMeshProUGUI _wingsLevel;
    void Start()
    {
        CheckPowerUpsLevel();
    }

    private void CheckPowerUpsLevel()
    {
        if (Save.GetPowerUp().DoubleCoinsLevel > 0)
        {
            _doubleCoinsLevel.text = Save.GetPowerUp().DoubleCoinsLevel.ToString();
        }
        else
        {
            _doubleCoins.SetActive(false);
        }
        if (Save.GetPowerUp().SlowTimeLevel > 0)
        {
            _slowTimeLevel.text = Save.GetPowerUp().SlowTimeLevel.ToString();
        }
        else
        {
            _slowTime.SetActive(false);
        }
        if (Save.GetPowerUp().WingsLevel > 0)
        {
            _wingsLevel.text = Save.GetPowerUp().WingsLevel.ToString();
        }
        else
        {
            _wings.SetActive(false);
        }
    }
}
