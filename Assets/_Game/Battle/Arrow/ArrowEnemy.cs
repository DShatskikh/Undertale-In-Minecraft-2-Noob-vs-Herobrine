using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class ArrowEnemy : Enemy
{
    [SerializeField]
    private GameObject[] _attacks;
    
    private IEnumerator Start()
    {
        BattleManager.Instance.Enemies.Add(this);
        BattleManager.Instance.IsEnemyTurn = true;
        BattleManager.Instance.HideButtons(true);
        BattleManager.Instance.MainText.gameObject.SetActive(false);
        // Heart.Instance.gameObject.SetActive(true);

        yield return null;
        
        BattleManager.Instance.SetFrame(new(2f, 1.75f));
        Heart.Instance.transform.position = new(0, -3.39f);
        Heart.Instance.enabled = true;
        yield return new WaitForSeconds(0.5f);
        
        var index = Random.Range(0, _attacks.Length);

        if (index == 0)
        {
            _attacks[0].SetActive(true);
            yield return new WaitForSeconds(5);  
        }
        else
        {
            _attacks[1].SetActive(true);
            yield return new WaitForSeconds(6);  
        }
        
        if (Stats.Instance.LevelName == "Dungeos Right Up 1")
        {
            if (Arrow.IsOtherSide)
            {
                Stats.Instance.Position = new Vector3(37.45f, 0.172655374f, 0);
            }
            else
            {
                Stats.Instance.Position = new Vector3(-0.270000011f, 0.230000004f, 0);
            }
        }
        else if (Stats.Instance.LevelName == "Dungeos Right 1")
        {
            Stats.Instance.Position = new Vector3(2.18000007f,0.0399999991f,0);
        }
        else if (Stats.Instance.LevelName == "Dungeos Right 2")
        {
            if (Arrow.IsOtherSide)
            {
                Stats.Instance.Position = new Vector3(-0.140000001f, 20.6499996f, 0);
            }
            else
            {
                Stats.Instance.Position = new Vector3(0f,0.0399999991f,0);
            }
        }
        else
        {
            Stats.Instance.Position = new Vector3(-0.270000011f, 0.230000004f, 0);
        }
        
        yield return BattleManager.Instance.AwaitExit();
    }
}
