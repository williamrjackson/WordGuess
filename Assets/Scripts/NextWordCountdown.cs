using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class NextWordCountdown : MonoBehaviour
{
    TextMeshProUGUI _timerText;
    [SerializeField]
    CanvasGroup _canvasGroup;
    private void Awake()
    {
        _timerText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_canvasGroup.alpha > 0f)
        {
            System.DateTime tomorrow = System.DateTime.Today.ToUniversalTime().AddDays(1);
            System.TimeSpan until = tomorrow.Subtract(System.DateTime.Now.ToUniversalTime());
            _timerText.SetText(until.ToString(@"hh\:mm\:ss"));
        }
    }
}
