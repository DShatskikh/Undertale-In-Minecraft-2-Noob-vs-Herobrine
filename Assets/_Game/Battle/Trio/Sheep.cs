using System.Collections;
using TMPro;
using UnityEngine;

public sealed class Sheep : Enemy
{
    [SerializeField]
    private GameObject _view;
    
    private Fivfiv _fivfiv;
    private Nastya _nastya;
    
    public bool IsDamage;
    public TextMeshPro Nickname;
    
    private IEnumerator Start()
    {
        _nastya = (Nastya)BattleManager.Instance.Enemies[1];
        _fivfiv = (Fivfiv)BattleManager.Instance.Enemies[0];
        
        var index = 0;
        
        while (true)
        {
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
                    
                    _nastya.IsActive = false;
                    _nastya.gameObject.SetActive(false);
                }

                _nastya.IsDamage = false;
            }
            else if (IsDamage && Health <= 0)
            {
                yield return EnemyMessageBox.AwaitWriting(new Vector2(-0.6723136f, 2.71f), 
                    "F", true);

                if (_nastya.IsActive)
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                        "Не волнуйся я справлюсь.");

                    _nastya.enabled = true;
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
            else
            {
                yield return EnemyMessageBox.AwaitWriting(new Vector2(-0.6723136f, 2.71f), 
                    "Остались только мы.", true);
            }
            
            Heart.Instance.gameObject.SetActive(true);
            Heart.Instance.enabled = true;
            
            if (index >= _fivfiv.Attacks.Length)
                index = 0;
            
            yield return _fivfiv.Attacks[index].AwaitProcess();
            index++;
        }
    }
    
    public override void Damage()
    {
        IsDamage = true;
        _view.GetComponent<Animator>().Play("Damage");
        
        if (Health <= 0)
        {
            Stats.Instance.Data[35] = 2;
        }
    }
}
