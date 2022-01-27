using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public enum LetterState {Inactive, Active, Completed, Matched, PositionMatched }
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI notFoundText;
    public Color ActiveRowColor;
    public Color CompletedRowColor;
    public Color InactiveRowColor;
    public Color MatchedLetterColor;
    public Color PositionMatchedLetterColor;
    public string TargetWord = "fight";
    private int _streak;
    public static GameManager Instance;
    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple GameManager's instantiated. Component removed from " + gameObject.name + ". Instance already found on " + Instance.gameObject.name + "!");
            Destroy(this);
        }
    }
    private bool _dailyWordmode = true;
    public bool DailyWordMode
    {
        get
        {
            return _dailyWordmode;
        }
        set
        {
            Debug.Log("DailyWordMode = " + value.ToString());
            DonePanel.Instance.Hide();
            _dailyWordmode = value;
            SetWord();
        }
    }
    private void Start()
    {
        SetWord();
    }
    void SetWord()
    {
        if (DailyWordMode)
        {
            GetWordByDay();
            _streak = PlayerPrefs.GetInt("consecutiveWins", 0);
            if (PlayerPrefs.GetString("lastWord", "") == TargetWord)
            {
                GameOver(_streak > 0, false);
            }
        }
        else
        {
            PickRandomWord();
        } 
    }
    void GetWordByDay()
    {
        TargetWord = Wrj.WordList.WordOfTheDay();
        // Debug.Log(TargetWord);
    }
    void PickRandomWord()
    {
        TargetWord = Wrj.WordList.RandomWord();
    }

    public void GameOver(bool win, bool addToStreak)
    {
        PlayerPrefs.SetString("lastWord", TargetWord);
        if (addToStreak)
        {
            if (win)
            {
                _streak++;
            }
            else
            {
                _streak = 0;
            }
        }
        PlayerPrefs.SetInt("consecutiveWins", _streak);
        DonePanel.Instance.Show(win, _streak);
    }

    public void WordNotFound()
    {
        Wrj.Utils.MapToCurve.Linear.ManipulateFloat(TextAlpha, 0f, 1f, .25f);
        Wrj.Utils.DeferredExecution(.5f, () =>
        {
            Wrj.Utils.MapToCurve.Linear.ManipulateFloat(TextAlpha, 1f, 0f, .25f);
        });
    }
    private void TextAlpha(float val)
    {
        notFoundText.alpha = val;
    }   

    

}
