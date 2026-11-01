using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Cat : Usable
{
    [SerializeField]
    private AudioSource _evilSFX;

    private bool _isShowCat => Meta.Instance.IsEndOldSpawn && (Meta.Instance.IsEndPacifist || Meta.Instance.IsEndGenocide);
    
    private IEnumerator Start()
    {
        if (Stats.Instance.Data[6] == 1 || !_isShowCat || Stats.Instance.Data[90] == 1)
        {
            gameObject.SetActive(false);
            yield break;
        }
        
        Player.Instance.enabled = false;
        Player.Instance.SetFlex(true);
        
        yield return null;

        Player.Instance.transform.position = new Vector3(0.540000021f, 0, 0);

        if (!Meta.Instance.IsEndEvil)
        {
            MusicManager.Instance.Stop();

            yield return null;

            yield return DialogueWindow.StartDialogue(new[]
            {
                "/c0<b>Мяу.",
                "/n0Котик?",
                "/c0Ты прячешься от своего могущества.",
                "/n0Котик, что ты такое мяукаешь?",
                "/c0Возьми <Color=\"red\">Палку Админа</Color> и забань всех игроков.",
                "/n0Нет я не буду этого делать!",
                "/c0Покажи им своë МОГУЩЕСТВО!",
            }, false);

            // Выбор
            yield return SelectWindow.StartSelect("Встать на путь <Color=\"red\">ЗЛА</Color>?", "Да", "Нет");

            if (!SelectWindow.IsRight)
            {
                yield return AwaitEvil();
            }
            else
            {
                yield return DialogueWindow.StartDialogue(new[]
                {
                    "(Вы сохранили свою доброту, <waitfor=1.0>но потеряли новый контент.)",
                    "(Вы можете встать на путь зла в следующий раз.)"
                }, false);
            }
        }
        else
        {
            yield return null;
            
            yield return DialogueWindow.StartDialogue(new[]
            {
                "/n0Котик пройди за меня игру.",
                "/c0Мяу?",
            }, false);

            StartCoroutine(AwaitUse());
            yield break;
        }

        Player.Instance.enabled = true;
    }

    public override IEnumerator AwaitUse()
    {
        if (Meta.Instance.IsEndEvil)
        {
            Player.Instance.enabled = false;
            
            yield return SelectWindow.StartSelect("Перейти сразу к новой концовке?", "Да", "Нет");

            if (!SelectWindow.IsRight)
            {
                Stats.Instance.Data[90] = 1;
                Stats.Instance.Data[36] = 1;
                SceneManager.LoadScene("Outro 2");
            }
            else
            {
                yield return SelectWindow.StartSelect("Встать на путь <Color=\"red\">ЗЛА</Color>?", "Да", "Нет");
                
                if (!SelectWindow.IsRight)
                {
                    yield return AwaitEvil();
                }
                else
                {
                    
                }
            }
            
            Player.Instance.enabled = true;
        }
    }

    private IEnumerator AwaitEvil()
    {
        yield return DialogueWindow.StartDialogue(new[]
        {
            "<speed=0.15>(Вы встали на путь <Color=\"red\">ЗЛА</Color>.)"
        }, false);

        _evilSFX.Play();

        var view = GetComponent<SpriteRenderer>();
        
        // Исчезновение
        while (view.color.a > 0)
        {
            view.color = view.color.AddA(-Time.deltaTime);

            yield return null;
        }

        Stats.Instance.Data[6] = 1;
        gameObject.SetActive(false);
        Player.Instance.enabled = true;
    }
}
