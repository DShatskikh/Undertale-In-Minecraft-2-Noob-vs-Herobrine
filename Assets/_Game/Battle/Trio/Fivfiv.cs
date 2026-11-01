using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class Fivfiv : Enemy
{
    [SerializeField]
    private AudioClip _theme;

    [SerializeField]
    private GameObject _view;

    [SerializeField]
    private TextMeshPro _nickname;

    private Nastya _nastya;
    private Sheep _sheep;
    
    public bool IsDamage;
    public AttacksPool[] Attacks;

    private IEnumerator Start()
    {
        Actions = new List<string>()
        {
            "Оценить",
            "Дружить"
        };

        Answers = new List<string>()
        {
            "Фивфив АТК 5 ЗЩ 5.*Прислушивается к мнению своих подруг.",
            "У вас ничего не получилось.*Подружитесь с Настей и Овечкой чтобы пощадить Фывфыва.",
        };
        
        Relationship = -99;
        
        _nastya = FindAnyObjectByType<Nastya>();
        _sheep = FindAnyObjectByType<Sheep>();
        
        BattleManager.Instance.Enemies.Add(this);
        BattleManager.Instance.Enemies.Add(_nastya);
        BattleManager.Instance.Enemies.Add(_sheep);
        BattleManager.Instance.IsRun = true;
        MusicManager.Instance.Play(_theme);
        
        var index = 0;
        
        while (true)
        {
            if (_nastya.Health < 0 || _sheep.Health < 0)
            {
                BattleManager.Instance.Message = "Теперь только битва.";
                BattleManager.Instance.IsRun = false;
            }
            else if (Relationship > 0)
            {
                if (BattleManager.Instance.Enemies[1].IsActive && BattleManager.Instance.Enemies[2].IsActive)
                {
                    BattleManager.Instance.Message = "Троица щадит вас.";
                }
                else
                {
                    BattleManager.Instance.Message = "Фывфыв вас щадит.";
                }
            }
            else if (_nastya.Relationship > 0 && _sheep.Relationship > 0)
            {
                BattleManager.Instance.Message = "Вы почти победили.";
            }
            else if (_sheep.Relationship > 0 || _nastya.Relationship > 0)
            {
                BattleManager.Instance.Message = "Продолжайте.";
            }
            else
            {
                BattleManager.Instance.Message = "Трое на одного.\nЭто вообще законно?";
            }

            if (_nastya.Health < 0 || _sheep.Health < 0)
            {
                Answers[1] = "У вас ничего не получилось.";
                _nastya.Answers[1] = "У вас ничего не получилось.";
                _sheep.Answers[1] = "У вас ничего не получилось.";
                
                _sheep.Relationship = -99;
                _nastya.Relationship = -99;
            }
            else if (_nastya.Relationship > 0 && _sheep.Relationship > 0)
            {
                Answers[1] = "Фывфыв теперь ваш друг.";
            }
            
            yield return BattleManager.Instance.AwaitStartTurn();
            yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
            
            Heart.Instance.gameObject.SetActive(false);

            if (_nastya.IsDamage)
            {
                if (_nastya.Health > 0)
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(0.64f, 2.71f), 
                        "Эй!");
                }
                else
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(0.64f, 2.71f), 
                        "Я проиграла.");
                    
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                        "НЕЕЕЕТ!");
                    
                    _sheep.Relationship = -99;
                    
                    _nastya.IsActive = false;
                    _nastya.gameObject.SetActive(false);
                }

                _nastya.IsDamage = false;
            }
            else if (_sheep.IsDamage)
            {
                if (_sheep.Health > 0)
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(-0.6723136f, 2.71f), 
                        "Ай!", true);
                }
                else
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(-0.6723136f, 2.71f), 
                        "F", true);
                    
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                        "ОВЕЕЕЧКА!");

                    _nastya.Relationship = -99;
                    
                    _sheep.IsActive = false;
                    _sheep.gameObject.SetActive(false);
                }

                _sheep.IsDamage = false;
            }
            else if (IsDamage && Health <= 0)
            {
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "ГГ");

                if (_nastya.IsActive)
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(0.64f, 2.71f), 
                        "Куда же ты?");

                    _nastya.enabled = true;
                }
                else if (_sheep.IsActive)
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(-0.6723136f, 2.71f), 
                        "Остались только мы.", true);

                    _sheep.enabled = true;
                }
                else
                {
                    _view.SetActive(false);
                    yield return BattleManager.Instance.AwaitExit();
                }
                
                IsActive = false;
                gameObject.SetActive(false);

                IsDamage = false;
            }
            else if (BattleManager.Instance.SelectScreenIndex == 3
                && BattleManager.Instance.MercyIndex == 0)
            {
                if (Relationship > 0)
                {
                    Stats.Instance.Data[8] = 1;
                    
                    Stats.Instance.Data[33] = 1;
                    Stats.Instance.Data[34] = 1;
                    Stats.Instance.Data[35] = 1;
                    yield return BattleManager.Instance.AwaitExit();
                }
                else
                {
                    if (_nastya.Relationship > 0)
                    {
                        _nastya.IsActive = false;
                        _nastya.gameObject.SetActive(false);
                    }

                    if (_sheep.Relationship > 0)
                    {
                        _sheep.IsActive = false;
                        _sheep.gameObject.SetActive(false);
                    }
                }
            }
            else if (_nastya.Relationship < 1
                && BattleManager.Instance.SelectScreenIndex == 1 
                && BattleManager.Instance.ActIndex == 1 
                && BattleManager.Instance.ActSelectedIndex == 1)
            {
                if (_sheep.Health <= 0)
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(0.64f, 2.71f), 
                        "...");
                }
                else if (_sheep.Relationship > 0)
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(0.64f, 2.71f), 
                        "Постойте я не могу сражаться с Нубиком.");
                
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                        "Так не честно.");
                    
                    _nastya.Relationship = 20;
                }
                else
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(0.64f, 2.71f), 
                        "Постойте я не могу сражаться с Нубиком.");
                
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                        "Почему?");
                
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(0.64f, 2.71f), 
                        "Потому что мы теперь друзья.");
                
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(-0.6723136f, 2.71f), 
                        "Эм...", true);
                
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(0.64f, 2.71f), 
                        "А что мне делать?");
                
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(0.64f, 2.71f), 
                        "Хилить?");
                    
                    _nastya.Relationship = 20;
                }
                
                _nastya.Answers[1] = "В этом нет необходимости.";
            }
            else if (_sheep.Relationship < 1
                && BattleManager.Instance.SelectScreenIndex == 1 
                && BattleManager.Instance.ActIndex == 1 
                && BattleManager.Instance.ActSelectedIndex == 2)
            {
                if (_nastya.Health <= 0)
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(-0.6723136f, 2.71f), 
                        "...", true);
                }
                else if (_nastya.Relationship > 0)
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(-0.6723136f, 2.71f), 
                        "Прости я не могу сражаться я дружу с Нубиком.", true);
                
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                        "Ничего я и один справлюсь.");
                    
                    _sheep.Relationship = 20;
                }
                else
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(-0.6723136f, 2.71f), 
                        "Простите я не могу сражаться я дружу с Нубиком.", true);
                
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(0.64f, 2.71f), 
                        "Ничего мы справимся.");
                    
                    _sheep.Relationship = 20;
                }
                
                _sheep.Answers[1] = "В этом нет необходимости.";
            }
            else if (Relationship < 1
                     && BattleManager.Instance.SelectScreenIndex == 1 
                     && BattleManager.Instance.ActIndex == 1 
                     && BattleManager.Instance.ActSelectedIndex == 0
                     && _sheep.Relationship > 0
                     && _nastya.Relationship > 0)
            {
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "Ладно, я сдаюсь.");
                
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "Ты победил.");
                
                Relationship = 20;
                Answers[1] = "В этом нет необходимости.";
            }
            else
            {
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "Защищайся.");
            }

            _nickname.color = Relationship > 0 ? Color.yellow : Color.white;
            _sheep.Nickname.color = _sheep.Relationship > 0 ? Color.yellow : Color.white;
            _nastya.Nickname.color = _nastya.Relationship > 0 ? Color.yellow : Color.white;
            
            Heart.Instance.gameObject.SetActive(true);
            Heart.Instance.enabled = true;
            
            if (index >= Attacks.Length)
                index = 0;
            
            yield return Attacks[index].AwaitProcess();
            index++;
        }
        
        // Дружить
        // Но сначала нужно подружиться со спутницами
    }
    
    public override void Damage()
    {
        IsDamage = true;
        _view.GetComponent<Animator>().Play("Damage");
        
        if (Health <= 0)
        {
            Stats.Instance.Data[33] = 2;
        }
    }
    
    public override IEnumerator AwaitRun()
    {
        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
            "Постой.");
    }
}
