using System.Collections;
using UnityEngine;

public sealed class Cat : MonoBehaviour
{
    [SerializeField]
    private AudioSource _evilSFX;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.Data[6] == 1)
        {
            gameObject.SetActive(false);
            yield break;
        }
        
        var view = GetComponent<SpriteRenderer>();
        
        Player.Instance.enabled = false;
        Player.Instance.SetFlex(true);

        yield return null;
        MusicManager.Instance.Stop();
        Player.Instance.transform.position = new Vector3(0.540000021f, 0, 0);
        yield return null;
        
        // иалог
        yield return DialogueWindow.StartDialogue(new []
        {
            "/c0Мяу.",
            "/n0Котик?",
            "/c0Ты прячешься от своего могущества.",
            "/n0Котик, что ты такое мяукаешь?",
            "/c0Возьми <Color=\"yellow\">Палку Админа</Color> и забань всех игроков.",
            "/n0Нет я не буду этого делать!",
            "/c0Покажи им своë могущество!",
        }, false);
        
        // Выбор
        yield return SelectWindow.StartSelect("Встать на путь <Color=\"red\">ЗЛА</Color>?", "Да", "Нет");

        if (!SelectWindow.IsRight)
        {
            yield return DialogueWindow.StartDialogue(new []
            {
                "<speed=0.15>(Вы встали на путь <Color=\"red\">ЗЛА</Color>.)"
            }, false);
            
            _evilSFX.Play();
        
            // Исчезновение
            while (view.color.a > 0)
            {
                view.color = view.color.AddA(-Time.deltaTime);
                yield return null;
            }
        
            Stats.Instance.Data[6] = 1;
            gameObject.SetActive(false);
        }
        else
        {
            yield return DialogueWindow.StartDialogue(new []
            {
                "(Вы сохранили свою доброту, но потеряли новый контент.)",
                "(Вы всегда можете встать на путь зла.)"
            }, false);
        }
        
        Player.Instance.enabled = true;
    }
}
