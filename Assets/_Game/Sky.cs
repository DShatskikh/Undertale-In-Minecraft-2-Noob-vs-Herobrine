using UnityEngine;

public sealed class Sky : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1;

    [SerializeField]
    private float _maxX = 8;
    
    [SerializeField]
    private float _minX = -8;
    
    private void Update()
    {
        transform.position = transform.position.AddX(Time.deltaTime * _speed);
        
        if (transform.position.x > _maxX)
            transform.position = transform.position.SetX(_minX);
    }
}
