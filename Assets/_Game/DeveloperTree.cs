using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            if (Stats.Instance.Data[55] == 1)
            {
                yield return SelectWindow.StartSelect("Уже всё?", "Да", "Нет");

                if (!SelectWindow.IsRight)
                {
                    yield return DialogueWindow.StartDialogue(new []
                    {
                        "Хорошо.",
                        "Тогда тогда тебе пора."
                    }, false);

                    Meta.Instance.IsEndEvil = true;
                    Stats.Instance.Reset();
                    SceneManager.LoadScene("Menu");
                }
                else
                {
                    yield return DialogueWindow.StartDialogue(new []
                    {
                        "Я тебя подожду."
                    }, false);
                }
                
                yield break;
            }
            
            yield return DialogueWindow.StartDialogue(new []
            {
                "У тебя есть что-то для меня?",
                "Хорошо.",
            }, false);

            _drop.Play();
            
            yield return DialogueWindow.StartDialogue(new []
            {
                "(Вы потеряли <Color=\"yellow\">Забагованный билет</Color>.)",
                "Хм...",
                "Наверное это какая-то ошибка.",
                "Тут написанна какая-то бесмыслица.",
                "Секундочку.",
                "...",
                "Вот.*Я его починил.*Держи.",
            }, false);
            
            _give.Play();
            
            yield return DialogueWindow.StartDialogue(new []
            {
                "(Вы получили <Color=\"yellow\">Изначальный плакат</Color>.)",
                "Пройди игру еще раз и увидишь <Color=\"yellow\">Истинную концовку</Color>.",
                "Теперь история в твоих руках.",
                "Если захочешь завершить игру просто поговори со мной снова.",
            }, false);

            Stats.Instance.Data[55] = 1;
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
