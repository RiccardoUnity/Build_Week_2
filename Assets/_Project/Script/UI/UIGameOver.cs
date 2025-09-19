using UnityEngine;
using UnityEngine.SceneManagement;
using Save = SGM.S_SaveManager;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private GameObject _ui;

    void Awake()
    {
        gameObject.SetActive(false);
        _ui.SetActive(true);
    }

    public void GameOverEvent()
    {
        gameObject.SetActive(true);
        _ui.SetActive(false);
        Save.SaveRecord();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
