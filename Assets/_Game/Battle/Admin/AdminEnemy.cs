using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdminEnemy : Enemy
{
    [SerializeField]
    private AudioClip _theme;

    [SerializeField]
    private GameObject[] _attacks;

    [SerializeField]
    private Animator _view;

    [SerializeField]
    private TextMeshPro _nickname;

    public bool IsJumpedPlatform;
    
    private IEnumerator Start()
    {
        Actions = new List<string>()
        {
            "Оценить",
            "Говорить"
        };

        Answers = new List<string>()
        {
            "Админ АТК ?? ЗЩ ??.*Ждёт вашего хода.",
            "Вы просите Админа пощадить вас.*(Это работает.)",
        };

        Relationship = -20;

        BattleManager.Instance.Enemies.Add(this);
        BattleManager.Instance.IsRun = true;
        MusicManager.Instance.Play(_theme);
        
        BattleManager.Instance.IsEnemyTurn = true;
        BattleManager.Instance.MainText.gameObject.SetActive(false);
        BattleManager.Instance.HideButtons();
        BattleManager.Instance.SetFrame(new Vector2(4, 1.75f));
        Heart.Instance.enabled = true;
        Heart.Instance.transform.position = new Vector3(0, -3.393f);

        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Привет, я Админ.");
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "А это твоя душа.");
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Для начала попробуй взять то яблоко.");
        
        _attacks[3].SetActive(true);
        yield return  new WaitUntil(() => IsJumpedPlatform);
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Не плохо.");
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Чтобы завершить ход противника нужно заполнить \"Прогресс хода\" на 100%.");
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Это просто.");
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Стой близко к атаке или жди пока шкала не заполнится сама.");
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Попробуй.");
        
        _attacks[3].SetActive(false);
        
        // Медленно летит топор
        _attacks[0].SetActive(true);
        yield return new WaitForSeconds(15);

        yield return BattleManager.Instance.AwaitSetFrame(new Vector2(2, 1));
        
        // стой средне гигантский топор
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "А теперь замри.");
        _attacks[1].SetActive(true);
        yield return new WaitForSeconds(3);
        
        // двигайся
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Двигайся.");
        _attacks[2].SetActive(true);
        yield return new WaitForSeconds(3);
        
        BattleManager.Instance.Progress = 100;

        if (Stats.Instance.HP > 17)
        {
            yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                "У тебя не плохо получается.");
        }
        else
        {
            yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                "Над этим тебе еще нужно поработать.");
        }
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Теперь твой ход.");
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Помни разговор может решить любой конфликт.");
        
        BattleManager.Instance.Message = "Выберите <color=yellow>ДЕЙСТВИЯ</color> чтобы расположить Админа.";
        
        while (true)
        {
            // Не плохо
            // Админ щадит вас
            // Когда ник противника становится желтым нажми пощада чтобы закончить бой
            // Молодец, я горжусь тобой
            
            yield return BattleManager.Instance.AwaitStartTurn();
            yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
            Heart.Instance.gameObject.SetActive(false);

            if (BattleManager.Instance.SelectScreenIndex == 0)
            {
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "Я в креативе.");
            }
            else if (BattleManager.Instance.SelectScreenIndex == 3
                && BattleManager.Instance.MercyIndex == 0
                && Relationship > 0)
            {
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "Ты готов.");
                
                Stats.Instance.Data[70] = 2;
                yield return BattleManager.Instance.AwaitExit();
            }
            else if (BattleManager.Instance.SelectScreenIndex == 1
                     && Relationship < 0)
            {
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "Ты всё правильно понял.");

                Relationship = 10;
                Answers[1] = "В этом не было нужды";
                _nickname.color = Color.yellow;
                BattleManager.Instance.Message = "Выберите <color=yellow>ПОЩАДА</color> чтобы закончить бой.";
            }
            else
            {
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "...");
            }
        }
    }

    public override IEnumerator AwaitRun()
    {
        Stats.Instance.Data[70] = 2;
        
        return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Куда же ты?.");
    }
}
