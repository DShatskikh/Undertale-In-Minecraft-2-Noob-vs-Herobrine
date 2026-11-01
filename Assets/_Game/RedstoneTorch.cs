using System;
using System.Collections;
using UnityEngine;

public sealed class RedstoneTorch : Usable
{
    [SerializeField]
    private SpriteRenderer[] _torches;

    [SerializeField]
    private Sprite _activate;

    [SerializeField]
    private GameObject _switchingLevel, _rDoor, _rOpenDoor;
    
    private void Update()
    {
        if (Stats.Instance.Data[41] == 1)
        {
            _torches[0].sprite = _activate;  
        }
        
        if (Stats.Instance.Data[42] == 1)
        {
            _torches[1].sprite = _activate;  
        }
        
        if (Stats.Instance.Data[43] == 1)
        {
            _torches[2].sprite = _activate;  
        }

        if (Stats.Instance.Data[41] == 1 && Stats.Instance.Data[42] == 1 && Stats.Instance.Data[43] == 1)
        {
            _switchingLevel.SetActive(true);
            _rDoor.SetActive(false);
            _rOpenDoor.SetActive(true);
        }
    }

    public override IEnumerator AwaitUse()
    {
        Player.Instance.enabled = false;
        
        if (Stats.Instance.Data[41] == 1 && Stats.Instance.Data[42] == 1 && Stats.Instance.Data[43] == 1)
        {
            yield return DialogueWindow.StartDialogue(new []
            {
                "/n0Все 3 факела.*Можно идти."
            }, true);
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new []
            {
                "/n0Нужно чтобы загорелось 3 факела."
            }, true);
        }
        
        Player.Instance.enabled = true;
    }
}
