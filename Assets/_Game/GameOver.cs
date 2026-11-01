using System.Collections;
using Febucci.TextAnimatorForUnity;
using TMPro;
using UnityEngine;

public sealed class GameOver : MonoBehaviour
{
    [SerializeField]
    private TypewriterComponent _titleLabel;
    
    [SerializeField]
    private TextMeshPro _messageLabel;

    [SerializeField]
    private SpriteRenderer _icon;
    
    [SerializeField]
    private GameOverButton _button;

    [SerializeField]
    private AudioClip _sfx;

    [SerializeField]
    private SpriteRenderer _blackout;
    
    private bool _isClicked;
    
    private IEnumerator Start()
    {
        MusicManager.Instance.Stop();
        _messageLabel.color = _messageLabel.color.SetA(0);
        _icon.color = _icon.color.SetA(0);
        
        _titleLabel.ShowText("Нубик, не теряй решимость!");
        _titleLabel.StartShowingText();
        yield return new WaitUntil(() => !_titleLabel.IsShowingText);

        while (_messageLabel.color.a < 1)
        {
            _messageLabel.color = _messageLabel.color.AddA(Time.deltaTime / 2);
            _icon.color = _icon.color.SetA(_messageLabel.color.a);
            yield return null;
        }
        
        yield return _button.AwaitShow();

        while (true)
        {
            if (InputManager.Instance.IsSubmitDown)
            {
                Next();
            }
            
            yield return null;
        }
    }

    public void Next()
    {
        if (_isClicked)
            return;
        
        _isClicked = true;
        StartCoroutine(AwaitNext());
        MusicManager.Instance.PlaySound(_sfx);
    }
    
    private IEnumerator AwaitNext()
    {
        while (_blackout.color.a < 1)
        {
            _blackout.color = _blackout.color.AddA(Time.deltaTime);
            yield return null;
        }

        Stats.Instance.LevelName = Stats.Instance.Data[3] switch
        {
            0 => "Spawn (0, 0)",
            1 => "Home",
            2 => "Dungeos Hub",
            3 => "Castle 1",
        };
        
        Stats.Instance.Position = Stats.Instance.Data[3] switch
        {
            0 => new Vector3(0.400000006f,-4.26000023f,0),
            1 => new Vector3(-2.20912743f, -6.60043097f, 0),
            2 =>new Vector3(0f, 0f, 0),
            3 => new Vector3(0f, 0f, 0),
        };
        
        SwitchingLevel.SwitchingNotWorld();
    }
}
