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
        
        Player.Instance.enabled = false;

        if (Stats.Instance.Data[7] == 1)
        {
            yield return DialogueWindow.StartDialogue(new[] { "(Ничего не произошло.)" }, false);
        }
        else
        {
            if (Stats.Instance.TryAddItem(_item))
            {
                _view.sprite = _opened;
                _sfx.Play();
            
                var itemName = _item switch
                {
                    "Steak" => "Стейк",
                    "Potion" => "Зелье лечения",
                    "GoldApple" => "Золотое яблоко",
                    _ => "Что-то...",
                };
            
                yield return DialogueWindow.StartDialogue(new[]
                {
                    $"(Вы получили <color=yellow>{itemName}</color>.)"
                }, false);
            
                Stats.Instance.Chests.Add(_id);
                _isOpened = true;  
            }
            else
            {
                yield return DialogueWindow.StartDialogue(new[]
                {
                    "(У вас нету места.)"
                }, false);
            }
        }
        
        Player.Instance.enabled = true;
    }
}
