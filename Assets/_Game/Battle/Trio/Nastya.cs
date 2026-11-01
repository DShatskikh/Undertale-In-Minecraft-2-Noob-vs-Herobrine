using System.Collections;
using TMPro;
using UnityEngine;

public sealed class Nastya : Enemy
{
    [SerializeField]
    private GameObject _view;
    
    private Fivfiv _fivfiv;
    private Sheep _sheep;
    
    public bool IsDamage;
    public TextMeshPro Nickname;
    
    private IEnumerator Start()
    {
        _sheep = (Sheep)BattleManager.Instance.Enemies[1];
        _fivfiv = (Fivfiv)BattleManager.Instance.Enemies[0];
        
        var index = 0;
        
        while (true)
        {
            yield return BattleManager.Instance.AwaitStartTurn();
            yield return new WaitUntil(() => BattleManager.Instance.IsEnemyTurn);
            Heart.Instance.gameObject.SetActive(false);
            
            if (_sheep.IsDamage)
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
                    
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(0.64f, 2.71f), 
                        "Пока Овечка.");
                    
                    _sheep.IsActive = false;
                    _sheep.gameObject.SetActive(false);
                }

                _sheep.IsDamage = false;
            }
            else if (IsDamage && Health <= 0)
            {
                yield return EnemyMessageBox.AwaitWriting(new Vector2(3.82f, 2.71f), 
                    "Я проиграла.");

                if (_sheep.IsActive)
                {
                    yield return EnemyMessageBox.AwaitWriting(new Vector2(-0.6723136f, 2.71f), 
                        "F", true);

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
            else
            {
                yield return EnemyMessageBox.AwaitWriting(new Vector2(0.64f, 2.71f), 
                    "Остались только мы."); 
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
            Stats.Instance.Data[34] = 2;
        }
    }
}
