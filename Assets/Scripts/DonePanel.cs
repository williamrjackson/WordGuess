using UnityEngine;
using UnityEngine.UI;

public class DonePanel : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    TMPro.TextMeshProUGUI text;
    [SerializeField]
    Button reloadButton;
    [SerializeField]
    TMPro.TextMeshProUGUI countText;
    public bool IsDone => canvasGroup.interactable;
    private Wrj.Utils.MapToCurve.Manipulation canvasFadeRoutine;
    public static DonePanel Instance;
    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple DonePanel's instantiated. Component removed from " + gameObject.name + ". Instance already found on " + Instance.gameObject.name + "!");
            Destroy(this);
        }
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void Show(bool win, int currentStreak)
    {
        countText.gameObject.SetActive(GameManager.Instance.DailyWordMode);
        reloadButton.gameObject.SetActive(!GameManager.Instance.DailyWordMode);

        if (win)
        {
            text.SetText($"Success!\n\nStreak: {currentStreak}\n\nNext Word in:\n");
        }
        else
        {
            text.SetText($"Failed\n\nStreak: {currentStreak}\n\nNext Word in:\n");
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasFadeRoutine = Wrj.Utils.MapToCurve.Linear.ManipulateFloat(SetAlpha, 0f, 1f, 1f);
    }
    public void Hide()
    {
        if (canvasFadeRoutine.coroutine != null)
        {
            canvasFadeRoutine.Stop();
        }
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    void SetAlpha(float val)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = val;
        }
    }
}
