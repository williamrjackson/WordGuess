using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(Button))]
public class KeyInteraction : MonoBehaviour
{
    public enum KeyType {Letter, Backspace, Enter }
    [SerializeField]
    private KeyType keyType = KeyType.Letter;
    EventTrigger _trigger;
    TextMeshProUGUI _characterTmp;
    char _character;
    public char Character => _character;
    private Image _background;
    private Button _button;
    private LetterState _state = LetterState.Active;
    private bool _stateInitialized = false;
    public LetterState State
    {
        set
        {
            if (keyType != KeyType.Letter) return;
            _stateInitialized = true;
            _state = value;
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
    void Start()
    {
        _characterTmp = transform.GetComponentInChildren<TextMeshProUGUI>();
        if (keyType == KeyType.Enter)
        {
            _character = '+';
        }
        else if (keyType == KeyType.Backspace)
        {
            _character = '-';
        }
        else
        {
            _character = _characterTmp.text[0];
        }
        _background = GetComponent<Image>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(MouseUp);
        Keyboard.Instance.AddKey(this);
        if (!_stateInitialized) State = _state;
    }

    public void UpdateFeedbackColor(LetterState state)
    {
        if (_state > state) return;
        State = state;
    }
    private void MouseDown(BaseEventData arg0)
    {
        transform.localScale = Vector3.one * 1.5f;
    }

    public void MouseUp()
    {
        switch (keyType)
        {
            case KeyType.Letter:
                Keyboard.Instance.RegisterLetter(_character);
                break;
            case KeyType.Backspace:
                Keyboard.Instance.RegisterBackspace();
                break;
            case KeyType.Enter:
                Keyboard.Instance.RegisterEnter();
                break;
            default:
                break;
        }
        transform.localScale = Vector3.one;
    }
}
