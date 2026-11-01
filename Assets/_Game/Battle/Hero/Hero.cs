using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class Hero : Enemy
{
    [SerializeField]
    private AudioClip _theme;

    [SerializeField]
    private GameObject _view;

    [SerializeField]
    private AttacksPool[] _attacks;

    [SerializeField]
    private GameObject _dog, _tear;
    
    [SerializeField]
    private TextMeshPro _nickname;
    
    private IEnumerator Start()
    {
        Actions = new List<string>()
        {
            "Оценить",
            "Говорить"
        };

        Answers = new List<string>()
        {
            "Герой АТК ??? ЗЩ ???.*На защите игроков от багов.",
            "Вы говорите с Героем.*(Кажется это работает.)"
        };
        
        Health = 20;
        MaxHealth = 20;
        Relationship = -20;
        
        BattleManager.Instance.Enemies.Add(this);
        BattleManager.Instance.IsRun = true;
        MusicManager.Instance.Stop();
        _view.SetActive(false);
        
        var speakIndex = 0;
        
        // Первая атака героя
        // Черт я надеялся победить первой атакой
        
        BattleManager.Instance.IsEnemyTurn = true;
        BattleManager.Instance.HideButtons(true);
        BattleManager.Instance.MainText.gameObject.SetActive(false);
        Heart.Instance.gameObject.SetActive(true);

        yield return null;
        Heart.Instance.enabled = true;

        yield return _attacks[0].AwaitProcess();
        
        _view.SetActive(true);
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), "Черт, я надеялся победить первой атакой.");
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), "Придётся выложится на полную.");
        
        MusicManager.Instance.Play(_theme);
        var index = 1;
        
        while (true)
        {
            if (BattleManager.Instance.Turn == 0)
            {
                BattleManager.Instance.Message = "Кажется вас ждёт тяжелый бой.";
            }
            else
            {
                BattleManager.Instance.Message = "Продолжайте.";
            }
            
            yield return BattleManager.Instance.AwaitStartTurn();
            yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
            Heart.Instance.gameObject.SetActive(false);

            if (Health <= 1)
            {
                Health = 1;
                yield return AwaitExit();
            }
            else
            {
                yield return BattleManager.Instance.AwaitSetFrame(_attacks[0].GetSize);
            
                // Говорит если мы жмём говорить

                if (BattleManager.Instance.SelectScreenIndex == 1 && BattleManager.Instance.ActIndex == 1)
                {
                    // yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    //     speakIndex switch
                    //     {
                    //         // Ладно я расскажу свою историю
                    //         0 => "Когда-то давно я впервые зашел на сервер и стал другом Админа.",
                    //         1 => "Я помогал ему с тестированием сервера и помогаю по сей день.",
                    //         2 => "Я сражался со всеми мобами на это сервере...",
                    //         3 => "Я тот кто защищает игроков от багов.",
                    //         4 => "Но в последнее время я задумываюсь...",
                    //         5 => "Мне 23 года и стоит ли мне тратить время на игру про кубики?",
                    //         6 => "Спасибо что послушал мой монолог.",
                    //     });

                    if (speakIndex == 0)
                    {
                        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                            "Когда я впервые зашел на сервер, то сразу в него влюбился...");
                    }
                    else if (speakIndex == 1)
                    {
                        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                            "Я добился всего что только можно, я победил всех мобов и игроков...");
                    }
                    else if (speakIndex == 2)
                    {
                        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                            "Я подружился с Админом и помогал ему в тестировании новых плагинов...");
                        
                        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                            "И стоял на страже сервера от багов...");
                    }
                    else if (speakIndex == 3)
                    {
                        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                            "Но в последнее время мне стало скучно...");
                    }
                    else
                    {
                        yield return AwaitExit();
                        // Спасибо что послушал мой монолог.
                        // Я решил уйти с сервера.
                        // Прощай Персик.
                        // (Грустный гав.)
                        // Рыцарь исчезает, на его место ложится собачка и бой заканчивается.
                    }
                    
                    speakIndex++;
                }
                else if (BattleManager.Instance.SelectScreenIndex == 0)
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                        "Защищайся!");
                }
                else if (BattleManager.Instance.SelectScreenIndex == 3)
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                        "Ты не сможешь меня пощадить пока я не захочу.");
                }
                else
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                        "...");
                }
                
                //Heart.Instance.gameObject.SetActive(true);
                //Heart.Instance.enabled = true;

                if (index > _attacks.Length - 1)
                {
                    index = 1;
                }
                
                // yield return BattleManager.Instance.AwaitSetFrame(_attacks[index].GetSize);
                yield return _attacks[index].AwaitProcess();
                index++;
            }
        }
    }

    private IEnumerator AwaitExit()
    {
        MusicManager.Instance.Stop();
        
        _view.GetComponent<Animator>().Play("New State");
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Знаешь я просто устал от игры про кубики.");
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Может я просто её перерос?");
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Прощай Персик.");
        
        _dog.SetActive(true);
        _view.GetComponent<SpriteRenderer>().enabled = false;
        _nickname.gameObject.SetActive(false);

        while (_dog.transform.localPosition.x < 0)
        {
            _dog.transform.localPosition = _dog.transform.localPosition.AddX(Time.deltaTime * 1);
            yield return null;
        }

        yield return new WaitForSeconds(1);
        
        _tear.SetActive(true);
        
        yield return new WaitForSeconds(2);
        
        Stats.Instance.Data[37] = 2;

        yield return BattleManager.Instance.AwaitExit();
    }
    
    public override void Damage()
    {
        _view.GetComponent<Animator>().Play("Damage");
    }
}
