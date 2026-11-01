using UnityEngine;

public sealed class ArrowShell : ShellBase
{
    private float _startTimer = 0.5f;
    private float _speed = 10;
    
    public override bool CanDamage { get; set; } = true;

    private void Update()
    {
        if (_startTimer > 0)
        {
            _startTimer -= Time.deltaTime;
            return;
        }
        
        transform.Translate(new Vector2(-1f, 0f) * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;
        
        Heart.Instance.Damage(11);
    }
}
