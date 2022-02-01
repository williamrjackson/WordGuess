using UnityEngine;

public class WordRowManager : MonoBehaviour
{
    [SerializeField]
    WordRow[] wordRows;
    private int _currentRowIndex = 0;

    public static WordRowManager Instance;
    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple WordRowManager's instantiated. Component removed from " + gameObject.name + ". Instance already found on " + Instance.gameObject.name + "!");
            Destroy(this);
        }
    }
    void Start()
    {
        Keyboard.Instance.OnLetter += LetterRecd;
        Keyboard.Instance.OnEnter += EnterRecd;
        Keyboard.Instance.OnBackspace += BackspaceRecd;
    }

    private void BackspaceRecd()
    {
        wordRows[_currentRowIndex].Back();
    }

    private void EnterRecd()
    {
        if (DonePanel.Instance.IsDone && !GameManager.Instance.DailyWordMode)
        {
            ReloadScene.Instance.Reload();
        }

        var result = wordRows[_currentRowIndex].ValidateWord();
        if (result == WordRow.WordValidationResults.NotAWord)
        {
            Debug.Log("NotAWord");
            GameManager.Instance.WordNotFound();
            return;
        }
        else if (result == WordRow.WordValidationResults.Correct)
        {
            Debug.Log("Match");
            GameManager.Instance.GameOver(true, true);
        }
        else if (wordRows[_currentRowIndex].Word.Length == 5)
        {
            if (_currentRowIndex == wordRows.Length - 1)
            {
                GameManager.Instance.GameOver(false, true);
            }
            else
            {
                _currentRowIndex++;
                wordRows[_currentRowIndex].SetRowActive();
            }
        }
    }

    private void LetterRecd(char letter)
    {
        wordRows[_currentRowIndex].AddLetter(letter);
    }
    public static void ActivateDefaultRow()
    {
        Instance.wordRows[Instance._currentRowIndex].SetRowActive();
    }
    public static void SaveWords()
    {
        for (int i = 0; i < Instance.wordRows.Length; i++)
        {
            PlayerPrefs.SetString($"word{i}", Instance.wordRows[i].Word);
        }
    }
    public static void LoadSavedWords()
    {
        for (int i = 0; i < Instance.wordRows.Length; i++)
        {
            string word = PlayerPrefs.GetString($"word{i}", "");
            Instance.wordRows[i].Word = word;
            Instance.wordRows[i].ValidateWord();
        }
    }
}
