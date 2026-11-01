using System;
using UnityEngine;

public sealed class MovedPlatform : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;

    [SerializeField]
    private float _speedY = 0;
    
    [SerializeField]
    private bool _isRight = true;

    [SerializeField]
    private bool _isBorder;
    
    [SerializeField]
    private float _border = 9;

    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }
    
    private void Update()
    {
        var speed = _isRight ? _speed : -_speed;
        transform.position = transform.position.AddX(speed * Time.deltaTime);
        transform.position = transform.position.AddY(_speedY * Time.deltaTime);

        if (_isBorder)
        {
            if (_isRight)
            {
                if (transform.position.x > _border)
                {
                    transform.position = transform.position.SetX(-_border);
                }
            }
            else
            {
                if (transform.position.x < -_border)
                {
                    transform.position = transform.position.SetX(_border);
                }
            }
        }
        
        _collider.enabled = Heart.Instance.transform.position.y - 0.15f >= transform.position.y;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.GetComponent<GroundChecker>())
            return;

        if (Heart.Instance.VSpeed > 0)
            return;
        
        if (_speedY != 0)
        {
            Heart.Instance.VSpeed = 0;
            Heart.Instance.transform.position = Heart.Instance.transform.position.AddY(_speedY * Time.deltaTime);
            other.gameObject.GetComponent<GroundChecker>().IsGrounded = true;
        }
        
        if (Heart.Instance.HSpeed != 0)
            return;
        
        var speed = _isRight ? _speed : -_speed;
        Heart.Instance.transform.position = Heart.Instance.transform.position.AddX(speed * Time.deltaTime);
    }
}
