using System;
using System.Collections;
using UnityEngine;

public sealed class TestAttacks : Enemy
{
    [SerializeField]
    private AttacksPool[] _attacks;

    private IEnumerator Start()
    {
        BattleManager.Instance.Enemies.Add(this);
        BattleManager.Instance.IsRun = false;
        
        BattleManager.Instance.IsEnemyTurn = true;
        BattleManager.Instance.HideButtons(true);
        BattleManager.Instance.MainText.gameObject.SetActive(false);
        Heart.Instance.gameObject.SetActive(true);
        Heart.Instance.enabled = true;

        yield return _attacks[0].AwaitProcess();
        // _spawners[0].Timer
        
        yield break;
    }
}
