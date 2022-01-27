using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DonePanel : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    TMPro.TextMeshProUGUI text;
    public bool IsDone => canvasGroup.interactable;
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
    }
    // Start is called before the first frame update
    public void Show(bool win)
    {
        if (win)
        {
            text.SetText("Success!");
        }
        else
        {
            text.SetText($"Word:\n{GameManager.Instance.TargetWord.ToUpper()}");
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        Wrj.Utils.MapToCurve.Linear.ManipulateFloat(SetAlpha, 0f, 1f, 1f);
    }

    void SetAlpha(float val)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = val;
        }
    }
}
