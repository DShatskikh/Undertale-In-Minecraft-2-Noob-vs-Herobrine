using UnityEngine;

public class BlueShell : ShellBase
{
    [SerializeField]
    private int _damage = 5;

    public override bool CanDamage { get; set; } = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>() || (other.GetComponent<Heart>().HSpeed == 0 && InputManager.Instance.Vertical <= 0))
        {
            return;
        }
        
        Heart.Instance.Damage(_damage);
        Destroy(gameObject);
    }
}
