using UnityEngine;
using UnityEngine.SceneManagement;
using Save = SGM.S_SaveManager;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private GameObject _ui;
    private bool _stayVisible;

    void Awake()
    {
        if (!_stayVisible)
        {
            gameObject.SetActive(false);
            _ui.SetActive(true);
        }
    }

    public void GameOverEvent()
    {
        _stayVisible = true;
        gameObject.SetActive(true);
        _ui.SetActive(false);
        Save.SaveRecord();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
