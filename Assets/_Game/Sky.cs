using UnityEngine;

public sealed class Sky : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1;
    
    private void Update()
    {
        transform.position = transform.position.AddX(Time.deltaTime * _speed);
        
        if (transform.position.x > 8)
            transform.position = transform.position.SetX(-8);
    }
}
