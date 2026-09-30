using System.Collections;
using UnityEngine;

public sealed class HackerSpawn : Usable
{
    public override IEnumerator AwaitUse()
    {
        StartBattleScreen.Transition("Hacker");
        yield break;
    }
}
