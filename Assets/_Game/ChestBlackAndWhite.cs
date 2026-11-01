using System.Collections;
using UnityEngine;

public sealed class ChestBlackAndWhite : Usable
{
    [SerializeField]
    private Sprite _opened;

    [SerializeField]
    private AudioSource _sfx;
    
    [SerializeField]
    private Material _material;
    
    private SpriteRenderer _view;
    private bool _isOpened;

    private void Start()
    {
        _view = GetComponent<SpriteRenderer>();
        
        if (Stats.Instance.OrdinaryItems.Contains("Ticket"))
        {
            _isOpened = true;
            _view.sprite = _opened;
            
            if (Stats.Instance.Data[7] == 1)
                _view.material = _material;
        }
    }

    public override IEnumerator AwaitUse()
    {
        if (_isOpened)
            yield break;
        
        Player.Instance.enabled = false;

        if (Meta.Instance.IsEndEvil)
        {
            yield return DialogueWindow.StartDialogue(new[]
            {
                "(Ничего интересного.)"
            }, false);
        }
        else
        {
            _view.sprite = _opened;
            
            if (Stats.Instance.Data[7] == 1)
                _view.material = _material;
            
            _sfx.Play();
            yield return DialogueWindow.StartDialogue(new[]
            {
                $"(Вы получили <color=yellow>Забагованный Билет</color>.)",
                "(В ней очень странный текст.)",
                "Dcnhtxfqnt njkmrj ctujlyz cntylfg jn cfvjuj Uhjrcf!",
                "/n0Зачем я это прочитал?!"
            }, false);
        
            Stats.Instance.OrdinaryItems.Add("Ticket");
            _isOpened = true; 
        }
        
        
        Player.Instance.enabled = true;
    }
}