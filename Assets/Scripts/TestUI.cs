using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestUI : MonoBehaviour
{
    public void UnloadScene()
    {
        SceneManager.UnloadScene("TestMiniGameScene");
        GameManager.Instance.IsGamePaused = false;
    }
}
