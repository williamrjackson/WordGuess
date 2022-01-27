using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    // Start is called before the first frame update
    void Start()
    {
        PickRandomWord();
    }
    void PickRandomWord()
    {
        TargetWord = Wrj.WordList.RandomWord();
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
