using System;
using System.Collections;
using UnityEngine;

public abstract class BannedUsable : Usable
{
    protected abstract int _id { get; }
    protected abstract string _message { get; }
    protected GameObject _gameObject;

    protected virtual void Start()
    {
        _gameObject = gameObject;
        
        if (Stats.Instance.Data[_id] == 1)
        {
            gameObject.SetActive(false);
        }
    }

    public override IEnumerator AwaitUse()
    {
        CoroutineRunner.Instance.StartCoroutine(AwaitBan());
        yield break;
    }

    protected IEnumerator AwaitBan()
    {
        Player.Instance.enabled = false;
        yield return DialogueWindow.StartDialogue(new[] { _message }, false);
        yield return Player.Instance.WaitAttack();
        Stats.Instance.Data[_id] = 1;
        _gameObject.SetActive(false);
        
        bool isEnd = true;
        
        for (int i = 11; i < 15; i++)
        {
            if (Stats.Instance.Data[i] != 1)
            {
                isEnd = false;
            } 
        }

        if (isEnd)
        {
            Stats.Instance.Data[7] = 1;
            FindAnyObjectByType<BlackAndWhiteWorld>().Activate();
            
            yield return new WaitForSeconds(1);

            yield return DialogueWindow.StartDialogue(new []
            {
                "Поздравляю! У тебя получилось попасть туда куда нельзя попасть.",
                "Я буду ждать тебя за деревом."
            }, false);
        }
        
        Player.Instance.enabled = true;
    }
}
