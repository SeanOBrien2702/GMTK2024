using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    
    public void StartGameButton()
    {
        SceneManager.LoadScene("GameScene");
    }   

 
    
    public void QuitButtion()
    {
        Application.Quit();
    }
}