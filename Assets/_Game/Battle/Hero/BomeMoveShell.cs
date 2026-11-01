using System;
using UnityEngine;

public sealed class BomeMoveShell : MonoBehaviour
{
    [SerializeField]
    private bool _isRight;
    
    [SerializeField]
    private float _border, _speed;
    
    private bool _isSecond;

    private void Update()
    {
        if (!_isSecond)
        {
            if (_isRight)
            {
                if (transform.position.x < -_border)
                {
                    transform.position += Vector3.right * _speed * Time.deltaTime;
                }
                else
                {
                    _isSecond = true;
                }
            }
            else
            {
                if (transform.position.x > _border)
                {
                    transform.position -= Vector3.right * _speed * Time.deltaTime;
                }
                else
                {
                    _isSecond = true;
                }
            }
        }
        else
        {
            if (_isRight)
            {
                transform.position -= Vector3.right * _speed * Time.deltaTime;
            }
            else
            {
                transform.position += Vector3.right * _speed * Time.deltaTime;
            }
        }
    }
}
