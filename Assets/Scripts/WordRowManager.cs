using UnityEngine;

public class WordRowManager : MonoBehaviour
{
    [SerializeField]
    WordRow[] wordRows;
    private int _currentRowIndex = 0;

    void Start()
    {
        Keyboard.Instance.OnLetter += LetterRecd;
        Keyboard.Instance.OnEnter += EnterRecd;
        Keyboard.Instance.OnBackspace += BackspaceRecd;
        wordRows[_currentRowIndex].SetRowActive();
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
}
