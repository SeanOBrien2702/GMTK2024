using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] List<AudioClip> musicList;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        audioSource.clip = musicList[SceneManager.GetActiveScene().buildIndex];

    }
}