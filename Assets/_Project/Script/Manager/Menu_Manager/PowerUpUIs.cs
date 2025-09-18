using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUIs : MonoBehaviour
{
    [SerializeField] SO_BasePowerUp _sO_BasePowerUpsLevel1;
    [SerializeField] SO_BasePowerUp _sO_BasePowerUpsLevel2;
    [SerializeField] SO_BasePowerUp _sO_BasePowerUpsLevel3;

    [SerializeField] TextMeshProUGUI _powerUpText;

    [SerializeField] Button _powerUpButton;

    private int _levelIndex;

    void Start()
    {
        _powerUpText.text = $"Click to level up!!";

    }

    public void PowerUponClick()
    {
        _levelIndex++;

        if (_levelIndex < 4)
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
        else if (_levelIndex > 3)
        {
            _powerUpButton.interactable = false;
        }

    }
}
