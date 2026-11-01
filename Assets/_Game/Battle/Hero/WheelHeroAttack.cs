using System.Collections;
using UnityEngine;

public sealed class WheelHeroAttack : BaseAttack
{
    [SerializeField]
    private WheelHeroShell _wheelHeroShell;

    public override Vector2 SizeField => new(1.75f, 1.75f);

    public override IEnumerator AwaitProcess()
    {
        Heart.Instance.transform.position = new Vector3(0, -3.4f);

        while (BattleManager.Instance.Progress < 100)
        {
            yield return null;
        }
        
        yield return _wheelHeroShell.AwaitHide();
        Destroy(gameObject);
    }
}
