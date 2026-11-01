using UnityEngine;

public sealed class ShellTrigger : ShellBase
{
    public override bool CanDamage { get; set; }
    public int Damage = 5;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;
        
        Heart.Instance.Damage(Damage);
        Destroy(gameObject);
    }
}
