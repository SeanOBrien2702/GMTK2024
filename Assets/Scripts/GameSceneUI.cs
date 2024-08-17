using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;

    private void Start()
    {
        pauseScreen.SetActive(false);
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
