using System;
using System.Collections;
using UnityEngine;

public sealed class Picture : Usable
{
    [SerializeField]
    private SwitchingLevel _switchingLevel;

    [SerializeField]
    private SpriteRenderer _eyes;
    
    [SerializeField]
    private Sprite[] _sprites;

    private void Update()
    {
        if (Player.Instance.transform.position.x < -1.25f)
        {
            _eyes.sprite = _sprites[0];
        }
        else if (Player.Instance.transform.position.x > 1.25f)
        {
            _eyes.sprite = _sprites[2];
        }
        else
        {
            _eyes.sprite = _sprites[1];
        }
    }

    public override IEnumerator AwaitUse()
    {
        Player.Instance.enabled = false;
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "У каждой картины есть секрет.",
            "Приходи когда выполнишь все достижения.",
        }, true);
        
        // Войди в меня
        
        Player.Instance.enabled = true;
    }
}
