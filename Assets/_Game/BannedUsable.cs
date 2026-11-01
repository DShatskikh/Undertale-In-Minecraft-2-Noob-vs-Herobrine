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
        
        for (int i = 11; i <= 21; i++)
        {
            if (Stats.Instance.Data[i] != 1)
            {
                isEnd = false;
            } 
        }

        if (isEnd)
        {
            CoroutineRunner.Instance.StartCoroutine(AwaitEnd());
        }
        else
        {
            Player.Instance.enabled = true;
        }
    }

    private IEnumerator AwaitEnd()
    {
        Stats.Instance.Data[7] = 1;
        FindAnyObjectByType<BlackAndWhiteWorld>()?.Activate();
        MusicManager.Instance.Play(Resources.Load<AudioClip>("DELTARUNE OST — man"));
            
        yield return new WaitForSeconds(1);

        yield return DialogueWindow.StartDialogue(new []
        {
            "Поздравляю! У тебя получилось попасть куда-то.",
            "Я жду тебя за деревом."
        }, false);
        
        Player.Instance.enabled = true;
    }
}
