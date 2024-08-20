using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField] Image barImage;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] TextMeshProUGUI moneyText;

    private void Start()
    {
        GameManager.OnGameTimeChanged += Instance_OnGameTimeChanged;
        GameManager.OnMoneyChanged += GameManager_OnMoneyChanged;
        //meManager.Instance.OnProgressChanged += Instance_OnProgressChanged;
        pauseScreen.SetActive(false);
    }

    private void GameManager_OnMoneyChanged(object sender, GameManager.OnMoneyChangedEvent e)
    {
        moneyText.text = e.Money.ToString();    
    }

    private void Instance_OnGameTimeChanged(object sender, GameManager.OnGameTimeChangedEvent e)
    {
        barImage.fillAmount = e.Time;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseScreen();
        }
    }

    void TogglePauseScreen()
    {
        pauseScreen.SetActive(!pauseScreen.activeSelf);
        GameManager.Instance.IsGamePaused = pauseScreen.activeSelf;
        if (MinigameManager.Instance != null) MinigameManager.Instance.isPaused = pauseScreen.activeSelf;
    }

    public void ResumeButton()
    {
        TogglePauseScreen();
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
