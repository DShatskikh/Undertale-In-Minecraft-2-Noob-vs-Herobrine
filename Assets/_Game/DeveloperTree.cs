using System.Collections;
using UnityEngine;

public sealed class DeveloperTree : Usable
{
    [SerializeField]
    private Material _material;
    
    [SerializeField]
    private SpriteRenderer[] _spriteRenderers;

    [SerializeField]
    private AudioSource _drop;
    
    [SerializeField]
    private AudioSource _give;
    
    public override IEnumerator AwaitUse()
    {
        if (Stats.Instance.Data[7] == 1)
        {
            Player.Instance.enabled = false;

            yield return DialogueWindow.StartDialogue(new []
            {
                "У тебя есть что-то для меня?",
                "Хорошо.",
            }, false);

            _drop.Play();
            
            yield return DialogueWindow.StartDialogue(new []
            {
                "(Вы потеряли <Color=\"yellow\">Сломанный плакат</Color>.)",
                "Хм...",
                "Наверное это какая-то ошибка.",
                "Я его починил.",
                "Секунду.",
                "...",
                "Держи.",
            }, false);
            
            _give.Play();
            
            yield return DialogueWindow.StartDialogue(new []
            {
                "(Вы получили <Color=\"yellow\">Изначальный плакат</Color>.)",
                "Пройди игру еще раз и увидишь <Color=\"yellow\">Истинную концовку</Color>.",
                "Теперь история в твоих руках.",
                "Если захочешь завершить игру просто поговори со мной.",
            }, false);
            
            // foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
            // {
            //     spriteRenderer.material = _material;
            // }
            
            Player.Instance.enabled = true;
        }
        else
        {
            Player.Instance.enabled = false;
            
            yield return DialogueWindow.StartDialogue(new []
            {
                "(Нет проблем.)",
            }, false);
            
            Player.Instance.enabled = true;
        }
    }
}
