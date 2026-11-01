using UnityEngine;

public sealed class OrangeShell : ShellBase
{
    [SerializeField]
    private int _damage = 5;

    private float _timer = 0.05f;
    
    public override bool CanDamage { get; set; } = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
        {
            return;
        }

        if (other.GetComponent<Heart>().HSpeed != 0 || InputManager.Instance.Vertical > 0)
        {
            _timer = 0.05f; 
            return;
        }
        
        if (_timer <= 0)
        {
            Heart.Instance.Damage(_damage);
            Destroy(gameObject);
        }
        else
        {
            _timer -= Time.deltaTime;
        }
    }
}
