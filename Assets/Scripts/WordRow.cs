using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordRow : MonoBehaviour
{
    [SerializeField]
    LetterBox[] letterBoxes;
    private int _currentLetterIndex = 0;

    public enum WordValidationResults {NotAWord, Incorrect, Correct}

    public string Word
    {
        get
        {
            string result = string.Empty;
            for (int i = 0; i < letterBoxes.Length; i++)
            {
                if (letterBoxes[i].Letter == ' ')
                {
                    return string.Empty;
                }
                result += letterBoxes[i].Letter;
            }
            return result.ToUpper();
        }
    }
    public void SetRowActive()
    {
        foreach (var item in letterBoxes)
        {
            item.State = LetterState.Active;
        }
    }
    public WordValidationResults SetRowCompleted()
    {
        int matchCount = 0;
        foreach (var item in letterBoxes)
        {
            if (item.State == LetterState.PositionMatched)
            {
                matchCount++;
            }
            if (item.State == LetterState.Active)
            {
                item.State = LetterState.Completed;
            }
            Keyboard.Instance.LetterFeedbackState(item.Letter, item.State);
        }
        if (matchCount == letterBoxes.Length)
        {
            return WordValidationResults.Correct;
        }
        return WordValidationResults.Incorrect;
    }
    public void AddLetter(char letter)
    {
        if (_currentLetterIndex >= letterBoxes.Length)
            return;
        
        letterBoxes[_currentLetterIndex].SetLetter(letter);
        _currentLetterIndex++;

    }

    internal void Back()
    {
        if (_currentLetterIndex > 0)
        {
            _currentLetterIndex--;
            letterBoxes[_currentLetterIndex].SetLetter(' ');
        }
    }

    internal WordValidationResults ValidateWord()
    {
        if (!string.IsNullOrWhiteSpace(Word))
        {
            if (!Wrj.WordList.CheckWord(Word))
            {
                Debug.Log("Invalid Word");
                return WordValidationResults.NotAWord;
            }
            List<char> word = new List<char>(Word);
            List<char> targetWord = new List<char>(GameManager.Instance.TargetWord.ToUpper());
            for (int i = Word.Length - 1; i >= 0; i--)
            {
                char thisLetter = Word[i];
                char targetLetter = targetWord[i];

                if (thisLetter == targetLetter)
                {
                    letterBoxes[i].State = LetterState.PositionMatched;
                    targetWord.RemoveAt(i);
                }
            }
            while (targetWord.Count > 0)
            {
                char c = targetWord[0];
                targetWord.RemoveAt(0);
                for (int i = 0; i < Word.Length; i++)
                {
                    if (Word[i] == c && letterBoxes[i].State != LetterState.PositionMatched)
                    {
                        letterBoxes[i].State = LetterState.Matched;
                        break;
                    }
                }
            }
            return SetRowCompleted();
        }
        return WordValidationResults.Incorrect;
    }
}
