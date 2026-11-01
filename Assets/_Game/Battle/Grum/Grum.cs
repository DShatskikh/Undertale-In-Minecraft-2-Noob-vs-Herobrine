using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grum : Enemy
{
    [SerializeField]
    private AudioClip _theme;
    
    [SerializeField]
    private AttacksPool[] _attacks;

    [SerializeField]
    private Animator _view;

    [SerializeField]
    private TextMeshPro _nickname;

    private bool _isHideHead;
    
    // Визг и звуки ужаса
    // Спасибо что помог принять мне свою красоту.
    
    private IEnumerator Start()
    {
        Actions = new List<string>()
        {
            "Оценить",
            "Убеждать"
        };

        Answers = new List<string>()
        {
            "Грум АТК 5 ЗЩ 3.*Носит тыкву чтобы не пугать своим скином.",
            "Вы убеждайте Грума что его скин не страшный.*Кажется это работает.",
        };
        
        Relationship = -34;
        
        BattleManager.Instance.Enemies.Add(this);
        BattleManager.Instance.IsRun = true;
        MusicManager.Instance.Play(_theme);
        
        var index = 0;
        
        while (true)
        {
            if (Relationship > 0)
            {
                BattleManager.Instance.Message = "Грум щадит вас.";
            }
            else  if (Relationship > -11)
            {
                BattleManager.Instance.Message = "Грум думает над вашими словами.";
            }
            else
            {
                BattleManager.Instance.Message = "По какой-то причине мир перевернулся.";
            }
            
            yield return BattleManager.Instance.AwaitStartTurn();
            yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
            
            if (BattleManager.Instance.SelectScreenIndex == 1 && BattleManager.Instance.ActIndex == 1)
            {
                Relationship += 11;
            }
            
            Heart.Instance.gameObject.SetActive(false);

            if (Health <= 0 || Relationship > 0)
            {
                if (Health <= 0)
                {
                    if (Relationship > 0)
                    {
                        _view.Play("Shake 2");
                        
                        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                            "<shake>как коварно.");
                        
                        _view.Play("Death 2");
                    }
                    else
                    {
                        _view.Play("Shake");
                    
                        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                            "<shake>я сейчас подумал, кому вообще какая разница какой у меня скин?");
                    
                        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                            "<shake>нужно было просто наслаждаться жизнью и не думать что о тебе подумают.");
                    
                        _view.Play("Death");
                    }
                    
                    yield return null;
                    yield return new WaitUntil(() => _view.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
                    _view.gameObject.SetActive(false);
                    Stats.Instance.Data[32] = 2;
                }
                else if (BattleManager.Instance.SelectScreenIndex == 1)
                {
                    if (!_isHideHead)
                    {
                        yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                            "Ты прав, я больше не буду прятать своё лицо.");
                    
                        _view.Play("TakeOff");
                        yield return null;
                        yield return new WaitUntil(() => _view.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

                        _isHideHead = true;
                        Answers[1] = "Вы убеждайте Грума что его скин не страшный.*Кажется в этом небыло необходимости.";
                    }
                    
                    continue;
                }
                else
                {
                    Stats.Instance.Data[8] = 1;
                    Stats.Instance.Data[32] = 1;
                }
                
                yield return BattleManager.Instance.AwaitExit();
            }
            
            yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                "Мой скин ужасен.");
                
            Heart.Instance.gameObject.SetActive(true);
            Heart.Instance.enabled = true;
            
            if (index >= _attacks.Length)
                index = 0;
            
            yield return _attacks[index].AwaitProcess();
            index++;
        }
    }

    public override void Damage()
    {
        if (Relationship > 0)
        {
            _view.Play("Damage 2");
        }
        else
        {
            _view.Play("Damage");
        }
    }

    private void Update()
    {
        if (Relationship >= 0)
            _nickname.color = Color.yellow;
    }
}
