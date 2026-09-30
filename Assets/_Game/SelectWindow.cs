using System.Collections;
using Febucci.TextAnimatorCore.Text;
using Febucci.TextAnimatorForUnity;
using UnityEngine;

public sealed class SelectWindow : MonoBehaviour
{
    public static SelectWindow Instance;
    public static bool IsRight;

    [SerializeField]
    private TypewriterComponent _label;
    
    [SerializeField]
    private TypewriterComponent _yesLabel;
    
    [SerializeField]
    private TypewriterComponent _noLabel;
    
    [SerializeField]
    private GameObject _heart;

    [SerializeField]
    private TextButton[] _buttons;

    [SerializeField]
    private AudioSource _sfx;
    
    [SerializeField]
    private AudioSource _selectSFX;
    
    private bool _isEndWriting;
    private bool _isRight;

    private void Awake()
    {
        _buttons[0].Select = () => Select(false);
        _buttons[0].Click = () => Click(false);
        _buttons[1].Select = () => Select(true);
        _buttons[1].Click = () => Click(true);
    }

    private void Update()
    {
        if (!_isEndWriting)
            return;
        
        if (InputManager.Instance.HorizontalDown)
        {
            if (InputManager.Instance.Horizontal > 0 && !_isRight)
            {
                Select(true);
            }
            else if (InputManager.Instance.Horizontal < 0 && _isRight)
            {
                Select(false);
            }
        }

        if (InputManager.Instance.IsSubmitDown)
        {
            Click(_isRight);
        }

        if (!_isRight)
        {
            _heart.transform.localPosition = new Vector3(-4.96f, -0.37f, 0f);
        }
        else
        {
            _heart.transform.localPosition = new Vector3(0f, -0.37f, 0f);
        }
    }

    public static IEnumerator StartSelect(string text, string yesText, string noText, bool isDown = false)
    {
        var dialogueWindow = Instantiate(Resources.Load<SelectWindow>("Select Window"),
            new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + (isDown ? -2f : 2.884f)),
            Camera.main.transform.rotation);

        Instance = dialogueWindow;
        yield return dialogueWindow.AwaitWrite(text, yesText, noText);
        yield return new WaitUntil(() => !Instance);
    }

    private void Select(bool isRight)
    {
        if (isRight != _isRight)
        {
            _isRight = isRight;
            _selectSFX.Play();
        }
    }

    private void Click(bool isRight)
    {
        IsRight = isRight;
        Destroy(gameObject);
    }

    private IEnumerator AwaitWrite(string text, string yesText, string noText)
    {
        _label.ShowText(text);
        _label.StartShowingText();
        _label.onCharacterVisible.AddListener(OnWrite);
        yield return new WaitUntil(() => !_label.IsShowingText);
        
        _yesLabel.ShowText(yesText);
        _yesLabel.StartShowingText();
        yield return new WaitUntil(() => !_yesLabel.IsShowingText);
        
        _noLabel.ShowText(noText);
        _noLabel.StartShowingText();
        yield return new WaitUntil(() => !_noLabel.IsShowingText);
        
        _isEndWriting = true;
    }

    private void OnWrite(CharacterData arg0)
    {
        _sfx.Play();
    }
}
