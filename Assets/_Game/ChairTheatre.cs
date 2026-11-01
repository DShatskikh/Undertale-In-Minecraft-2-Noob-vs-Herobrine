using System;
using System.Collections;
using UnityEngine;

public sealed class ChairTheatre : Usable
{
    [SerializeField]
    private bool _isNeed;
    
    public override IEnumerator AwaitUse()
    {
        Player.Instance.enabled = false;
        
        if (!_isNeed)
        {
            yield return DialogueWindow.StartDialogue(new[]
            {
                "(Это не ваше место.)"
            }, false);
            
            Player.Instance.enabled = true;
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new[]
            {
                "(Тут надпись с вашим ником.)"
            }, false);

            yield return SelectWindow.StartSelect("Сесть?", "Да", "Нет");

            if (!SelectWindow.IsRight)
            {
                FindAnyObjectByType<Theatre>().StartShow();
            }
            else
            {
                Player.Instance.enabled = true;
            }
        }
    }
}
