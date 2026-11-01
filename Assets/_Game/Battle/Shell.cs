using System;
using UnityEngine;

public sealed class Shell : ShellBase
{
    [SerializeField]
    private int _damage = 5;

    [SerializeField]
    private bool _isDestroy = true;
    
    public override bool CanDamage { get; set; } = true;
    public override int Damage => _damage;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
        {
            return;
        }
        
        if (!enabled)
        {
            return;
        }

        if (_damage > 0)
        {
            Heart.Instance.Damage(_damage);
            
            if (_isDestroy)
                Destroy(gameObject);
        }

        if (_damage < 0)
        {
            BattleManager.Instance.Health(-_damage);
            
            if (_isDestroy)
                Destroy(gameObject);
        }
    }
}
