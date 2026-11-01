using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Herobrine : Enemy
{
    [SerializeField]
    private AudioClip _theme;
    
    [SerializeField]
    private AttacksPool[] _attacks;

    [SerializeField]
    private GameObject _teleport;
    
    [SerializeField]
    private Animator _view;

    [SerializeField]
    private TextMeshPro _nickname;
    
    private int _index;
    
    private IEnumerator Start()
    {
        Actions = new List<string>()
        {
            "Оценить"
        };

        Answers = new List<string>()
        {
            "Херобрин АТК 7 ЗЩ 7.*Настал час расплаты."
        };
        
        Relationship = -20;
        
        BattleManager.Instance.Enemies.Add(this);
        BattleManager.Instance.IsRun = true;
        MusicManager.Instance.Play(_theme);

        while (true)
        {
            if (Relationship > 0)
            {
                BattleManager.Instance.Message = "Херобрин щадит вас.";
            }
            else
            {
                BattleManager.Instance.Message = "Финал.";
            }
            
            yield return BattleManager.Instance.AwaitStartTurn();
            yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
            Heart.Instance.gameObject.SetActive(false);
            
            if (Health <= 0)
            {
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "<shake>как я мог проиграть нубу?");
                
                _view.Play("Death");
                
                yield return null;
                yield return new WaitUntil(() => _view.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
                _view.gameObject.SetActive(false);
                Stats.Instance.Data[36] = 2;
                yield return BattleManager.Instance.AwaitExit();
            }
            else if (BattleManager.Instance.Turn > 4)
            {
                if (BattleManager.Instance.SelectScreenIndex == 3
                    && BattleManager.Instance.MercyIndex == 0 
                    && Relationship > 0)
                {
                    Stats.Instance.Data[36] = 1;
                    yield return BattleManager.Instance.AwaitExit();
                }
                else
                {
                    Relationship = 10;
                    _nickname.color = Color.yellow;
                
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                        "Я устал сражаться.");
                }
            }
            else
            {
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "Тебе меня не победить!");
                
                yield return Attack();
            }
        }
    }

    private IEnumerator Attack()
    {
        Heart.Instance.gameObject.SetActive(true);
        Heart.Instance.enabled = true;
            
        if (_index >= _attacks.Length)
            _index = 0;
            
        var teleport = Instantiate(_teleport);
        teleport.SetActive(true);
        yield return new WaitForSeconds(1);
        Destroy(teleport);
            
        yield return _attacks[_index].AwaitProcess();

        teleport = Instantiate(_teleport);
        teleport.SetActive(true);
        yield return new WaitForSeconds(1);
        Destroy(teleport);
            
        _view.transform.localPosition = new Vector3(0, 0);
            
        _index++;
    }
    
    public override void Damage()
    {
        _view.GetComponent<Animator>().Play("Damage");
    }
}
