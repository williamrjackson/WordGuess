using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterBox : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;
    UnityEngine.UI.Image _background;

    private LetterState _state = LetterState.Inactive;
    private bool _stateInitialized = false;
    public LetterState State
    {
        set
        {
            _state = value;
            _stateInitialized = true;
            switch (_state)
            {
                case LetterState.Active:
                    _background.color = GameManager.Instance.ActiveRowColor;
                    break;
                case LetterState.Completed:
                    _background.color = GameManager.Instance.CompletedRowColor;
                    break;
                case LetterState.Matched:
                    _background.color = GameManager.Instance.MatchedLetterColor;
                    break;
                case LetterState.PositionMatched:
                    _background.color = GameManager.Instance.PositionMatchedLetterColor;
                    break;
                default:
                    _background.color = GameManager.Instance.InactiveRowColor;
                    break;
            }
        }
        get => _state;
    }
    public char Letter => string.IsNullOrWhiteSpace(text.text) ? ' ' : text.text[0];
    private void Awake()
    {
        _background = GetComponent<UnityEngine.UI.Image>();
        text.SetText(" ");
    }
    private void Start()
    {
        if (!_stateInitialized) State = _state;
    }
    public void SetLetter(char letter)
    {
        text.SetText(letter.ToString());
    }
}
