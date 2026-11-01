using System;
using System.Collections;
using UnityEngine;

public sealed class Snowman : Usable
{
    [SerializeField]
    private Sprite _notAngry;
    
    private void Start()
    {
        
    }

    public override IEnumerator AwaitUse()
    {
        Player.Instance.enabled = false;
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/n0Хоть бы не битва.<waitfor=0.5>*Хоть бы не битва.",
            "/l1Я злой снеговик, сейчас я надеру тебе зад, глупый нуб.",
            "/n0Стой, <waitfor=0.25>нам не обязательно сражаться.",
            "/l1Обязательно.",
            "/n0Может ты создан для чего-то большего?",
            "/n0Например для любви.",
            "/l1Любви?",
            "/n0Любовь это наше всё.",
            "/l1Ты прав.",
        }, false);
        
        // смена спрайта
        GetComponent<SpriteRenderer>().sprite = _notAngry;
        GetComponent<AudioSource>().Play();
        
        yield return new WaitForSeconds(0.25f);
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/l0Любовь... <waitfor=0.5>любовь.",
            "/l0Я создан для любви, <waitfor=0.25>а не для битв.",
        }, false);
        
        // Уходит
        GetComponent<Collider2D>().enabled = false;
        var y = -7;
        
        while (transform.position.y > y)
        {
            transform.position = transform.position.AddY(-Time.deltaTime * 2);
            yield return null;
        }
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/n0Фух, <waitfor=0.25>пронесло.",
        }, false);
        
        Player.Instance.enabled = true;
    }
}
