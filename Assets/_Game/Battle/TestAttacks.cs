using System;
using System.Collections;
using UnityEngine;

public sealed class TestAttacks : Enemy
{
    [SerializeField]
    private ShellSpawner _attack;

    private IEnumerator Start()
    {
        BattleManager.Instance.Enemies.Add(this);
        BattleManager.Instance.IsRun = false;
        
        BattleManager.Instance.IsEnemyTurn = true;
        BattleManager.Instance.HideButtons(true);
        BattleManager.Instance.MainText.gameObject.SetActive(false);
        Heart.Instance.gameObject.SetActive(true);
        
        yield return null;

        Heart.Instance.enabled = true;
        var attack = Instantiate(_attack);
        attack.gameObject.SetActive(true);
        attack.transform.localScale = Vector3.one;
        
        yield break;
    }
}
