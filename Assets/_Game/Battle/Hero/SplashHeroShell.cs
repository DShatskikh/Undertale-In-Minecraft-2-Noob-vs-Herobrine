using System;
using System.Collections;
using UnityEngine;

public sealed class SplashHeroShell : ShellBase
{
    public override bool CanDamage { get; set; }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>() || !CanDamage)
            return;

        Heart.Instance.Damage(5);
    }

    private void OnSFXAnimationEvent()
    {
        GetComponent<AudioSource>().Play();
    }
    
    private void OnDamageOnAnimationEvent()
    {
        CanDamage = true;
    }
    
    private void OnDamageOFFAnimationEvent()
    {
        CanDamage = false;
    }
}
