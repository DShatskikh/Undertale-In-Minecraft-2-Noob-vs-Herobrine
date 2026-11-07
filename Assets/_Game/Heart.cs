using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    private float _jumpTimer = 0f;
    private float _damageTimer;
    private Animator _animator;
    
    public float VSpeed = 0f;
    public float HSpeed = 0f;
    public Vector2 GravityDirection = Vector2.down;
    public bool IsInclusible => _damageTimer > 0;

    private void OnEnable()
    {
        if (_rigidbody2D)
        {
            _rigidbody2D.linearVelocity = Vector2.zero;
            VSpeed = 0;
        }
    }

    private void OnDisable()
    {
        if (_rigidbody2D)
        {
            _rigidbody2D.linearVelocity = Vector2.zero;
            VSpeed = 0;
        }
    }
    
    private void Awake()
    {
        Instance = this;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GravityDirection == Vector2.down || GravityDirection == Vector2.up)
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
        }
        else
        {
            if (InputManager.Instance.Vertical > 0)
            {
                HSpeed = _moveSpeed;
            }
            else if (InputManager.Instance.Vertical < 0)
            {
                HSpeed = -_moveSpeed;
            }
            else
            {
                HSpeed = 0f;
            } 
        }

        if (GravityDirection == Vector2.down)
        {
            if (InputManager.Instance.Vertical > 0 && VSpeed > 0.9f)
            {
                VSpeed += _vMoveSpeed * Time.deltaTime;
            }
        }
        else if (GravityDirection == Vector2.up)
        {
            if (InputManager.Instance.Vertical < 0 && VSpeed > 0.9f)
            {
                VSpeed += _vMoveSpeed * Time.deltaTime;
            }
        }
        else if (GravityDirection == Vector2.right)
        {
            if (InputManager.Instance.Horizontal < 0 && VSpeed > 0.9f)
            {
                VSpeed += _vMoveSpeed * Time.deltaTime;
            }
        }
        else if (GravityDirection == Vector2.left)
        {
            if (InputManager.Instance.Horizontal > 0 && VSpeed > 0.9f)
            {
                VSpeed += _vMoveSpeed * Time.deltaTime;
            }
        }
        
        if (InputManager.Instance.Vertical < 0 && InputManager.Instance.VerticalDown && VSpeed > 0)
        {
            VSpeed = 1;
        }
        
        _jumpTimer -= Time.deltaTime;

        if (_groundChecker.IsGrounded && _jumpTimer <= 0)
        {
            if (GravityDirection == Vector2.down && InputManager.Instance.VerticalDown && InputManager.Instance.Vertical > 0)
            {
                VSpeed = _jumpVelocity;
                _jumpTimer = 0.1f;
            }
            else if (GravityDirection == Vector2.up && InputManager.Instance.VerticalDown && InputManager.Instance.Vertical < 0)
            {
                VSpeed = _jumpVelocity;
                _jumpTimer = 0.1f;
            }
            else if (GravityDirection == Vector2.right && InputManager.Instance.HorizontalDown && InputManager.Instance.Horizontal < 0)
            {
                VSpeed = _jumpVelocity;
                _jumpTimer = 0.1f;
            }
            else if (GravityDirection == Vector2.left && InputManager.Instance.HorizontalDown && InputManager.Instance.Horizontal > 0)
            {
                VSpeed = _jumpVelocity;
                _jumpTimer = 0.1f;
            }
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

        if (GravityDirection != Vector2.right)
        {
            foreach (var collision in collisions)
            {
                if (collision.collider.CompareTag("BattleField") && HSpeed > 0)
                {
                    HSpeed = 0;
                }
            }
        }

        if (GravityDirection != Vector2.left)
        {
            collisions = Physics2D.RaycastAll(transform.position, Vector2.left, 0.25f);

            foreach (var collision in collisions)
            {
                if (collision.collider.CompareTag("BattleField") && HSpeed < 0)
                {
                    HSpeed = 0;
                }
            }
        }

        if (GravityDirection != Vector2.up)
        {
            if (GravityDirection == Vector2.right || GravityDirection == Vector2.left)
            {
                collisions = Physics2D.RaycastAll(transform.position, Vector2.up, 0.3f);
            }
            else
            {
                collisions = Physics2D.RaycastAll(transform.position, Vector2.up, 0.25f);
            }
            
            foreach (var collision in collisions)
            {
                if (collision.collider.CompareTag("BattleField"))
                {
                    if (GravityDirection == Vector2.right && HSpeed > 0)
                    {
                        HSpeed = 0f;
                    }
                    else if (GravityDirection == Vector2.left && HSpeed > 0)
                    {
                        HSpeed = 0f;
                    }
                    else if (GravityDirection != Vector2.right && GravityDirection != Vector2.left && VSpeed > 0)
                    {
                        VSpeed = -0.25f;
                    }
                }
            }  
        }

        if (GravityDirection == Vector2.right || GravityDirection == Vector2.left)
        {
            collisions = Physics2D.RaycastAll(transform.position, Vector2.down, 0.3f);

            foreach (var collision in collisions)
            {
                if (collision.collider.CompareTag("BattleField"))
                {
                    if (GravityDirection == Vector2.right && HSpeed < 0)
                    {
                        HSpeed = 0f;
                    }
                    else if (GravityDirection == Vector2.left && HSpeed < 0)
                    {
                        HSpeed = 0f;
                    }
                }
            }
        }

        if (GravityDirection == Vector2.down)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            _rigidbody2D.position = _rigidbody2D.position.AddY(VSpeed * Time.deltaTime);
            _rigidbody2D.position = _rigidbody2D.position.AddX(HSpeed * Time.deltaTime);
        }
        else if (GravityDirection == Vector2.up)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
            _rigidbody2D.position = _rigidbody2D.position.AddY(-VSpeed * Time.deltaTime);
            _rigidbody2D.position = _rigidbody2D.position.AddX(HSpeed * Time.deltaTime);
        }
        else if (GravityDirection == Vector2.right)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
            _rigidbody2D.position = _rigidbody2D.position.AddX(-VSpeed * Time.deltaTime);
            _rigidbody2D.position = _rigidbody2D.position.AddY(HSpeed * Time.deltaTime);
        }
        else if (GravityDirection == Vector2.left)
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
            _rigidbody2D.position = _rigidbody2D.position.AddX(VSpeed * Time.deltaTime);
            _rigidbody2D.position = _rigidbody2D.position.AddY(HSpeed * Time.deltaTime);
        }
        
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
        
        _damageTimer = 2f;
        _animator.Play("Damage");
        _damageSFX.Play();
        Stats.Instance.HP -= damage;

        if (Stats.Instance.HP <= 0)
        {
            BattleManager.Instance.RollBackData();
            SceneManager.LoadScene("GameOver");
        }
    }
}
