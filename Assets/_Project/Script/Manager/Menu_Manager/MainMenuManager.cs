using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject _mainMenuScene;
    [SerializeField] GameObject _optionMenuScene;
    [SerializeField] GameObject _shopMenuScene;

    // Start is called before the first frame update
    void Awake()
    {
        _mainMenuScene.SetActive(true);
        _optionMenuScene.SetActive(false);
        _shopMenuScene.SetActive(false);
    }

    public void OnButtonClick(string menuName)
    {
        _mainMenuScene.SetActive(false);
        _optionMenuScene.SetActive(false);
        _shopMenuScene.SetActive(false);

        switch (menuName)
        {
            case "Main":
                _mainMenuScene.SetActive(true);
                break;
            case "Options":
                _optionMenuScene.SetActive(true);
                break;
            case "Shop":
                _shopMenuScene.SetActive(true);
                break;
        }
    }
    public void SoundOnClick(AudioSource audioSource)
    {
        audioSource.Play();


    }
    public void SceneLoader(string _SceneName)
    {
        SceneManager.LoadScene(_SceneName);
    }
}
