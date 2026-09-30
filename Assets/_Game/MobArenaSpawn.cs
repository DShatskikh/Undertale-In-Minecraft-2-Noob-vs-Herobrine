using System;
using System.Collections;
using UnityEngine;

public sealed class MobArenaSpawn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Player>())
            return;

        StartCoroutine(AwaitSelect());
    }

    private IEnumerator AwaitSelect()
    {
        Player.Instance.enabled = false;

        yield return SelectWindow.StartSelect("Зайти внутрь?\n(Начнётся бой)", "Да", "Нет");

        if (!SelectWindow.IsRight)
        {
            StartBattleScreen.Transition("Hacker");
        }
        else
        {
            Player.Instance.enabled = true;
        }
    }
}
