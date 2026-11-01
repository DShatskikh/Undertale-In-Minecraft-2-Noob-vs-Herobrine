using System.Collections;
using UnityEngine;

public sealed class FakeChest : Usable
{
    [SerializeField]
    private string _id;
    
    [SerializeField]
    private Sprite _opened;

    [SerializeField]
    private AudioSource _sfx;

    [SerializeField]
    private GameObject _effect;
    
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
            yield return DialogueWindow.StartDialogue(new []
            {
                "(Ничего не произошло.)",
            }, false);
            
            Player.Instance.enabled = true;
            yield break;
        }

        _view.sprite = _opened;
        _sfx.Play();
        Stats.Instance.Chests.Add(_id);
        _isOpened = true; 
        _effect.SetActive(true);
        
        yield return new WaitForSeconds(1);
        Player.Instance.enabled = true;
    }
}
