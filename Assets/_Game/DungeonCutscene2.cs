using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class DungeonCutscene2 : MonoBehaviour
{
    [SerializeField]
    private Transform _admin;

    [SerializeField]
    private SpriteRenderer _bisnesman;

    [SerializeField]
    private AudioSource _screamSFX;
    
    private IEnumerator Start()
    {
        if (Stats.Instance.Data[44] != 1 || Stats.Instance.Data[7] == 1)
        {
            gameObject.SetActive(false);
            yield break;
        }
        
        Player.Instance.enabled = false;

        // Поворачиваем игрока
        Player.Instance.SetFlex(false);

        yield return null;
        yield return new WaitForSeconds(0.5f);
        
        // Бу.
        // А!
        yield return DialogueWindow.StartDialogue(new []
        {
            "/b0Бу.",
        }, true);
        
        _screamSFX.Play();
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/n0А!",
        }, true);
        
        yield return new WaitForSeconds(0.5f);
        
        Player.Instance.SetFlex(true);
        
        // Ты чего так пугаешь?
        // Прости.
        // Получилось забрать артефакт?
        // Нет, Херобрин опередил меня.
        // Да как он посмел!
        // Ладно, держи утешительный приз.
        // (Вы получили палку админа.)
        // Спасибо.
        // Эй, а зачем тебе та штука?
        // Чтобы захватить Майнкрафт и стать самым главным.
        // Пон.
        // ...
        // Ну я пошел
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/n0Ты чего так пугаешь?",
            "/b0Прости.",
            "/b0Получилось забрать ту штуку?",
            "/n0Нет, Херобрин опередил меня.",
            "/b0Да как он посмел!",
            "/b0Ладно, держи утешительный приз.",
            "(Вы получили <color=red>Палку Админа</color>.)",
            "/n0Спасибо.",
            "/n0Эй, а зачем тебе та штука?",
            "/b0Чтобы захватить Майнкрафт и стать самым главным.",
            "/b0Очевидно.",
            "/n0Ну да<waitfor=0.3>.<waitfor=0.3>.<waitfor=0.3>.",
        }, true);
        
        yield return new WaitForSeconds(1.5f);
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/b0.<waitfor=1.0>.<waitfor=1.0>.",
            "/b0Ну я пошел.",
        }, true);
        
        _bisnesman.flipX = true;
        
        // Прилетает Админ

        while (_admin.position.y > 11.537f)
        {
            _admin.position = _admin.position.AddY(-Time.deltaTime * 3);
            yield return null;
        }
        
        // Что вы тут делаете?
        // ничего (тресётся)
        // Уходит (или приходит Админ и забирает магазин и деньги у бизнесмена и банит Нубика.)
        
        yield return DialogueWindow.StartDialogue(new []
        {
            "/a0Что вы тут делаете?",
            "/b0<shake>ничего.",
            "/a0Тут нельзя находиться.",
            "/a0Назови хоть одну причину тебя не банить?",
            "/b0Это всё Нубик, у него <color=red>Палка Админа</color>.",
            "/n0Эй!",
            "/a0Хорошо, я не буду тебя банить.",
            "/a0Всего лишь заберу все твои деньги и магазин.",
            "/b0Отстой.",
            "/a0А теперь ты.",
        }, true);
        
        yield return new WaitForSeconds(1f);

        Meta.Instance.IsEndOldSpawn = true;
        Stats.Instance.Reset();
        
        SceneManager.LoadScene("Ban");
        
        Player.Instance.enabled = true;
    }
}
