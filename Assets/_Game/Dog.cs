using System;
using System.Collections;
using UnityEngine;

public sealed class Dog : Usable
{
    [SerializeField]
    private GameObject _tear;

    private void Start()
    {
        if (Stats.Instance.Data[37] != 0)
            _tear.SetActive(true);
    }

    public override IEnumerator AwaitUse()
    {
        Player.Instance.enabled = false;

        if (Stats.Instance.Data[37] == 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            yield return DialogueWindow.StartDialogue(new[] { "/d0<b>Гав." }, false);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new[] { "/d1<b>..." }, false);
        }
        
        Player.Instance.enabled = true;
    }
}
