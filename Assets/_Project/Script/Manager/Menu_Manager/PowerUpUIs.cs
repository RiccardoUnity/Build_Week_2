using SGM;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Save = SGM.S_SaveManager;

public class PowerUpUIs : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] SO_BasePowerUp _sO_BasePowerUpsLevel1;
    [SerializeField] SO_BasePowerUp _sO_BasePowerUpsLevel2;
    [SerializeField] SO_BasePowerUp _sO_BasePowerUpsLevel3;

    [SerializeField] TextMeshProUGUI _powerUpText;

    [SerializeField] Button _powerUpButton;

    private int _levelIndex;
    private PowerUpType _type;
    [SerializeField] private TMP_Text _coinNumber;

    void Start()
    {
        Save.GetPowerUp();
        _type = _sO_BasePowerUpsLevel1.PowerUp;
        switch (_type)
        {
            case PowerUpType.DoubleCoin:
                _levelIndex = Save.powerUp.DoubleCoinsLevel;
                break;
            case PowerUpType.Wings:
                _levelIndex = Save.powerUp.WingsLevel;
                break;
            case PowerUpType.SlowTime:
                _levelIndex = Save.powerUp.SlowTimeLevel;
                break;
        }
        
        _powerUpText.text = $"Click to level up!!";

    }

    public void PowerUponClick()
    {
        switch (_levelIndex)
        {
            case 0:
                if (_sO_BasePowerUpsLevel1.cost <= Save.GetCoin())
                {
                    Increase(_sO_BasePowerUpsLevel1.cost);
                }
                break;
            case 1:
                if (_sO_BasePowerUpsLevel2.cost <= Save.GetCoin())
                {
                    Increase(_sO_BasePowerUpsLevel2.cost);
                }
                break;
            case 2:
                if (_sO_BasePowerUpsLevel3.cost <= Save.GetCoin())
                {
                    Increase(_sO_BasePowerUpsLevel3.cost);
                }
                break;
        }
    }

    private void Increase(int cost)
    {
        Save.SaveCoin(-cost);
        _coinNumber.text = Save.GetCoin().ToString();
        ++_levelIndex;
        Save.powerUp.IncreasePowerUp(_type);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (_levelIndex)
        {
            case 0:
                _powerUpText.text = $"Cost: {_sO_BasePowerUpsLevel1.cost} \nLevel: {_sO_BasePowerUpsLevel1.level}";
                break;
            case 1:
                _powerUpText.text = $"Cost: {_sO_BasePowerUpsLevel2.cost} \nLevel: {_sO_BasePowerUpsLevel2.level}";
                break;
            case 2:
                _powerUpText.text = $"Cost: {_sO_BasePowerUpsLevel3.cost} \nLevel: {_sO_BasePowerUpsLevel3.level}";
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _powerUpText.text = $"Click to level up!!";
    }
}
