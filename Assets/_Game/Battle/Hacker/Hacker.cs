using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Hacker : Enemy
{
    [SerializeField]
    private AudioClip _theme;

    [SerializeField]
    private GameObject _view;

    [SerializeField]
    private GameObject _admin;

    [SerializeField]
    private AttacksPool[] _attacks;
    
    // Троллит игрока если тот опустит его здоровье до 0 и будет еще 3 хода где он будет ржать и странная мини атака
    
    private IEnumerator Start()
    {
        Actions = new List<string>()
        {
            "Оценить",
            "Говорить"
        };

        Answers = new List<string>()
        {
            "Хакер АТК ??? ЗЩ ???.*Величайший хакер в истории! (по его словам)",
            "Вы говорите с Хакером.*Кажется он вас не слушает.",
        };
        
        Relationship = -20;
        
        BattleManager.Instance.Enemies.Add(this);
        BattleManager.Instance.IsRun = true;
        MusicManager.Instance.Play(_theme);
        // BattleManager.Instance.WriteMessage("Хакер преграждает вам путь");
        
        for (int i = 0; i <= 3; i++)
        {
            if (i == 0)
            {
                BattleManager.Instance.Message = "Вы ощущаете силу читов.";
                yield return BattleManager.Instance.AwaitStartTurn();
                yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
                Heart.Instance.gameObject.SetActive(false);
            
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "Я великий!\nЯ могучий!\nЯ величайший хакер в истории!");
                
                Heart.Instance.gameObject.SetActive(true);
                Heart.Instance.enabled = true;
                yield return _attacks[0].AwaitProcess(); 
            }
            else if (i == 1)
            {
                BattleManager.Instance.Message = "Вы ощутили силу читов.";
                yield return BattleManager.Instance.AwaitStartTurn();
                yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
                Heart.Instance.gameObject.SetActive(false);
            
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "Они смеялись что я не умею ПВПхаться, но кто смеётся теперь?!");
                
                Heart.Instance.gameObject.SetActive(true);
                Heart.Instance.enabled = true;
                yield return _attacks[1].AwaitProcess(); 
            }
            else if (i == 2)
            {
                BattleManager.Instance.Message = "Это нечестный бой.";
                yield return BattleManager.Instance.AwaitStartTurn();
                yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
                Heart.Instance.gameObject.SetActive(false);
            
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "Я одолел всех кто смеялся надо мной. Кажется я повторяюсь.");
                
                Heart.Instance.gameObject.SetActive(true);
                Heart.Instance.enabled = true;
                yield return _attacks[2].AwaitProcess(); 
            }
            else
            {
                BattleManager.Instance.Message = "Читы зло.";
                yield return BattleManager.Instance.AwaitStartTurn();
                yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
                Heart.Instance.gameObject.SetActive(false);
                
                yield return AwaitCutscene();
            }

            while (_view.transform.position != new Vector3(0, 0.469999999f, 0))
            {
                _view.transform.position = Vector3.MoveTowards(_view.transform.position, new Vector3(0, 0.469999999f, 0), Time.deltaTime * 6);
                yield return null;
            }
        }
    }

    private IEnumerator AwaitCutscene()
    {
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "А теперь моя финальная атака!");

        yield return new WaitForSeconds(1);
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Подожди, щас выдам себе админку.");

        _admin.transform.localPosition = _admin.transform.localPosition.SetY(1.92f);
        _admin.SetActive(true);

        while (_admin.transform.localPosition.y > 0)
        {
            _admin.transform.localPosition = _admin.transform.localPosition.AddY(-Time.deltaTime * 1);
            yield return null;
        }
        
        while (_view.transform.localPosition.x > -0.66f)
        {
            _view.transform.localPosition = _view.transform.localPosition.AddX(-Time.deltaTime * 1);
            yield return null;
        }
        
        yield return new WaitForSeconds(1);
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(2.18f, 2.71f), 
            "Что ты себе выдашь?", true);
        
        yield return new WaitForSeconds(1);
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(1.71f, 2.71f), 
            "<shake>ничего");
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(2.18f, 2.71f), 
            "На сервере запрещены читы.", true);
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(1.71f, 2.71f), 
            "<shake>Не надо дядя");
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(2.18f, 2.71f), 
            "Ты будешь наказан пока не научишься себя вести.", true);
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(1.71f, 2.71f), 
            "<shake>Не надо дядя");
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(2.18f, 2.71f), 
            "<shake>НЕЕЕЕЕТ!");
        
        yield return new WaitForSeconds(1);
        _view.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        
        yield return EnemyMessageBox.AwaitWriting(new Vector2(1.71f, 2.71f), 
            "Не обращай внимание, считай что ты победил.", true);
        
        while (_admin.transform.localPosition.y < 1.92f)
        {
            _admin.transform.localPosition = _admin.transform.localPosition.AddY(Time.deltaTime * 3);
            yield return null;
        }
        
        _admin.SetActive(false);
        
        Stats.Instance.Data[31] = 1;
        yield return BattleManager.Instance.AwaitExit();
    }

    public override void Damage()
    {
        _view.GetComponent<Animator>().Play("Damage");
    }
}
