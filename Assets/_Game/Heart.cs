using UnityEngine;

public sealed class Heart : MonoBehaviour
{
    public static Heart Instance;
    
    [SerializeField]
    private float _moveSpeed = 3f;
    
    [SerializeField]
    private float _vMoveSpeed = 0.5f;
    
    [SerializeField]
    private float _jumpVelocity = 6f;

    [SerializeField]
    private float _gravity = 0.5f;
    
    [SerializeField]
    private float _maxFallSpeed = 7f;
    
    [SerializeField]
    private GroundChecker _groundChecker;

    [SerializeField]
    private Rigidbody2D _rigidbody2D;

    [SerializeField]
    private AudioSource _damageSFX;
    
    private bool _isGrounded = false;
    private float _jumpTimer = 0f;
    private float _damageTimer;
    private Animator _animator;
    
    public float VSpeed = 0f;
    public float HSpeed = 0f;

    private void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (InputManager.Instance.Horizontal > 0)
        {
            HSpeed = _moveSpeed;
        }
        else if (InputManager.Instance.Horizontal < 0)
        {
            HSpeed = -_moveSpeed;
        }
        else
        {
            HSpeed = 0f;
        }
        
        if (InputManager.Instance.Vertical > 0 && VSpeed > 0)
        {
            VSpeed += _vMoveSpeed * Time.deltaTime;
        }
        
        if (InputManager.Instance.Vertical < 0 && InputManager.Instance.VerticalDown && VSpeed > 0)
        {
            VSpeed = 1;
        }
        
        _jumpTimer -= Time.deltaTime;
        
        if (InputManager.Instance.VerticalDown 
            && InputManager.Instance.Vertical > 0
            && _groundChecker.IsGrounded 
            && _jumpTimer <= 0)
        {
            VSpeed = _jumpVelocity;
            _jumpTimer = 0.1f;
        }

        if ((VSpeed > 0.5) && (VSpeed < 8)) VSpeed -= 0.6f * Time.deltaTime * _gravity;
        if ((VSpeed > -1) && (VSpeed <= 0.5)) VSpeed -= 0.2f * Time.deltaTime * _gravity;
        if ((VSpeed > -4) && (VSpeed <= -1)) VSpeed -= 0.5f * Time.deltaTime * _gravity;
        if (VSpeed <= -4) VSpeed -= 0.2f * Time.deltaTime * _gravity;
        
        if (_groundChecker.IsGrounded && _jumpTimer <= 0 && VSpeed < 0)
        {
            VSpeed = 0f;
        }

        var collisions = Physics2D.RaycastAll(transform.position, Vector2.right, 0.25f);

        foreach (var collision in collisions)
        {
            if (collision.collider.tag == "BattleField" && HSpeed > 0)
            {
                HSpeed = 0;
            }
        }
        
        collisions = Physics2D.RaycastAll(transform.position, Vector2.left, 0.25f);

        foreach (var collision in collisions)
        {
            if (collision.collider.tag == "BattleField" && HSpeed < 0)
            {
                HSpeed = 0;
            }
        }
        
        collisions = Physics2D.RaycastAll(transform.position, Vector2.up, 0.25f);

        foreach (var collision in collisions)
        {
            if (collision.collider.tag == "BattleField" && VSpeed > 0)
            {
                VSpeed = -0.25f;
            }
        }
        
        _rigidbody2D.position = _rigidbody2D.position.AddX(HSpeed * Time.deltaTime).AddY(VSpeed * Time.deltaTime);

        if (_damageTimer > 0)
        {
            _damageTimer -= Time.deltaTime;

            if (_damageTimer <= 0)
            {
                _animator.Play("Normal");
            }
        }
    }

    public void Damage(int damage)
    {
        if (_damageTimer > 0)
            return;
        
        _damageTimer = 1f;
        _animator.Play("Damage");
        _damageSFX.Play();
        Stats.Instance.HP -= damage;
    }
}
