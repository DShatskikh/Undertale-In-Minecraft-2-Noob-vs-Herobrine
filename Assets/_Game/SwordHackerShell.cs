using UnityEngine;

public sealed class SwordHackerShell : ShellBase
{
    public static bool CanActive = true;
    private const float SPEED = 15;

    [SerializeField]
    private bool _isHealth;
    
    public override bool CanDamage
    {
        get => _canDamage;
        set => _canDamage = value;
    }

    [SerializeField]
    private Animator _animator;

    private bool _canDamage = true;
    private bool _isRight = true;
    private float _startTimer = 0.5f;

    public void Init(bool isRight)
    {
        _isRight =  isRight;
        transform.localScale = new Vector3(_isRight ? 1 : -1, 1, 1);
    }

    private void Update()
    {
        // if (!CanActive)
        // {
        //     _canDamage = false;
        //     _animator.Play("Sworld Hide");
        //     return;
        // }
        
        if (_startTimer > 0)
        {
            _startTimer -= Time.deltaTime;
            return;
        }

        _animator.Play("Sworld Fly");
        var speed = _isRight ? SPEED : -SPEED;
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        // transform.position = transform.position.AddX(speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;

        if (_isHealth)
        {
            BattleManager.Instance.Health(2);
        }
        else
        {
            Heart.Instance.Damage(4);
        }
        
        Destroy(gameObject);
    }
}
