using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayNextButton : MonoBehaviour
{
    [SerializeField] private GameObject _nextButton;

    private void OnEnable()
    {
        TypewriterEffect.CompleteTextRevealed += ShowNextButton;
    }

    private void OnDisable()
    {
        TypewriterEffect.CompleteTextRevealed -= ShowNextButton;
    }

    private void ShowNextButton()
    {
        _nextButton.SetActive(true);
    }

    public void HideNextButton()
    {
        _nextButton.SetActive(false);
    }
}
