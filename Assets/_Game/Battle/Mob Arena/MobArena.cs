using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class MobArena : Enemy
{
    [SerializeField]
    private TextMeshPro _moneyLabel;

    [SerializeField]
    private AudioClip _theme;

    [SerializeField]
    private Animator _view;

    [SerializeField]
    private AttacksPool[] _attacks;

    [SerializeField]
    private AudioSource _addMoneySFX;
    
    private int _money;
    
    private IEnumerator Start()
    {
        Actions = new List<string>()
        {
            "Оценить",
            "Клянчить",
        };

        Answers = new List<string>()
        {
            "Мобы АТК ?? ЗЩ ??.*Собирайте $ и делайте ходы чтобы зарабатывать $.",
            "Вы попросили денег.*Вы получили 1$."
        };
        
        Relationship = -99;
        
        BattleManager.Instance.Enemies.Add(this);
        BattleManager.Instance.IsRun = true;
        MusicManager.Instance.Play(_theme);
        
        var index = 0;

        while (true)
        {
            BattleManager.Instance.Message = "Вас окружили мобы. (Сбегите чтобы закончить бой.)";
            
            yield return BattleManager.Instance.AwaitStartTurn();
            var isRun = false;
            
            if (_view.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                _view.Play("Return"); 
            
            while (!BattleManager.Instance.IsEnemyTurn)
            {
                yield return null;
                
                if (BattleManager.Instance.IsFightSelect || BattleManager.Instance.IsFight)
                {
                    isRun = true;
                    _view.Play("Run");
                }
                else if (isRun)
                {
                    isRun = false;
                    _view.Play("Return"); 
                }
            }

            if (isRun)
            {
                _view.Play("Return"); 
            }
            
            Heart.Instance.gameObject.SetActive(false);

            if (BattleManager.Instance.SelectScreenIndex == 1 && BattleManager.Instance.ActIndex == 1)
            {
                AddMoney(1);
            }
            
            yield return EnemyMessageBox.AwaitWriting(new Vector2(3.18000007f, 1.63f), 
                "Защищайся.");
            
            Heart.Instance.gameObject.SetActive(true);
            Heart.Instance.enabled = true;
            
            if (index >= _attacks.Length)
                index = 0;
            
            yield return _attacks[index].AwaitProcess();
            index++;
        }
    }

    private void Update()
    {
        if (_money > Meta.Instance.RecordMoney)
        {
            Meta.Instance.RecordMoney = _money;
        }
        
        _moneyLabel.text = $"Рекорд {Meta.Instance.RecordMoney}$\nТекущий {_money}$";
    }

    public void AddMoney(int amount)
    {
        _money += amount;
        _addMoneySFX.Play();
    }
}
