using System;
using UnityEngine;

public sealed class MoveShell : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;

    [SerializeField]
    private Vector2 _direction = new Vector3(1, 0);
    
    private void Update()
    {
        transform.position += (Vector3)_direction * _speed * Time.deltaTime;
    }
}
