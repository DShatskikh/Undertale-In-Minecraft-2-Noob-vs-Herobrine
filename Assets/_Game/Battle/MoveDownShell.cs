using UnityEngine;

public sealed class MoveDownShell : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    
    private void Update()
    {
        transform.position -= transform.up * _speed * Time.deltaTime;
    }
}
