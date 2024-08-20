using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Xml.Serialization;
using System;
using Object = UnityEngine.Object;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{

// GENERAL FUNCTIONALITY
    private TMP_Text _storytextBox;


    private int _activeVisibleCharacterIndex;
    private Coroutine _textTypecoroutine;
    private bool _readyForNextText = true;

    private WaitForSeconds _initialDelay;
    private WaitForSeconds _interpuncuationDelay;

    [Header("Writing Settings")]
    [SerializeField] private float charactersPerSecond = 20;
    [SerializeField] private float interpunctuationDelay = 0.5f;

//SKIPPING FUNCTIONALITY
    public bool CurrentlySkipping { get; private set; }
    private WaitForSeconds _skipDelay;

    [Header("Skip Controls")]
    [SerializeField] private bool fastSkip;
    [SerializeField] [Min(1)] private int skipSpeedup = 5;

    // EVENT FUNCTIONALITY
    private WaitForSeconds _storyBoxFullEventDelay;
    [SerializeField][Range(0.1f, 0.5f)] private float sendDoneDelay = 0.25f;

    public static event Action CompleteTextRevealed;
    public static event Action<char> CharacterRevealed;



    private void Awake()
    {
        _storytextBox = GetComponent<TMP_Text>();

        _initialDelay = new WaitForSeconds(1 / charactersPerSecond);
        _interpuncuationDelay = new WaitForSeconds(interpunctuationDelay);

        _skipDelay = new WaitForSeconds(1 / (charactersPerSecond * skipSpeedup));

        _storyBoxFullEventDelay = new WaitForSeconds(sendDoneDelay);

    }

    private void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareForNewText);
    }

    private void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareForNewText);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            if ( _storytextBox.maxVisibleCharacters != _storytextBox.textInfo.characterCount - 1 )
            {
                Skip();
            }
        }
    }

    void Skip()
    {
        if (CurrentlySkipping)
        {
            return;
        }

        CurrentlySkipping = true;

        if (!fastSkip)
        {
            StartCoroutine(routine: SkipSpeedupReset());
            return;
        }

        StopCoroutine(_textTypecoroutine);
        _storytextBox.maxVisibleCharacters = _storytextBox.textInfo.characterCount;
        _readyForNextText = true; 
        CompleteTextRevealed?.Invoke();
    }

    public void PrepareForNewText(Object obj)
    {
        if (!_readyForNextText)
            return;

        _readyForNextText = false;


        if (_textTypecoroutine != null)
        {
            StopCoroutine(_textTypecoroutine);
        }
        
        _storytextBox.maxVisibleCharacters = 0;
        _activeVisibleCharacterIndex = 0;

        _textTypecoroutine = StartCoroutine(routine: Typewriter());
    }

    private IEnumerator SkipSpeedupReset()
    {
        yield return new WaitUntil(() => _storytextBox.maxVisibleCharacters == _storytextBox.textInfo.characterCount - 1 );
        CurrentlySkipping = false;
    }
    private IEnumerator Typewriter()
    {
        TMP_TextInfo textInfo = _storytextBox.textInfo;

        while (_activeVisibleCharacterIndex < textInfo.characterCount + 1) 
        {
            var lastCharacterIndex = textInfo.characterCount - 1;

            if (_activeVisibleCharacterIndex >= lastCharacterIndex)
            {
                _storytextBox.maxVisibleCharacters++;
                yield return _storyBoxFullEventDelay;
                CompleteTextRevealed?.Invoke();
                _readyForNextText = true;
                yield break;
            }


            char character = textInfo.characterInfo[_activeVisibleCharacterIndex].character;
            _storytextBox.maxVisibleCharacters++;

            if (!CurrentlySkipping &&  ( character == '?' ||  character == '.' || character == ',' || character == '!'))
            {
                yield return _interpuncuationDelay;
            }
            else
            {
                yield return CurrentlySkipping ? _skipDelay : _initialDelay;
            }
            CharacterRevealed?.Invoke(character);
            _activeVisibleCharacterIndex++;
        }
    }

}
