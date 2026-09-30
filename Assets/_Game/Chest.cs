using System.Collections;
using UnityEngine;

public sealed class Chest : Usable
{
    [SerializeField]
    private string _id;

    [SerializeField]
    private string _item;
    
    [SerializeField]
    private Sprite _opened;

    [SerializeField]
    private AudioSource _sfx;
    
    private SpriteRenderer _view;
    private bool _isOpened;

    private void Start()
    {
        _view = GetComponent<SpriteRenderer>();
        
        if (Stats.Instance.Chests.Contains(_id))
        {
            _isOpened = true;
            _view.sprite = _opened;
        }
    }

    public override IEnumerator AwaitUse()
    {
        if (_isOpened)
            yield break;
        
        _view.sprite = _opened;
        _sfx.Play();
        Player.Instance.enabled = false;
        yield return DialogueWindow.StartDialogue(new[] { "Вы открыли сундук", "Вы получили Стейк" }, false);
        Player.Instance.enabled = true;
        Stats.Instance.Chests.Add(_id);
        _isOpened = true;
    }
}
