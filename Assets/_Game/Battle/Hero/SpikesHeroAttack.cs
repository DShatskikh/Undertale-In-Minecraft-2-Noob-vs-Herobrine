using System;
using System.Collections;
using UnityEngine;

public sealed class SpikesHeroAttack : BaseAttack
{
    [SerializeField]
    private SpikeHeroShell _prefab;
    
    public override Vector2 SizeField => new Vector2(6.71f, 1.75f);
    public bool IsActive = true;
    
    public override IEnumerator AwaitProcess()
    {
        Heart.Instance.transform.position = new Vector3(0, -3.4f);
        var count = 4;
        
        while (BattleManager.Instance.Progress < 100 && count > 0)
        {
            StartCoroutine(AwaitGroup());
            yield return new WaitForSeconds(2.9f);

            count--;
        }
        
        IsActive = false;

        foreach (var spikeHeroShell in FindObjectsByType<SpikeHeroShell>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            spikeHeroShell.Hide();
        }
        
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private IEnumerator AwaitGroup()
    {
        var startPosition = transform.position.AddX(6.19f);
        
        for (int i = 0; i < 20; i++)
        {
            if (!IsActive)
                yield break;
            
            Instantiate(_prefab, startPosition.AddX(i * -0.6700001f), Quaternion.identity, transform);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
