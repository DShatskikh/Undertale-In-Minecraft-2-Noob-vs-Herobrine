using System.Collections;
using UnityEngine;

public sealed class GrumView : MonoBehaviour
{
    [SerializeField]
    private AudioSource _sfx, _damageSFX;

    [SerializeField]
    private Enemy _enemy;
    
    private void SFXPlay()
    {
        _sfx.Play();
    }

    private void Damage()
    {
        StartCoroutine(AwaitDamage());
    }

    private IEnumerator AwaitDamage()
    {
        _damageSFX.Play();
        var damageLabel = Instantiate(Resources.Load<DamageIndicator>("Damage Indicator"));
        StartCoroutine(damageLabel.Init(_enemy, 99));

        yield return new WaitForSeconds(1);
        Destroy(damageLabel.gameObject);
    }
}
